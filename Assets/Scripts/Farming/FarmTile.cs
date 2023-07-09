using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using Items;
using Items.ItemData;
using Managers;
using NaughtyAttributes;
using ObjectPool;
using UI.Farming;
using UnityEngine;

namespace Farming
{
    public class FarmTile : MonoBehaviour, IDamagable, IHealable
    {
        #region Serialized Properties

        [field: SerializeField] [field: BoxGroup("Soil")] public MeshFilter soilMeshFilter { get; private set; }
        [field: SerializeField] [field: BoxGroup("Soil")] public MeshRenderer soilRenderer { get; private set; }
        [field: SerializeField] [field: BoxGroup("Plant")] public MeshFilter plantMeshFilter { get; private set; }
        [field: SerializeField] [field: BoxGroup("Plant")] public SpriteRenderer plantRenderer { get; private set; }
        [field: SerializeField] [field: BoxGroup("Legacy")] public int maxHealth { get; private set; }
        [field: SerializeField] [field: BoxGroup("UI")] public CropUI cropUI { get; private set; }

        #endregion

        public GenericHealth health { get; private set; }
        public DateTime datePlanted { get; set; }
        public TimeSpan timeRemaining { get; set; }
        public SeedData seedData { get; set; }
        public Sprite defaultPlantSprite { get; private set; }

        public bool isWatered { get; set; }
        
        public int totalMinutesDuration => seedData.minutesToGrow;
        public float progress
        {
            get
            {
                switch (tileState)
                {
                    case TileState.Growing:
                        return 1f - (float) timeRemaining.TotalMinutes / totalMinutesDuration;
                    case TileState.ReadyToHarvest:
                        return 1;
                    default:
                        return 0;
                }
            }
        }

        public TileState tileState => currentState?.tileState ?? TileState.Empty;

        #region States
        
        public EmptyTileState emptyTileState { get; private set; }
        public PlantedTileState plantedTileState { get; private set; }
        public GrowingTileState growingTileState { get; private set; }
        public ReadyToHarvestTileState readyToHarvestTileState { get; private set; }

        #endregion
        
        [SerializeReference] private FarmTileState currentState;

        public readonly Evt<TileState> OnChangeState = new Evt<TileState>();
        private IPoolable poolableImplementation;

        private void Awake()
        {
            health = new GenericHealth(maxHealth);
            defaultPlantSprite = plantRenderer.sprite;
        }

        public FarmTile Initialize()
        {
            emptyTileState = new EmptyTileState(this);
            plantedTileState = new PlantedTileState(this);
            growingTileState = new GrowingTileState(this);
            readyToHarvestTileState = new ReadyToHarvestTileState(this);
            
            ChangeState(emptyTileState);
            return this;
        }
        
        public void ChangeState(FarmTileState farmTileState_)
        {
            currentState?.ExitLogic();
            currentState = farmTileState_;
            currentState?.EnterLogic();
            
            OnChangeState.Invoke(tileState);
        }

        #region Action

        public void OnWaterPlant()
        {
            currentState?.WaterPlant();
            Heal(new HealInfo(10));
        }
        
        public void OnPlantSeed(SeedData seedData_)
        {
            currentState?.PlantSeed(seedData_);
        }
        
        public void OnInteract()
        {
            if(tileState != TileState.ReadyToHarvest) return;
            
            currentState?.Interact();
        }

        #endregion

        #region Interaction

        public void Enter()
        {
            cropUI.gameObject.SetActive(true);
        }
        
        public void Exit()
        {
            cropUI.gameObject.SetActive(false);
        }

        #endregion

        public void Load(SaveSystem.FarmTileSaveData saveData_, SeedData seedData_)
        {
            Initialize();
            
            Debug.Log("Load TILE");
            Debug.Log("Date Planted: " + saveData_.datePlanted);
            Debug.Log("Time Remaining: " + saveData_.minutesRemaining);

            if (!StringHelpers.TryParseVector3(saveData_.position, out var _position))
            {
                Debug.LogError("Cant Parse Vector3");
                return;
            }
            if(!StringHelpers.TryParseQuaternion(saveData_.rotation, out var _rotation))
            {
                Debug.LogError("Cant Parse Quaternion");
                return;
            }
            
            var _transform = transform;
            
            _transform.position = _position;
            _transform.rotation = _rotation;
            
            Debug.Log(seedData_.ItemName);
            seedData = seedData_;
            
            switch (saveData_.tileState)
            {
                case TileState.Planted:
                    isWatered = false;
                    ChangeState(plantedTileState);
                    break;
                case TileState.Growing:
                    isWatered = true;
                    
                    ChangeState(growingTileState);
                    
                    if(DateTime.TryParseExact(saveData_.datePlanted, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var _datePlanted))
                    {
                        datePlanted = _datePlanted;
                    }
                    
                    if (double.TryParse(saveData_.minutesRemaining, out var _minutes))
                    {
                        timeRemaining = TimeSpan.FromMinutes(_minutes);
                        Debug.Log($"Time Remaining: {timeRemaining.TotalMinutes}");
                    }
                    
                    var _state = currentState as GrowingTileState;
                    
                    _state.UpdateAppearance();
                    break;
                case TileState.ReadyToHarvest:
                    ChangeState(readyToHarvestTileState);
                    break;
                case TileState.Empty:
                default:
                    ChangeState(emptyTileState);
                    break;
            }
        }
        
        #region Legacy

        public void TakeDamage(DamageInfo damageInfo_)
        {
            health.AddHealth(-damageInfo_.DamageAmount);
            
            if(health.CurrentHealth > 0) return;
            FarmTileManager.RemoveTile(this);
        }
        
        public void Heal(HealInfo healInfo_, bool isOverHeal_ = false)
        {
            health.AddHealth(healInfo_.HealAmount);
        }

        #endregion
    }
}