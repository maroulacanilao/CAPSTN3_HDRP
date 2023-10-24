using CustomEvent;
using Spells.Base;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.TabMenu.CharacterInfo
{
    public class SpellInfoBtn : SelectableMenuButton
    {
        [SerializeField] TextMeshProUGUI spellNameTxt;
        [SerializeField] Sprite selectedSprite, deselectedSprite;
        
        public static Evt<SpellInfoBtn> OnClick = new Evt<SpellInfoBtn>();

        public SpellData spellData { get; private set; }
        
        public void Initialize(SpellData spellData_)
        {
            spellData = spellData_;
            spellNameTxt.text = spellData.spellName;
            button.onClick.AddListener(() => OnClick.Invoke(this));
        }

        public override void SelectButton()
        {
            base.SelectButton();
            // Text.sprite = selectedSprite;
        }
        
        public override void DeselectButton()
        {
            base.DeselectButton();
            // Text.sprite = deselectedSprite;
        }
    }
}
