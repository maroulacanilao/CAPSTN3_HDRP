using System.Collections;
using BaseCore;
using Character;
using CustomEvent;
using CustomHelpers;
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
        
        public FarmTile currTile { get; private set; }

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

        #region Tool Selection
        
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

        #endregion

        #region Action

        public EquipmentAction GetEquipmentAction()
        {
            var _nearestInteractable = interactDetector.nearestInteractable;

            if (_nearestInteractable != null && _nearestInteractable is FarmTileInteractable _farmTileInteractable)
            {
                if(_farmTileInteractable.farmTile.tileState == TileState.ReadyToHarvest) return EquipmentAction.Harvest;
            }
            
            if(CurrentItem == null) return EquipmentAction.None;

            var _tile = toolArea.GetFarmTile();
            SetTile(_tile);
            
            
            if (CurrentItem.Data is HoeData)
            {
                if (currTile != null)
                {
                    if (currTile.tileState == TileState.ReadyToHarvest)
                    {
                        return EquipmentAction.Harvest;
                    }
                    return currTile.tileState == TileState.Empty ? EquipmentAction.UnTill : EquipmentAction.None;
                }
                
                if (!toolArea.IsTillable())
                {
                    return EquipmentAction.None;
                }

                return EquipmentAction.Till;

            }
            
            if (CurrentItem.Data is WateringCanData)
            {
                if (currTile == null)
                {
                    return EquipmentAction.None;
                }
            
                if(currTile.tileState == TileState.ReadyToHarvest) return EquipmentAction.Harvest;

                return EquipmentAction.Water;
            }

            if (CurrentItem is ItemSeed)
            {
                if (currTile == null)
                {
                    return EquipmentAction.None;
                }

                switch (currTile.tileState)
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
        

        #endregion

        #region Farm Action

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
            if(currTile == null) return;
            
            currTile.OnWaterPlant();
            currTile.Heal(new HealInfo(10));
        }

        public void Plant()
        {
            if(CurrentItem is not {Data: SeedData}) return;
            if (currTile == null) return;
            
            currTile.OnPlantSeed(CurrentItem.Data as SeedData);
            var _itemSeed = (ItemSeed) CurrentItem;
            _itemSeed.RemoveStack();
            InventoryEvents.OnUpdateStackable.Invoke(_itemSeed);
        }

        public void Harvest()
        {
            if (currTile == null) return;
            
            if(currTile.tileState != TileState.ReadyToHarvest) return;
            if(currTile.seedData == null) return;
            
            currTile.OnInteract();
        }

        #endregion
        
        public void SetTile(FarmTile tile_)
        {
            if (currTile.IsValid())
            {
                currTile.Exit();
            }
            
            currTile = tile_;
            
            if (currTile.IsValid())
            {
                currTile.Enter();
            }
        }

        private IEnumerator Co_TileSetter()
        {
            var _waiter = new WaitForSeconds(0.2f);
            while (enabled)
            {
                var _tile = toolArea.GetFarmTile();
                SetTile(_tile);
                yield return _waiter;
            }
        }
    }
}