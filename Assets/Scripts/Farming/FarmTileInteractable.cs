using BaseCore;
using UI.Farming;
using UnityEngine;

namespace Farming
{
    public class FarmTileInteractable : InteractableObject
    {
        [field: SerializeField] public FarmTile farmTile { get; private set; }
        [SerializeField] private CropUI cropUI;

        protected override void OnEnable()
        {
            base.OnEnable();
            farmTile.OnChangeState.AddListener(ChangeState);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            farmTile.OnChangeState.RemoveListener(ChangeState);
            cropUI.gameObject.SetActive(false);
        }

        protected override void Interact()
        { 
            if (farmTile.tileState != TileState.ReadyToHarvest) return;
            farmTile.OnInteract();
        }

        protected override void Enter()
        {
            cropUI.gameObject.SetActive(true);
        }
        
        protected override void Exit()
        {
            cropUI.gameObject.SetActive(false);
        }
        
        private void ChangeState(FarmTile tile_, TileState tileState_)
        {
            canInteract = tileState_ == TileState.Planted;
        }
        
        
    }
}
