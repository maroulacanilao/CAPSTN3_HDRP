using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using Items.Inventory;
using Items.ItemData;
using ObjectPool;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Farming
{
    public class FarmTileManager : Singleton<FarmTileManager>
    {
        [field: SerializeField] public FarmTile farmTilePrefab { get; private set; }
        [field: SerializeField] public LayerMask farmTileLayerMask { get; private set; }
        [field: SerializeField] public LayerMask farmGroundLayerMask { get; private set; }
        
        [field: SerializeField] public SerializedDictionary<SeedData, int> plantedSeeds { get; private set; } = new SerializedDictionary<SeedData, int>();
        [SerializeField] private GameDataBase gameDataBase;

        public readonly HashSet<FarmTile> farmTiles = new HashSet<FarmTile>();
        
        public static readonly Evt<FarmTile> OnAddFarmTile = new Evt<FarmTile>();
        public static readonly Evt<FarmTile> OnRemoveTile = new Evt<FarmTile>();
        public static readonly Evt<SeedData> OnHarvestCrop = new Evt<SeedData>();

        private ToolArea toolArea;
        private PlayerData playerData => gameDataBase.playerData;
        private PlayerInventory inventory => playerData.inventory;
        private PlayerLevel playerLevel => playerData.LevelData;
        private Vector3 tileSize => toolArea.size;

        protected override void Awake()
        {
            base.Awake();
            OnHarvestCrop.AddListener(HarvestCrop);
            transform.position = Vector3.zero;
        }

        private void Start()
        {
            toolArea = ToolArea.Instance;
        }

        private void OnDestroy()
        {
            OnHarvestCrop.AddListener(HarvestCrop);
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
            var _yPos = toolArea.GetGround().point.y + 0.01f + (toolArea.size.y / 2f);

            var _tile = farmTilePrefab.gameObject
                .GetInstance<FarmTile>(ToolArea.Instance.transform.position.SetY(_yPos), Quaternion.identity)
                .Initialize();
            
            //Instantiate(farmTilePrefab, ToolArea.Instance.transform.position.SetY(_yPos), Quaternion.identity);
            _tile.Initialize();
            // _tile.transform.rotation = Quaternion.Euler(90, 0, 0);
            return _tile;
        }

        public SerializedDictionary<SeedData, int> GetPlantedData()
        {
            plantedSeeds.Clear();
            
            foreach (var _tile in farmTiles)
            {
                if(_tile.IsEmptyOrDestroyed()) continue;
                if(!_tile.gameObject.activeInHierarchy) continue;
                if(_tile.tileState == TileState.Empty) continue;
                if(_tile.seedData == null) continue;

                if(plantedSeeds.ContainsKey(_tile.seedData)) plantedSeeds[_tile.seedData]++;
                
                else plantedSeeds.Add(_tile.seedData, 1);
            }
            
            return plantedSeeds;
        }
        
        public void HarvestCrop(SeedData seedData_)
        {
            if(seedData_ == null) return;
            // if(!plantedSeeds.ContainsKey(seedData_)) return;
            // if(plantedSeeds[seedData_] <= 0) return;
            //
            // plantedSeeds[seedData_]--;
            // if(plantedSeeds[seedData_] <= 0) plantedSeeds.Remove(seedData_);

            var _count = seedData_.producePossibleCount.GetRandomInRange();
            var _item = seedData_.produceData.GetConsumableItem(_count);
            inventory.AddItem(_item);
            
            playerLevel.AddExp(seedData_.expReward);
        }

        public static void AddTileManual(FarmTile farmTile_)
        {
            if(Instance.IsEmptyOrDestroyed()) return;
            Instance.farmTiles.Add(farmTile_);
            OnAddFarmTile.Invoke(farmTile_);
        }

        public static void AddTilesManual(List<FarmTile> tiles_)
        {
            if(Instance.IsEmptyOrDestroyed()) return;
            Instance.farmTiles.UnionWith(tiles_);
        }
    }
}