using System;
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
    public class PlayerEquipment : MonoBehaviour
    {
        [field: SerializeField] public PlayerCharacter player { get; private set; }
        [field: SerializeField] public FarmTools farmTools { get; private set; }

        private PlayerInventory playerInventory;

        private Action[] _equipmentActions;
        
        public int currIndex { get; private set; }
        public Item CurrentItem => playerInventory.ItemTools[currIndex];
        
        public static readonly Evt<Item> OnChangeItemOnHand = new Evt<Item>();

        private void Start()
        {
            playerInventory = player.playerInventory;
            currIndex = 0;
            _equipmentActions = new Action[4];
            InventoryEvents.OnItemOnHandUpdate.AddListener(OnUpdateTools);

            for (int i = 0; i < playerInventory.ItemTools.Length; i++)
            {
                OnUpdateTools(i, playerInventory.ItemTools[i]);
            }
            OnChangeItemOnHand.Invoke(CurrentItem);
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
            _equipmentActions[currIndex]?.Invoke();
        }

        public void CycleTool(bool isNext_)
        {
            if (isNext_) currIndex++;
            else currIndex--;
            ClampIndex();
            OnChangeItemOnHand.Invoke(CurrentItem);
        }

        public void SelectTool(int index_)
        {
            currIndex = index_;
            ClampIndex();
            OnChangeItemOnHand.Invoke(CurrentItem);
        }

        private void ClampIndex()
        {
            currIndex %= _equipmentActions.Length;
            if (currIndex < 0) currIndex = _equipmentActions.Length - 1;
        }

        private void OnUpdateTools(int index_, Item item_)
        {
            _equipmentActions[index_] = null;
            if(item_ == null) return;
            
            switch (item_.ItemType)
            {
                case ItemType.Consumable:
                {
                    var _consumable = (ItemConsumable) item_;
                    _equipmentActions[index_] = () =>
                    {
                        _consumable.Consume(player.statusEffectReceiver);
                        InventoryEvents.OnUpdateStackable.Invoke(_consumable);
                    };
                    break;
                }
                
                case ItemType.Seed:
                {
                    var _seed = (ItemSeed) item_;
                    _equipmentActions[index_] = () =>
                    {
                        farmTools.PlantSeed(_seed.Data as SeedData);
                        _seed.RemoveStack();
                    };
                    break;
                }
                case ItemType.Tool:
                {
                    var _toolData = (ToolData) item_.Data;
                    _equipmentActions[index_] = () =>
                    {
                        _toolData.UseTool(this);
                    };
                    break;
                }
                default:
                    _equipmentActions[index_] = null;
                    break;
            }

        }
    }
}