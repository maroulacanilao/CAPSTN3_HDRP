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
        private Vector3 ToolPosition => toolArea.transform.position;
        private ToolArea toolArea;
        
        private void Start()
        {
            toolArea = ToolArea.Instance;
        }
        
        public void TillTile()
        {
            if (toolArea.GetFarmTile())
            {
                DestroyTile();
                return;
            }
            if(!toolArea.IsTillable()) return;
            
            FarmTileManager.OnAddFarmTile.Invoke();
        }

        public void WaterTile()
        {
            var _farmTile = toolArea.GetFarmTile();
            
            if(_farmTile == null) return;
            
            _farmTile.OnWaterPlant();
            _farmTile.Heal(new HealInfo(10));
        }

        public void PlantSeed(ItemSeed itemSeed_)
        {
            var _farmTile = toolArea.GetFarmTile();

            if(_farmTile == null) return;
            if(_farmTile.tileState != TileState.Empty) return;
            
            _farmTile.OnPlantSeed(itemSeed_.Data as SeedData);
            itemSeed_.RemoveStack();
            InventoryEvents.OnUpdateStackable.Invoke(itemSeed_);
        }
        
        public void DestroyTile()
        {
            FarmTileManager.OnRemoveTile.Invoke();
        }
    }
}