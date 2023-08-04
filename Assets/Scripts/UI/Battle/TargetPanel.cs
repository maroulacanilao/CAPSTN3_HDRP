using System;
using System.Collections.Generic;
using BattleSystem;
using UnityEngine;

namespace UI.Battle
{
    public class TargetPanel : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private TargetButton targetButtonPrefab;
        
        private BattleActionUI mainPanel;
        private BattleManager battleManager;

        public void Initialize(BattleActionUI mainPanel_, List<BattleCharacter> partyList_)
        {
            mainPanel = mainPanel_;
            battleManager = BattleManager.Instance;

            for (int i = 0; i < partyList_.Count; i++)
            {
                var _character = partyList_[i];
                if(_character == null) continue;

                var _targetButton = Instantiate(targetButtonPrefab, parent);
                _targetButton.Initialize(mainPanel, _character);
                if(_targetButton == null) continue;
                if (i == 0) _targetButton.gameObject.AddComponent<ButtonSelectFirst>();
            }
        }

        private void OnEnable()
        {
            InputUIManager.OnCancel.AddListener(Cancel);
        }
        
        private void OnDisable()
        {
            InputUIManager.OnCancel.RemoveListener(Cancel);
        }

        private void Cancel()
        {
            if(!gameObject.activeSelf) return;
            mainPanel.BackToActionPanel();
        }
    }
}
