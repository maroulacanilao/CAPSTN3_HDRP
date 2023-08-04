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
    public enum TileState { Empty = 0, Planted = 1, Growing = 2, ReadyToHarvest = 3,Growing2 = 4 }

    [System.Serializable]
    public abstract class FarmTileState
    {
        public TileState tileState { get; protected set; }
        protected FarmTile FarmTile;
        protected MeshFilter SoilMesh;
        protected MeshRenderer SoilRenderer;

        public FarmTileState(FarmTile farmTile_)
        {
            FarmTile = farmTile_;
            SoilMesh = FarmTile.soilMeshFilter;
            SoilRenderer = FarmTile.soilRenderer;
        }

        public virtual void EnterLogic() { }

        public virtual void ExitLogic() { }
        public virtual void Interact() { }

        public virtual void PlantSeed(SeedData seedData_) { }
        public virtual void WaterPlant() { }

        public virtual void ChangeRenderer()
        {
            SoilMesh.mesh = tileState.GetTileMesh();
        }

        public virtual void ChangeMaterial()
        {
            SoilRenderer.material = AssetHelper.GetTileMaterial(FarmTile.isWatered);
        }

        public virtual void ChangePlantMesh()
        {
            var _mesh = tileState == TileState.Empty || FarmTile.seedData == null ? null : FarmTile.seedData.plantSprite;
        }
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
            FarmTile.seedData = null;
            FarmTile.datePlanted = default;
            FarmTile.isWatered = false;
            FarmTile.timeRemaining = default;

            FarmTile.plantRenderer.gameObject.SetActive(false);
            FarmTile.plantRenderer.sprite = null;

            ChangeRenderer();
            ChangeMaterial();

            FarmTile.health.RefillHealth();
        }

        public override void WaterPlant()
        {
            FarmTile.isWatered = true;
            ChangeMaterial();
        }

        public override void PlantSeed(SeedData seedData_)
        {
            FarmTile.seedData = seedData_;
            
            AudioManager.PlayPlanting();

            if (FarmTile.isWatered) FarmTile.ChangeState(FarmTile.growingTileState);
            else FarmTile.ChangeState(FarmTile.plantedTileState);
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
            // farmTile.soilMeshFilter.sprite = farmTile.seedData.soilSprite;
            ChangeRenderer();
        }

        public override void WaterPlant()
        {
            FarmTile.isWatered = true;
            ChangeMaterial();
            FarmTile.ChangeState(FarmTile.growingTileState);
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
            FarmTile.timeRemaining = default;
            FarmTile.plantRenderer.sprite = FarmTile.seedData.readyToHarvestSprite;
            ChangeRenderer();
            // farmTile.soilMeshFilter.material = farmTile.seedData.readyToHarvestMaterial;
        }

        public override void Interact()
        {
            FarmTileManager.OnHarvestCrop.Invoke(FarmTile.seedData);
            FarmTile.ChangeState(FarmTile.emptyTileState);
            FarmTile.health.RefillHealth();
            AudioManager.PlayHarvesting();
        }
    }
}
