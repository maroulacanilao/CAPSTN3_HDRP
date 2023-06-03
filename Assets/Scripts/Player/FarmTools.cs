using System;
using BaseCore;
using CustomHelpers;
using Farming;
using Items;
using Items.Inventory;
using Items.ItemData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Player
{
    public class FarmTools : MonoBehaviour
    {
        private ToolArea toolArea;
        
        private void Start()
        {
            toolArea = ToolArea.Instance;
        }
        
        public bool TillTile()
        {
            var _tile = toolArea.GetFarmTile();
            
            if (_tile == null)
            {
                if(!toolArea.IsTillable()) return false;
            
                FarmTileManager.AddFarmTileAtToolLocation();
                return true;
            }
            
            if (TryHarvest(_tile)) return true;
            
           DestroyTile();
           return true;
        }

        public bool WaterTile()
        {
            var _farmTile = toolArea.GetFarmTile();
            
            if(_farmTile == null) return false;
            
            if(TryHarvest(_farmTile)) return true;

            if (_farmTile.tileState == TileState.ReadyToHarvest) return true;
            
            _farmTile.OnWaterPlant();
            _farmTile.Heal(new HealInfo(10));
            return true;
        }

        public bool PlantSeed(ItemSeed itemSeed_)
        {
            var _farmTile = toolArea.GetFarmTile();

            if(_farmTile == null) return false;
            
            if(TryHarvest(_farmTile)) return true;
            
            if(_farmTile.tileState != TileState.Empty) return false;
            
            _farmTile.OnPlantSeed(itemSeed_.Data as SeedData);
            itemSeed_.RemoveStack();
            InventoryEvents.OnUpdateStackable.Invoke(itemSeed_);
            return true;
        }
        
        public void DestroyTile()
        {
            FarmTileManager.RemoveTileAtToolLocation();
        }

        public bool TryHarvest(FarmTile tile_)
        {
            if(tile_.tileState != TileState.ReadyToHarvest) return false;
            if(tile_.seedData == null) return false;
            
            tile_.OnInteract();
            return true;
        }
    }
}