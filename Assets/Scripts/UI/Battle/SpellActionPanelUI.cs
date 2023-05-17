using UnityEngine;

namespace UI.Battle
{
    public class SpellActionPanelUI : MonoBehaviour
    {
        [SerializeField] private SpellBtnItemUI[] spellBtnItems;
        [SerializeField] private SpellDetailsPanel spellDetailsPanel;
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
            
            spellDetailsPanel.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            spellDetailsPanel.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            spellDetailsPanel.gameObject.SetActive(false);
        }
        
        private void OnSelectSpell(SpellBtnItemUI spellBtn_)
        {
            spellDetailsPanel.ShowPanel(spellBtn_);
        }
        
        public void OnCancel()
        {
            if(!gameObject.activeSelf) return;
            mainPanel.BackToActionPanel();
        }
    }
}
