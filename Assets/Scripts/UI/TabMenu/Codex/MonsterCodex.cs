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

        public void OnEnable()
        {
            RemoveItems();
            if(enemyDataBase == null) enemyDataBase = dataBase.enemyDataBase;
            
            if (enemyDataBase.enemyKillsStats.Count == 0)
            {
                codexInfoDisplay.gameObject.SetActive(false);
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

            codexItems[0].GetOrAddComponent<ButtonSelectFirst>();
            
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
                quantity = $"Kills: {_statPair.Value}"
            };

            codexInfoDisplay.DisplayInfo(_info);
            
            EventSystem.current.SetSelectedGameObject(codexItems[index_].gameObject);
            codexItems[index_].SelectButton();
        }
    }
}