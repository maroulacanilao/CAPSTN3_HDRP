using CustomEvent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShrineUI
{
    public class ShrineUI_Item : SelectableMenuButton
    {
        [SerializeField] private TextMeshProUGUI txt;
        [SerializeField] private Image icon;

        public static readonly Evt<ShrineUI_Item> OnClickItem = new Evt<ShrineUI_Item>();


        public void Initialize(string text_,Sprite icon_)
        {
            txt.text = text_;
            this.icon.sprite = icon_;
            button.onClick.AddListener(OnClick);
        }
        
        private void OnClick()
        {
            OnClickItem.Invoke(this);
        }
    }
}
