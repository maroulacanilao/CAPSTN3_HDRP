using BaseCore;
using Character;
using CustomEvent;
using Farming;
using Items;
using Items.Inventory;
using Items.ItemData;
using Items.ItemData.Tools;
using Managers;
using UnityEngine;

namespace Player
{
    public enum EquipmentAction { Till, Water, Plant, Harvest, UnTill, Consume, None }
    
    [DefaultExecutionOrder(-1)]
    public class PlayerEquipment : MonoBehaviour
    {
        [field: SerializeField] public PlayerCharacter player { get; private set; }
        [SerializeField] private InteractDetector interactDetector;
        [SerializeField] private ToolArea toolArea;

        private PlayerInventory playerInventory => player.playerInventory;

        public int currIndex { get; private set; }
        public Item CurrentItem => playerInventory.ItemTools[currIndex];
        
        public static readonly Evt<int> OnChangeItemOnHand = new Evt<int>();
        public static readonly Evt OnTillAction = new Evt();
        public static readonly Evt OnWaterAction = new Evt();
        public static readonly Evt OnUnTillAction = new Evt();

        private void Start()
        {
            currIndex = 0;
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
            var _nearestInteractable = interactDetector.nearestInteractable;

            if (_nearestInteractable != null && _nearestInteractable is FarmTileInteractable _farmTileInteractable)
            {
                if(_farmTileInteractable.farmTile.tileState == TileState.ReadyToHarvest) return EquipmentAction.Harvest;
            }
            
            if(CurrentItem == null) return EquipmentAction.None;

            var _tile = toolArea.GetFarmTile();
            
            
            if (CurrentItem.Data is HoeData)
            {
                if (_tile != null)
                {
                    if (_tile.tileState == TileState.ReadyToHarvest)
                    {
                        return EquipmentAction.Harvest;
                    }
                    return _tile.tileState == TileState.Empty ? EquipmentAction.UnTill : EquipmentAction.None;
                }
                
                if (!toolArea.IsTillable())
                {
                    return EquipmentAction.None;
                }

                return EquipmentAction.Till;

            }
            
            if (CurrentItem.Data is WateringCanData)
            {
                if (_tile == null)
                {
                    return EquipmentAction.None;
                }
            
                if(_tile.tileState == TileState.ReadyToHarvest) return EquipmentAction.Harvest;

                return EquipmentAction.Water;
            }

            if (CurrentItem is ItemSeed)
            {
                if (_tile == null)
                {
                    return EquipmentAction.None;
                }

                switch (_tile.tileState)
                {
                    case TileState.ReadyToHarvest:
                        return EquipmentAction.Harvest;
                    
                    case TileState.Empty:
                        return EquipmentAction.Plant;
                    
                    case TileState.Planted:
                    case TileState.Growing:
                    default:
                        return EquipmentAction.None;
                }
            }
            
            if (CurrentItem is ItemConsumable) return EquipmentAction.Consume;
            
            return EquipmentAction.None;
        }

        private void DoEquipmentAction(EquipmentAction action_)
        {
            switch (action_)
            {
                case EquipmentAction.None:
                    break;
                case EquipmentAction.Till:
                    OnTillAction.Invoke();
                    break;
                case EquipmentAction.Water:
                    OnWaterAction.Invoke();
                    break;
                case EquipmentAction.Plant:
                    Plant();
                    break;
                case EquipmentAction.Harvest:
                    Harvest();
                    break;
                case EquipmentAction.UnTill:
                    OnUnTillAction.Invoke();
                    break;
            }
        }
        
        public void Till()
        {
            FarmTileManager.AddFarmTileAtToolLocation();
        }
        
        public void UnTill()
        {
            FarmTileManager.RemoveTileAtToolLocation();
        }

        public void Water()
        {
            var _farmTile = toolArea.GetFarmTile();
            
            if(_farmTile == null) return;
            
            _farmTile.OnWaterPlant();
            _farmTile.Heal(new HealInfo(10));
        }

        public void Plant()
        {
            if(CurrentItem is not {Data: SeedData}) return;
            var _tile = toolArea.GetFarmTile();
            if (_tile == null) return;
            
            _tile.OnPlantSeed(CurrentItem.Data as SeedData);
            var _itemSeed = (ItemSeed) CurrentItem;
            _itemSeed.RemoveStack();
            InventoryEvents.OnUpdateStackable.Invoke(_itemSeed);
        }

        public void Harvest()
        {
            var _tile = toolArea.GetFarmTile();
            if (_tile == null) return;
            
            if(_tile.tileState != TileState.ReadyToHarvest) return;
            if(_tile.seedData == null) return;
            
            _tile.OnInteract();
        }
    }
}