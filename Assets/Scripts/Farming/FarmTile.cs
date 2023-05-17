using System;
using System.ComponentModel;
using CustomHelpers;
using Items;
using Items.ItemData;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace Farming
{
    public class FarmTile : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer soilRenderer { get; private set; }
        [field: SerializeField] public SpriteRenderer plantRenderer { get; private set; }
        
        [field: SerializeField] public Color tilledColor { get; private set; }
        [field: SerializeField] public Color wateredColor { get; private set; }

        public DateTime datePlanted { get; set; }

        public SeedData seedData { get; set; }

        public bool isWatered { get; set; }
        
        public int minutesRemaining { get; set; }
        public int totalMinutesDuration => seedData.minutesToGrow;
        public Sprite defaultSoilSprite;
        public Sprite defaultPlantSprite;
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
        
        public TileState tileState => currentState.tileState;

        #region States
        
        public EmptyTileState emptyTileState;
        public PlantedTileState plantedTileState;
        public GrowingTileState GrowingTileState;
        public ReadyToHarvestTileState readyToHarvestTileState;

        #endregion
        
        [SerializeReference] private FarmTileState currentState;
        public FarmTile Initialize()
        {
            defaultSoilSprite = soilRenderer.sprite;
            defaultPlantSprite = plantRenderer.sprite;
            emptyTileState = new EmptyTileState(this);
            plantedTileState = new PlantedTileState(this);
            GrowingTileState = new GrowingTileState(this);
            readyToHarvestTileState = new ReadyToHarvestTileState(this);
            
            ChangeState(emptyTileState);
            return this;
        }

        public void ChangeState(FarmTileState farmTileState_)
        {
            currentState?.ExitLogic();
            currentState = farmTileState_;
            currentState?.EnterLogic();
        }

        public void OnWaterPlant()
        {
            currentState.WaterPlant();
        }
        
        public void OnPlantSeed(SeedData seedData_)
        {
            currentState.PlantSeed(seedData_);
        }
        
        public void OnInteract()
        {
            currentState.Interact();
        }
    }
}