using UnityEngine;

namespace UI.Battle
{
    public class SpellActionPanelUI : MonoBehaviour
    {
        [SerializeField] private SpellBtnItemUI[] spellBtnItems;
        [SerializeField] private BattleSpellDetailsPanel battleSpellDetailsPanel;
        private BattleActionUI mainPanel;
        
        private void Awake()
        {
            SpellBtnItemUI.OnSelectSpell.AddListener(OnSelectSpell);
            InputUIManager.OnCancel.AddListener(OnCancel);
        }

        private void OnDestroy()
        {
            SpellBtnItemUI.OnSelectSpell.RemoveListener(OnSelectSpell);
            InputUIManager.OnCancel.RemoveListener(OnCancel);
        }

        public void ShowPanel(BattleActionUI mainPanel_)
        {
            mainPanel = mainPanel_;
            var _spellUser = mainPanel.player.spellUser;
            for (var i = 0; i < _spellUser.spellList.Count; i++)
            {
                spellBtnItems[i].Initialize(mainPanel_,i);
            }
            
            battleSpellDetailsPanel.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            battleSpellDetailsPanel.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            battleSpellDetailsPanel.gameObject.SetActive(false);
        }
        
        private void OnSelectSpell(SpellBtnItemUI spellBtn_)
        {
            battleSpellDetailsPanel.ShowPanel(spellBtn_);
        }
        
        public void OnCancel()
        {
            if(!gameObject.activeSelf) return;
            mainPanel.BackToActionPanel();
        }
        
        public bool IsSpellIndexValid(int index_)
        {
            return index_ >= 0 && index_ < spellBtnItems.Length;
        }
    }
}
