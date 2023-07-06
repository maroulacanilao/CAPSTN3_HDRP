using System;
using CustomHelpers;
using Items.ItemData;
using Managers;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
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

        public virtual void EnterLogic() { }

        public virtual void ExitLogic() { }
        public virtual void Interact() { }

        public virtual void PlantSeed(SeedData seedData_) { }
        public virtual void WaterPlant() { }
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
            farmTile.isWatered = false;
            farmTile.timeRemaining = default;

            farmTile.plantRenderer.gameObject.SetActive(false);
            farmTile.plantRenderer.sprite = farmTile.defaultPlantSprite;

            // ToDO: Add mat to farmtile
            // farmTile.soilRenderer.color = farmTile.tilledColor;
            // farmTile.soilRenderer.sprite = farmTile.defaultSoilSprite;

            farmTile.health.RefillHealth();
        }

        public override void WaterPlant()
        {
            farmTile.isWatered = true;
            // ToDO: Add mat to farmtile
            // farmTile.soilRenderer.color = farmTile.wateredColor;
        }

        public override void PlantSeed(SeedData seedData_)
        {
            farmTile.seedData = seedData_;

            if (farmTile.isWatered) farmTile.ChangeState(farmTile.growingTileState);
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
            // ToDO: Add mat to farmtile
            // farmTile.soilRenderer.sprite = farmTile.seedData.soilSprite;
        }

        public override void WaterPlant()
        {
            farmTile.isWatered = true;
            // ToDO: Add mat to farmtile
            // farmTile.soilRenderer.color = farmTile.wateredColor;
            farmTile.ChangeState(farmTile.growingTileState);
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
            farmTile.timeRemaining = default;
            farmTile.plantRenderer.sprite = farmTile.seedData.readyToHarvestSprite;
            // farmTile.meshRenderer.material = farmTile.seedData.readyToHarvestMaterial;
        }

        public override void Interact()
        {
            // var _item = farmTile.seedData.produceData.GetConsumableItem(1);
            // GameManager.Instance.GameDataBase.playerInventory.AddItem(_item);
            FarmTileManager.OnHarvestCrop.Invoke(farmTile.seedData);
            farmTile.ChangeState(farmTile.emptyTileState);
            farmTile.health.RefillHealth();
        }
    }
}
