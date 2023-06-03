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
        [field: SerializeField] public InteractDetector interactDetector { get; private set; }

        private PlayerInventory playerInventory;

        private Action[] _equipmentActions;
        
        public int currIndex { get; private set; }
        public Item CurrentItem => playerInventory.ItemTools[currIndex];
        
        public static readonly Evt<int> OnChangeItemOnHand = new Evt<int>();

        private void Start()
        {
            playerInventory = player.playerInventory;
            currIndex = 0;
            _equipmentActions = new Action[4];
            
            if(interactDetector == null) interactDetector = farmTools.GetComponent<InteractDetector>();
            
            InventoryEvents.OnItemOnHandUpdate.AddListener(OnUpdateTools);

            for (int i = 0; i < playerInventory.ItemTools.Length; i++)
            {
                OnUpdateTools(i, playerInventory.ItemTools[i]);
            }
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
            _equipmentActions[currIndex]?.Invoke();
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
            currIndex %= _equipmentActions.Length;
            if (currIndex < 0) currIndex = _equipmentActions.Length - 1;
        }

        private void OnUpdateTools(int index_, Item item_)
        {
            _equipmentActions[index_] = null;
            
            if (item_ == null)
            {
                _equipmentActions[index_] = Interact;
                return;
            }
            
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
                        if(farmTools.PlantSeed(_seed)) return;
                        Interact();
                    };
                    break;
                }
                case ItemType.Tool:
                {
                    var _toolData = (ToolData) item_.Data;
                    _equipmentActions[index_] = () =>
                    {
                        if(_toolData.UseTool(this)) return;
                        Interact();
                    };
                    break;
                }
                default:
                    _equipmentActions[index_] = Interact;
                    break;
            }
        }

        private void Interact()
        {
            Debug.Log("Interact");
            interactDetector.Interact(); 
            
        }
        
    }
}