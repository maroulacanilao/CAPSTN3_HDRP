using BaseCore;
using UI.Farming;
using UnityEngine;

namespace Farming
{
    public class FarmTileInteractable : InteractableObject
    {
        [field: SerializeField] public FarmTile farmTile { get; private set; }
        [SerializeField] private CropUI cropUI;

        protected override void Interact()
        { 
            if (farmTile.tileState != TileState.ReadyToHarvest) return;
            farmTile.OnInteract();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            cropUI.gameObject.SetActive(false);
        }

        protected override void Enter()
        {
            cropUI.gameObject.SetActive(true);
            
        }
        
        protected override void Exit()
        {
            cropUI.gameObject.SetActive(false);
        }
    }
}
