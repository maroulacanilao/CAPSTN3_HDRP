using System.Collections.Generic;
using System.Linq;
using CustomHelpers;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using UI.Farming;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.TabMenu.Codex
{
    public class MonsterCodex : CodexMenu
    {
        [SerializeField] private EnemyDataBase enemyDataBase;
        private List<KeyValuePair<EnemyData,int>> enemyKillsStatsList = new List<KeyValuePair<EnemyData, int>>();

        protected override void OnEnable()
        {
            base.OnEnable();
            RemoveItems();
            if(enemyDataBase == null) enemyDataBase = dataBase.enemyDataBase;
            
            errorTXT.gameObject.SetActive(enemyDataBase.enemyKillsStats.Count <= 0);
            if (enemyDataBase.enemyKillsStats.Count == 0)
            {
                codexInfoDisplay.DisplayInfo(new CodexInfo());
                return;
            }
            
            
            enemyKillsStatsList.Clear();
            enemyKillsStatsList = enemyDataBase.enemyKillsStats.ToList();
            
            for (var _i = 0; _i < enemyKillsStatsList.Count; _i++)
            {
                var _statPair = enemyKillsStatsList[_i];
                var _codexItem = Instantiate(codexItemPrefab, contentParent).Initialize(_i,_statPair.Key.characterName);
                codexItems.Add(_codexItem);
            }
            
            EventSystem.current.SetSelectedGameObject(codexItems[0].gameObject);

            ShowCodex(0);
        }
        
        public override void ShowCodex(int index_)
        {
            var _statPair = enemyKillsStatsList[index_];

            var _info = new CodexInfo
            {
                name = _statPair.Key.characterName,
                description = _statPair.Key.encyclopediaInfo.description,
                sprite = _statPair.Key.encyclopediaInfo.sprite,
                quantityTxt = $"Kills: {_statPair.Value}",
                quantity = _statPair.Value,
                errorMsg = $"Kill more {_statPair.Key.characterName} to unlock more from this entry."
            };

            codexInfoDisplay.DisplayInfo(_info);
            
            EventSystem.current.SetSelectedGameObject(codexItems[index_].gameObject);
            codexItems[index_].SelectButton();
        }
    }
}