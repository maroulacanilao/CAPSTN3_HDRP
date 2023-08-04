using UnityEngine;

namespace Items.ItemData
{
    [UnityEngine.CreateAssetMenu(menuName = "ScriptableObjects/ItemData/QuestItemData", fileName = "QuestItemData")]
    public class QuestItemData : ItemData
    {
        private void Reset()
        {
            ItemType = ItemType.QuestItem;
        }

        protected override void OnValidate()
        {
            ItemType = ItemType.QuestItem;
        }
    }
}