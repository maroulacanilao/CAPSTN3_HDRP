using NaughtyAttributes;
using UnityEngine;

namespace Items.ItemData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/WeaponData", fileName = "WeaponData")]
    public class WeaponData : GearData
    {
        private void Reset()
        {
            itemType = ItemType.Weapon;
        }

        private void OnValidate()
        {
            itemType = ItemType.Weapon;
        }
    }
}