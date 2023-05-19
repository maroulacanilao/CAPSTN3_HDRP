using BaseCore;
using CustomEvent;
using UnityEngine;

namespace Farming
{
    public class FarmTileInteractable : InteractableObject
    {
        [field: SerializeField] public FarmTile farmTile { get; private set; }
        
        public static readonly Evt<FarmTile> OnEnterFarmTile = new Evt<FarmTile>();
        public static readonly Evt<FarmTile> OnExitFarmTile = new Evt<FarmTile>();
        protected override void Interact()
        { 
            if (farmTile.tileState != TileState.ReadyToHarvest) return;
            Debug.Log("Harvest");
            farmTile.OnInteract();
            showIcon = false;
            OnExit.Invoke(this);
            OnExitFarmTile.Invoke(farmTile);
        }
        
        protected override void Enter()
        {
            if(farmTile == null) return;
            
            if (farmTile.tileState == TileState.ReadyToHarvest && 
                !showIcon)
            {
                showIcon = true;
                OnEnter.Invoke(this);
            }
            if(farmTile.tileState == TileState.Empty) return;
            OnEnterFarmTile.Invoke(farmTile);
        }
        
        protected override void Exit()
        {
            // Close UI
            OnExitFarmTile.Invoke(farmTile);
        }
    }
}
