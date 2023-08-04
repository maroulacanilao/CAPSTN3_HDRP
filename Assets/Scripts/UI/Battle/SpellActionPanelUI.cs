using System.Collections.Generic;
using UnityEngine;

namespace UI.Battle
{
    public class SpellActionPanelUI : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private SpellBtnItemUI spellBtnItemPrefab;
        [SerializeField] private BattleSpellDetailsPanel battleSpellDetailsPanel;
        
        private List<SpellBtnItemUI> spellBtnItems;
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

        private void CreateList()
        {
            RemoveList();
            for (int i = 0; i < mainPanel.player.spellUser.spellList.Count; i++)
            {
                var spellBtnItem = Instantiate(spellBtnItemPrefab, parent);
                spellBtnItem.Initialize(mainPanel,i);
                spellBtnItems.Add(spellBtnItem);
            }
            
            if(spellBtnItems.Count <= 0) return;

            spellBtnItems[0].gameObject.AddComponent<ButtonSelectFirst>();
            battleSpellDetailsPanel.ShowPanel(spellBtnItems[0]);
        }

        private void RemoveList()
        {
            spellBtnItems ??= new List<SpellBtnItemUI>();
            
            foreach (var btn in spellBtnItems)
            {
                Destroy(btn.gameObject);
            }
            spellBtnItems.Clear();
        }

        public void ShowPanel(BattleActionUI mainPanel_)
        {
            mainPanel = mainPanel_;
            
            CreateList();
            
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
            return index_ >= 0 && index_ < spellBtnItems.Count;
        }
    }
}
