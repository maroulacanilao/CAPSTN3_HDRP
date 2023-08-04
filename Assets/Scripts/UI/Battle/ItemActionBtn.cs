using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Battle
{
    public class ItemActionBtn : SelectableMenuButton, IPointerEnterHandler
    {
        [SerializeField] private TextMeshProUGUI itemNameTxt;
        public Item item { get; private set; }

        public void Initialize(BattleActionUI battleActionUI_, Item item_)
        {
            item = item_;
            itemNameTxt.text = item.Data.ItemName;

            button.onClick.AddListener(() =>
            {
                UseItemActionUI.CurrentItemBtn = this;
                BattleActionUI.Instance.currentAction = BattleAction.Item;
                var _player = BattleActionUI.Instance.player;
                BattleActionUI.Instance.StartAction(_player);
            });
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            button.Select();
        }

        public override void SelectButton()
        {
            base.SelectButton();
            UseItemActionUI.OnItemBtnSelect.Invoke(this);
        }
    }
}
