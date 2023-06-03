using System;
using System.ComponentModel;
using System.Threading.Tasks;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using Items;
using Items.ItemData;
using Managers;
using NaughtyAttributes;
using UI.Farming;
using UnityEngine;

namespace Farming
{
    public class FarmTile : MonoBehaviour, IDamagable, IHealable
    {
        [field: SerializeField] public SpriteRenderer soilRenderer { get; private set; }
        [field: SerializeField] public SpriteRenderer plantRenderer { get; private set; }
        [field: SerializeField] public Color tilledColor { get; private set; }
        [field: SerializeField] public Color wateredColor { get; private set; }
        [field: SerializeField] public int maxHealth { get; private set; }

        public GenericHealth health { get; private set; }
        public DateTime datePlanted { get; set; }

        public SeedData seedData { get; set; }

        public Sprite defaultSoilSprite { get; private set; }
        public Sprite defaultPlantSprite { get; private set; }
        
        public bool isWatered { get; set; }
        
        public int minutesRemaining { get; set; }
        public int totalMinutesDuration => seedData.minutesToGrow;
        public float progress
        {
            get
            {
                switch (tileState)
                {
                    case TileState.Growing:
                        return 1 - (minutesRemaining / (float) totalMinutesDuration);
                    case TileState.ReadyToHarvest:
                        return 1;
                    default:
                        return 0;
                }
            }
        }

        public TileState tileState => currentState?.tileState ?? TileState.Empty;

        #region States
        
        public EmptyTileState emptyTileState;
        public PlantedTileState plantedTileState;
        public GrowingTileState growingTileState;
        public ReadyToHarvestTileState readyToHarvestTileState;

        #endregion
        
        [SerializeReference] private FarmTileState currentState;

        public readonly Evt<TileState> OnChangeState = new Evt<TileState>();

        public FarmTile Initialize()
        {
            defaultSoilSprite = soilRenderer.sprite;
            defaultPlantSprite = plantRenderer.sprite;

            health = new GenericHealth(maxHealth);
            
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

        public void OnWaterPlant()
        {
            currentState?.WaterPlant();
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
    }
}