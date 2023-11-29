using System.Collections.Generic;
using System.Linq;
using Items.ItemData;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.TabMenu.Codex
{
    public class FishCodex : CodexMenu
    {
        [SerializeField] private FishDataBase fishDataBase;

        private List<KeyValuePair<ConsumableData, int>> fishStatsList = new List<KeyValuePair<ConsumableData, int>>();

        [SerializeField] protected CodexInfoDisplayRevised codexInfoDisplayRevised;

        protected override void OnEnable()
        {
            base.OnEnable();
            RemoveItems();
            if (fishDataBase == null) fishDataBase = dataBase.fishDataBase;

            errorTXT.gameObject.SetActive(fishDataBase.fishCatchStats.Count <= 0);

            if (fishDataBase.fishCatchStats.Count == 0)
            {
                codexInfoDisplayRevised.DisplayInfo(new CodexInfoRevised());
                return;
            }

            fishStatsList.Clear();
            fishStatsList = fishDataBase.fishCatchStats.ToList();

            for (var _i = 0; _i < fishStatsList.Count; _i++)
            {
                var _statPair = fishStatsList[_i];
                var _codexItem = Instantiate(codexItemPrefab, contentParent).Initialize(_i, _statPair.Key.ItemName);
                codexItems.Add(_codexItem);
            }

            EventSystem.current.SetSelectedGameObject(codexItems[0].gameObject);

            ShowCodex(0);
        }

        public override void ShowCodex(int index_)
        {
            var _statPair = fishStatsList[index_];

            var _info = new CodexInfoRevised
            {
                // name = _statPair.Key.ItemName,
                description = _statPair.Key.encyclopediaInfo.description,
                sprite = _statPair.Key.encyclopediaInfo.sprite,
                quantityTxt = $"{_statPair.Value} Caught",
                quantity = _statPair.Value,
                quantityNeededCount = _statPair.Key.quantityNeededCount
            };

            codexInfoDisplayRevised.DisplayInfo(_info);

            EventSystem.current.SetSelectedGameObject(codexItems[index_].gameObject);
            codexItems[index_].SelectButton();
        }

    }
}