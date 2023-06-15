using System;
using BaseCore;
using Character;
using CustomEvent;
using Farming;
using Fungus;
using Items;
using Items.Inventory;
using Items.ItemData;
using Items.ItemData.Tools;
using Managers;
using UnityEngine;

namespace Player
{
    public enum EquipmentAction { Interact, Till, Water, Plant, Harvest, UnTill, Consume, None }
    
    public class PlayerEquipment : MonoBehaviour
    {
        [field: SerializeField] public PlayerCharacter player { get; private set; }
        [field: SerializeField] public InteractDetector interactDetector { get; private set; }

        private PlayerInventory playerInventory;
        
        private ToolArea toolArea;
        public int currIndex { get; private set; }
        public Item CurrentItem => playerInventory.ItemTools[currIndex];
        
        public static readonly Evt<int> OnChangeItemOnHand = new Evt<int>();

        private void Start()
        {
            playerInventory = player.playerInventory;
            currIndex = 0;

            toolArea = ToolArea.Instance;
            
            OnChangeItemOnHand.Invoke(0);
        }

        private void OnEnable()
        {
            InputManager.OnCycleTool.AddListener(CycleTool);
            InputManager.OnSelectTool.AddListener(SelectTool);
        }
        
        private void OnDisable()
        {
            InputManager.OnCycleTool.RemoveListener(CycleTool);
            InputManager.OnSelectTool.RemoveListener(SelectTool);
        }

        public void UseTool()
        {
            Debug.Log(CurrentItem);
            // _equipmentActions[currIndex]?.Invoke();
            var _action = GetEquipmentAction();
            DoEquipmentAction(_action);
        }

        public void CycleTool(bool isNext_)
        {
            if (isNext_) currIndex++;
            else currIndex--;
            ClampIndex();
            OnChangeItemOnHand.Invoke(currIndex);
        }

        public void SelectTool(int index_)
        {
            currIndex = index_;
            ClampIndex();
            OnChangeItemOnHand.Invoke(currIndex);
        }

        private void ClampIndex()
        {
            currIndex %= playerInventory.ItemTools.Length;
            if (currIndex < 0) currIndex = playerInventory.ItemTools.Length - 1;
        }

        public EquipmentAction GetEquipmentAction()
        {
            if(CurrentItem == null) return CanInteract() ? EquipmentAction.Interact : EquipmentAction.None;
            
            if (CurrentItem.Data is HoeData)
            {
                var _farmTile = toolArea.GetFarmTile();

                if (_farmTile != null)
                {
                    if (_farmTile.tileState == TileState.ReadyToHarvest)
                    {
                        return EquipmentAction.Harvest;
                    }
                    return _farmTile.tileState == TileState.Empty ? EquipmentAction.UnTill : EquipmentAction.None;
                }
                
                if (!toolArea.IsTillable())
                {
                    return CanInteract() ? EquipmentAction.Interact : EquipmentAction.None;
                }

                return EquipmentAction.Till;

            }
            
            if (CurrentItem.Data is WateringCanData)
            {
                var _farmTile = toolArea.GetFarmTile();

                if (_farmTile == null)
                {
                    return CanInteract() ? EquipmentAction.Interact : EquipmentAction.None;
                }
            
                if(_farmTile.tileState == TileState.ReadyToHarvest) return EquipmentAction.Harvest;

                return EquipmentAction.Water;
            }

            if (CurrentItem is ItemSeed)
            {
                var _farmTile = toolArea.GetFarmTile();

                if (_farmTile == null || _farmTile.tileState != TileState.Empty)
                {
                    return CanInteract() ? EquipmentAction.Interact : EquipmentAction.None;
                }

                return EquipmentAction.Plant;
            }
            
            if (CurrentItem is ItemConsumable) return EquipmentAction.Consume;
            
            return CanInteract() ? EquipmentAction.Interact : EquipmentAction.None;
        }

        private void DoEquipmentAction(EquipmentAction action_)
        {
            switch (action_)
            {
                case EquipmentAction.None:
                    break;
                case EquipmentAction.Till:
                    FarmTileManager.AddFarmTileAtToolLocation();
                    break;
                case EquipmentAction.Interact:
                    if(CanInteract()) interactDetector.Interact();
                    break;
                case EquipmentAction.Water:
                    Water();
                    break;
                case EquipmentAction.Plant:
                    Plant();
                    break;
                case EquipmentAction.Harvest:
                    Harvest();
                    break;
                case EquipmentAction.UnTill:
                    FarmTileManager.RemoveTileAtToolLocation();
                    break;
            }
        }

        private void Water()
        {
            var _farmTile = toolArea.GetFarmTile();
            
            if(_farmTile == null) return;
            
            _farmTile.OnWaterPlant();
            _farmTile.Heal(new HealInfo(10));
        }

        private void Plant()
        {
            if(CurrentItem is not {Data: SeedData}) return;
            var _tile = toolArea.GetFarmTile();
            if (_tile == null) return;
            
            _tile.OnPlantSeed(CurrentItem.Data as SeedData);
            var _itemSeed = (ItemSeed) CurrentItem;
            _itemSeed.RemoveStack();
            InventoryEvents.OnUpdateStackable.Invoke(_itemSeed);
        }

        private void Harvest()
        {
            var _tile = toolArea.GetFarmTile();
            if (_tile == null) return;
            
            if(_tile.tileState != TileState.ReadyToHarvest) return;
            if(_tile.seedData == null) return;
            
            _tile.OnInteract();
        }
        
        private bool CanInteract()
        {
            return interactDetector.nearestInteractable != null && interactDetector.nearestInteractable.canInteract;
        }
    }
}