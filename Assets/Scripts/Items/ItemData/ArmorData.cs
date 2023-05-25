using NaughtyAttributes;
using UnityEngine;

namespace Items.ItemData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/ArmorData", fileName = "ArmorData")]
    public class ArmorData : GearData
    {
        private void Reset()
        {
            ItemType = ItemType.Armor;
        }

        protected override void OnValidate()
        {
            ItemType = ItemType.Armor;
        }
    }
}