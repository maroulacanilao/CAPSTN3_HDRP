using System.Collections.Generic;
using BaseCore;
using CustomEvent;
using UnityEngine;

namespace Farming
{
    public class FarmTileManager : Singleton<FarmTileManager>
    {
        [SerializeField] private Material tilledMaterial;
        [SerializeField] private Material plantedMaterial;
        [SerializeField] private Material wateredMaterial;
        [SerializeField] private Material growingMaterial;
        
        private HashSet<FarmTile> farmTiles = new HashSet<FarmTile>();
    
        public static readonly Evt<FarmTile> OnAddFarmTile = new Evt<FarmTile>();
        
        public static Material TilledMaterial => Instance.tilledMaterial;
        public static Material PlantedMaterial => Instance.plantedMaterial;
        public static Material WateredMaterial => Instance.wateredMaterial;
        public static Material GrowingMaterial => Instance.growingMaterial;

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
    }
}