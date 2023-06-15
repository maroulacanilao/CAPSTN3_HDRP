using System;
using System.Collections.Generic;
using System.Linq;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using ObjectPool;
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
        

        public static readonly Evt<FarmTile> OnAddFarmTile = new Evt<FarmTile>();
        public static readonly Evt<FarmTile> OnRemoveTile = new Evt<FarmTile>();

        protected override void Awake()
        {
            base.Awake();
            
            transform.position = Vector3.zero;
        }

        private void Start()
        {
            var _tile = SpawnTileAtToolArea();
            var _size = _tile.GetComponent<Collider>().bounds.size;
            
            Debug.Log(_size);
            toolArea.Initialize(new Vector2(_size.x,_size.z), farmTileLayerMask, farmGroundLayerMask);
            
            _tile.gameObject.ReturnInstance();
        }

        private void OnDestroy()
        {
            
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
        
        public FarmTile[] GetNonEmptyTiles()
        {
            PurgeNulls();
            return farmTiles.Where(f_ => f_.IsValid() && f_.tileState != TileState.Empty).ToArray();
        }
        
        public static void AddFarmTileAtToolLocation()
        {
            if(Instance.IsEmptyOrDestroyed()) return;
            var _tile = Instance.SpawnTileAtToolArea();
            Instance.farmTiles.Add(_tile);
            _tile.transform.SetParent(Instance.transform);
            OnAddFarmTile.Invoke(_tile);
        }
        
        public static void RemoveTileAtToolLocation()
        {
            if(Instance.IsEmptyOrDestroyed()) return;
            var _farmTile = Instance.toolArea.GetFarmTile();
            
            if(_farmTile.IsEmptyOrDestroyed()) return;
            
            if(_farmTile.tileState != TileState.Empty) return;
            
            Instance.farmTiles.Remove(_farmTile);
            OnRemoveTile.Invoke(_farmTile);
            Destroy(_farmTile.gameObject);
        }
        
        public static void RemoveTile(FarmTile farmTile_)
        {
            if(Instance.IsEmptyOrDestroyed()) return;
            if(farmTile_.IsEmptyOrDestroyed()) return;
            
            Instance.farmTiles.Remove(farmTile_);
            OnRemoveTile.Invoke(farmTile_);
            farmTile_.gameObject.ReturnInstance();
        }

        public FarmTile SpawnTileAtToolArea()
        {
            var _yPos = toolArea.GetGround().point.y + 0.01f;

            var _tile = farmTilePrefab.gameObject
                .GetInstance<FarmTile>(ToolArea.Instance.transform.position.SetY(_yPos), Quaternion.identity)
                .Initialize();
            
            //Instantiate(farmTilePrefab, ToolArea.Instance.transform.position.SetY(_yPos), Quaternion.identity);
            _tile.Initialize();
            _tile.transform.rotation = Quaternion.Euler(90, 0, 0);
            return _tile;
        }
    }
}