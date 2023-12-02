using Items.Inventory;
using Items.ItemData;
using ScriptableObjectData;
using Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShrineUI
{
    public class ShrineRequirementItem : MonoBehaviour
    {
        [SerializeField] protected GameDataBase gameDataBase;

        protected PlayerInventory Inventory => gameDataBase.playerInventory;

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameTxt, countTxt;
    
        public void Set(OfferRequirement offerRequirement_)
        {
            var _data = offerRequirement_.consumableData;
            var _count = offerRequirement_.count;
            icon.sprite = _data.Icon;
            if (nameTxt != null) nameTxt.text = _data.ItemName;
            // countTxt.text = $"x{_count}";

            if (Inventory.StackableDictionary.TryGetValue(offerRequirement_.consumableData, out var _stackable))
            {
                if (_stackable.StackCount < _count)
                {
                    countTxt.text = $"<color=#af023d>{_stackable.StackCount}</color>/{_count}";
                }
                else
                {
                    countTxt.text = $"{_stackable.StackCount}/{_count}";
                }
            }
            else
            {
                countTxt.text = $"<color=#af023d>0</color>/{_count}";
            }
        }

        public void SetNull()
        {
            icon.sprite = null;
            if (nameTxt != null) nameTxt.text = "";
            countTxt.text = "";
        }
    }
}
