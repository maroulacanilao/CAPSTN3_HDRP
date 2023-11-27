using System.Collections.Generic;
using System.Linq;
using Items.ItemData;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.TabMenu.Codex
{
    public class CropCodexRevised : CodexMenu
    {
        [SerializeField] private CropDataBase cropDataBase;

        private List<KeyValuePair<ConsumableData, int>> cropStatsList = new List<KeyValuePair<ConsumableData, int>>();

        [SerializeField] protected CodexInfoDisplayRevised codexInfoDisplayRevised;

        protected override void OnEnable()
        {
            base.OnEnable();
            RemoveItems();
            if (cropDataBase == null) cropDataBase = dataBase.cropDataBase;

            errorTXT.gameObject.SetActive(cropDataBase.cropHarvestStats.Count <= 0);

            if (cropDataBase.cropHarvestStats.Count == 0)
            {
                codexInfoDisplayRevised.DisplayInfo(new CodexInfoRevised());
                return;
            }

            cropStatsList.Clear();
            cropStatsList = cropDataBase.cropHarvestStats.ToList();

            for (var _i = 0; _i < cropStatsList.Count; _i++)
            {
                var _statPair = cropStatsList[_i];
                var _codexItem = Instantiate(codexItemPrefab, contentParent).Initialize(_i, _statPair.Key.ItemName);
                codexItems.Add(_codexItem);
            }

            EventSystem.current.SetSelectedGameObject(codexItems[0].gameObject);

            ShowCodex(0);
        }

        public override void ShowCodex(int index_)
        {
            var _statPair = cropStatsList[index_];

            var _info = new CodexInfoRevised
            {
                // name = _statPair.Key.ItemName,
                description = _statPair.Key.encyclopediaInfo.description,
                sprite = _statPair.Key.encyclopediaInfo.sprite,
                quantityTxt = $"{_statPair.Value} Harvest(s)",
                quantity = _statPair.Value,
                quantityNeededCount = _statPair.Key.quantityNeededCount
            };

            codexInfoDisplayRevised.DisplayInfo(_info);

            EventSystem.current.SetSelectedGameObject(codexItems[index_].gameObject);
            codexItems[index_].SelectButton();
        }
    }
}
