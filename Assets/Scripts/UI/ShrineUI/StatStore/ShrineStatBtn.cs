using CustomHelpers;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ShrineUI.StatStore
{
    public class ShrineStatBtn : SelectableMenuButton, IPointerEnterHandler
    {
        [SerializeField] private TextMeshProUGUI itemName_TXT, count_TXT;
        [SerializeField] private Image itemIcon_IMG;
        
        public ItemConsumable item { get; private set; }

        private static ShrineStatBtn currentSelected;
        public static ShrineStatBtn CurrentSelected => currentSelected.IsEmptyOrDestroyed() ? null : currentSelected;

        public void SetItem(ItemConsumable itemConsumable_)
        {
            item = itemConsumable_;
            itemName_TXT.text = itemConsumable_.Data.ItemName;
            itemIcon_IMG.sprite = itemConsumable_.Data.Icon;
            count_TXT.text = $"x{itemConsumable_.StackCount}";
        }

        public override void SelectButton()
        {
            base.SelectButton();
            currentSelected = this;
        }
        
        public override void DeselectButton()
        {
            base.DeselectButton();
            if(CurrentSelected == this) currentSelected = null;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        { 
            OnSelect(eventData);
        }
    }
}
