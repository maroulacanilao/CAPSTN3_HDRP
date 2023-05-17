using NaughtyAttributes;
using UnityEngine;

namespace Items.ItemData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/ArmorData", fileName = "ArmorData")]
    public class ArmorData : GearData
    {
        private void Reset()
        {
            itemType = ItemType.Armor;
        }

        private void OnValidate()
        {
            itemType = ItemType.Armor;
        }
    }
}