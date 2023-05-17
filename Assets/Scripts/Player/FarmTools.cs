using System;
using CustomHelpers;
using Farming;
using Items;
using Items.ItemData;
using UnityEngine;

namespace Player
{
    public class FarmTools : MonoBehaviour
    {
        [SerializeField] private FarmTile tilledTilePrefab;
        [SerializeField] private LayerMask tileLayer;
        [SerializeField] private LayerMask tillableLayer;

        private Vector3 ToolPosition => toolArea.transform.position;
        private ToolArea toolArea;
        
        private void Start()
        {
            toolArea = ToolArea.Instance;
            var _tile = SpawnTile();
            var _size = _tile.GetComponent<Collider>().bounds.size;
            toolArea.Instantiate(new Vector2(_size.x,_size.y));
            Destroy(_tile.gameObject);
        }
        
        public void TillTile()
        {
            if(toolArea.GetFarmTile()) return;
            if(!toolArea.IsTillable()) return;
            
            FarmTileManager.OnAddFarmTile.Invoke(SpawnTile());
        }

        public void WaterTile()
        {
            var _farmTile = toolArea.GetFarmTile();
            
            if(_farmTile == null) return;
            
            _farmTile.OnWaterPlant();
        }

        private FarmTile SpawnTile()
        {
            var _yPos = toolArea.GetGround().point.y + 0.01f; 
            var _tile = Instantiate(tilledTilePrefab, ToolArea.Instance.transform.position.SetY(_yPos), Quaternion.identity).Initialize();
            _tile.transform.rotation = Quaternion.Euler(90, 0, 0);
            return _tile;
        }

        public void PlantSeed(SeedData itemSeed_)
        {
            var _farmTile = toolArea.GetFarmTile();

            if(_farmTile == null) return;
            if(_farmTile.tileState != TileState.Empty) return;
            
            _farmTile.OnPlantSeed(itemSeed_);
        }
    }
}