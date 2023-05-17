using System;
using Items;
using Items.ItemData;
using Managers;
using UnityEngine;

namespace Farming
{
    [System.Serializable]
    public enum TileState { Empty = 0, Planted = 1, Growing = 2, ReadyToHarvest = 3 }   
    
    [System.Serializable]
    public abstract class FarmTileState
    {
        public TileState tileState { get; protected set; }
        protected FarmTile farmTile;
        
        public FarmTileState(FarmTile farmTile_)
        {
            farmTile = farmTile_;
        }
        
        public virtual void EnterLogic() {}

        public virtual void ExitLogic() {}
        public virtual void Interact() {}
        
        public virtual void PlantSeed(SeedData seedData_) { }
        public virtual void WaterPlant() { }
        public virtual ItemConsumable HarvestPlant() { return null; }
    }
    
    [System.Serializable]
    public class EmptyTileState : FarmTileState
    {
        public EmptyTileState(FarmTile farmTile_) : base(farmTile_)
        {
            tileState = TileState.Empty;
        }
        
        public override void EnterLogic()
        {
            //TODO: Add mat to farmtile
            farmTile.seedData = null;
            farmTile.datePlanted = default;

            farmTile.minutesRemaining = 0;
            farmTile.isWatered = false;
            
            farmTile.plantRenderer.gameObject.SetActive(false);
            farmTile.plantRenderer.sprite = farmTile.defaultPlantSprite;
            
            farmTile.soilRenderer.color = farmTile.tilledColor;
            farmTile.soilRenderer.sprite = farmTile.defaultSoilSprite;
        }
        
        public override void WaterPlant()
        {
            farmTile.isWatered = true;
            farmTile.soilRenderer.color = farmTile.wateredColor;
        }

        public override void PlantSeed(SeedData seedData_)
        {
            farmTile.seedData = seedData_;
            
            if(farmTile.isWatered) farmTile.ChangeState(farmTile.GrowingTileState);
            else farmTile.ChangeState(farmTile.plantedTileState);
        }
    }
    
    [System.Serializable]
    public class PlantedTileState : FarmTileState
    {
        public PlantedTileState(FarmTile farmTile_) : base(farmTile_)
        {
            tileState = TileState.Planted;
        }
    
        public override void EnterLogic()
        {
            farmTile.soilRenderer.sprite = farmTile.seedData.soilSprite;
        }
        
        public override void WaterPlant()
        {
            farmTile.isWatered = true;
            farmTile.soilRenderer.color = farmTile.wateredColor;
            farmTile.ChangeState(farmTile.GrowingTileState);
        }
    }
    
    [System.Serializable]
    public class GrowingTileState : FarmTileState
    {
        private bool IsNextPhase = false;
        public GrowingTileState(FarmTile farmTile_) : base(farmTile_)
        {
            tileState = TileState.Growing;
        }
    
        public override void EnterLogic()
        {
            IsNextPhase = false;
            
            farmTile.datePlanted = TimeManager.DateTime;
            farmTile.minutesRemaining = farmTile.totalMinutesDuration;
            
            farmTile.soilRenderer.sprite = farmTile.seedData.soilSprite;
            farmTile.soilRenderer.color = farmTile.wateredColor;
            TimeManager.OnMinuteTick.AddListener(UpdateTimeRemaining);
        }

        public override void ExitLogic()
        {
            TimeManager.OnMinuteTick.RemoveListener(UpdateTimeRemaining);
        }
    
        public override void Interact() { }
        
        private void UpdateTimeRemaining(TimeManager timeManager_)
        {
            if(!farmTile.isWatered) return;
            if (farmTile.minutesRemaining <= 0)
            {
                farmTile.ChangeState(farmTile.readyToHarvestTileState);
                return;
            }
            farmTile.minutesRemaining--;

            if(IsNextPhase) return;
            
            // var _progress = farmTile.timeRemaining.Hours / (float) farmTile.seedData.minutesToGrow;
            Debug.Log(farmTile.progress);
            
            if (farmTile.progress >= 0.1f)
            {
                IsNextPhase = true;
                farmTile.plantRenderer.gameObject.SetActive(true);
                farmTile.soilRenderer.sprite = farmTile.defaultSoilSprite;
                farmTile.plantRenderer.sprite = farmTile.seedData.plantSprite;
            }
        }
    }

    [System.Serializable]
    public class ReadyToHarvestTileState : FarmTileState
    {
        public ReadyToHarvestTileState(FarmTile farmTile_) : base(farmTile_)
        {
            tileState = TileState.ReadyToHarvest;
        }
    
        public override void EnterLogic()
        {
            farmTile.minutesRemaining = 0;
            farmTile.plantRenderer.sprite = farmTile.seedData.harvestSprite;
            // farmTile.meshRenderer.material = farmTile.seedData.readyToHarvestMaterial;
        }

        public override void Interact()
        {
            var _item = farmTile.seedData.produceData.GetConsumableItem();
            GameManager.Instance.GameDataBase.playerInventory.AddItem(_item);
        }
    }
}
