using System.Collections.Generic;
using System.Linq;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using UnityEngine;

namespace Farming
{
    public class FarmTileManager : Singleton<FarmTileManager>
    {
        [field: SerializeField] public LayerMask farmTileLayerMask { get; private set; }
        public HashSet<FarmTile> farmTiles { get; private set; } = new HashSet<FarmTile>();

        public static readonly Evt<FarmTile> OnAddFarmTile = new Evt<FarmTile>();

        protected override void Awake()
        {
            base.Awake();
            OnAddFarmTile.AddListener(AddFarmTile);
            transform.position = Vector3.zero;
        }

        private void OnDestroy()
        {
            OnAddFarmTile.RemoveListener(AddFarmTile);
        }
    
        private void AddFarmTile(FarmTile farmTile_)
        {
            farmTiles.Add(farmTile_);
            farmTile_.transform.SetParent(transform);
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
    }
}