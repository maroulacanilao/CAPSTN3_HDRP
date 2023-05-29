using System;
using System.Collections.Generic;
using System.Linq;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Farming
{
    public class FarmTileManager : Singleton<FarmTileManager>
    {
        [field: SerializeField] public FarmTile farmTilePrefab { get; private set; }
        [field: SerializeField] public ToolArea toolArea { get; private set; }
        [field: SerializeField] public LayerMask farmTileLayerMask { get; private set; }
        [field: SerializeField] public LayerMask farmGroundLayerMask { get; private set; }
        
        public HashSet<FarmTile> farmTiles { get; private set; } = new HashSet<FarmTile>();
        

        public static readonly Evt OnAddFarmTile = new Evt();
        public static readonly Evt OnRemoveTile = new Evt();

        protected override void Awake()
        {
            base.Awake();
            OnAddFarmTile.AddListener(AddFarmTile);
            OnRemoveTile.AddListener(RemoveTile);
            transform.position = Vector3.zero;
        }

        private void Start()
        {
            var _tile = SpawnTileAtToolArea();
            var _size = _tile.GetComponent<Collider>().bounds.size;
            
            toolArea.Instantiate(new Vector2(_size.x,_size.y), farmTileLayerMask, farmGroundLayerMask);
            
            Destroy(_tile.gameObject);
        }

        private void OnDestroy()
        {
            OnAddFarmTile.RemoveListener(AddFarmTile);
            OnRemoveTile.RemoveListener(RemoveTile);
        }
    
        private void AddFarmTile()
        {
            var _tile = SpawnTileAtToolArea();
            farmTiles.Add(_tile);
            _tile.transform.SetParent(transform);
        }

        public List<FarmTile> GetAllNonEmptyTile()
        {
            PurgeNulls();
            return farmTiles.Where(f => f.IsValid() && f.tileState != TileState.Empty).ToList();
        }

        private void PurgeNulls()
        {
            for (int i = farmTiles.Count - 1; i >= 0; i--)
            {
                var _tile = farmTiles.ElementAt(i);
                if(_tile.IsEmptyOrDestroyed()) farmTiles.Remove(_tile);
            }
        }
        
        public bool HasNonEmptyTile()
        {
            PurgeNulls();
            return farmTiles.Any(f => f.IsValid() && f.tileState != TileState.Empty);
        }
        
        private void RemoveTile()
        {
            var _farmTile = toolArea.GetFarmTile();

            if(_farmTile == null) return;
            if(_farmTile.IsEmptyOrDestroyed()) return;
            
            if(_farmTile.tileState != TileState.Empty) return;
            
            farmTiles.Remove(_farmTile); 
            Destroy(_farmTile.gameObject);
        }

        public FarmTile SpawnTileAtToolArea()
        {
            var _yPos = toolArea.GetGround().point.y + 0.01f;
            var _tile = Instantiate(farmTilePrefab, ToolArea.Instance.transform.position.SetY(_yPos), Quaternion.identity);
            _tile.Initialize();
            _tile.transform.rotation = Quaternion.Euler(90, 0, 0);
            return _tile;
        }
    }
}