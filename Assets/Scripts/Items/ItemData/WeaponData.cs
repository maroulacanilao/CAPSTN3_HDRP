using NaughtyAttributes;
using UnityEngine;

namespace Items.ItemData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/WeaponData", fileName = "WeaponData")]
    public class WeaponData : GearData
    {
        private void Reset()
        {
            ItemType = ItemType.Weapon;
        }

        protected override void OnValidate()
        {
            ItemType = ItemType.Weapon;
        }
    }
}