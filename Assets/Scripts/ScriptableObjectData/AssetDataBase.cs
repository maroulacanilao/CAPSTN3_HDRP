using NaughtyAttributes;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/AssetDataBase", fileName = "AssetDataBase")]
    public class AssetDataBase : ScriptableObject
    {
        [field: BoxGroup("Stats Icon")]
        [field: SerializeField] public Sprite healthIcon { get; private set; }
        [field: BoxGroup("Stats Icon")]
        [field: SerializeField] public Sprite manaIcon { get; private set; }
        [field: BoxGroup("Stats Icon")]
        [field: SerializeField] public Sprite phyDmgIcon { get; private set; }
        [field: BoxGroup("Stats Icon")]
        [field: SerializeField] public Sprite armIcon { get; private set; }
        [field: BoxGroup("Stats Icon")]
        [field: SerializeField] public Sprite magDmgIcon { get; private set; }
        [field: BoxGroup("Stats Icon")]
        [field: SerializeField] public Sprite magResIcon { get; private set; }
        [field: BoxGroup("Stats Icon")]
        [field: SerializeField] public Sprite spdIcon { get; private set; }
        [field: BoxGroup("Stats Icon")]
        [field: SerializeField] public Sprite accIcon { get; private set; }
        
        [field: BoxGroup("Item Icon")]
        [field: SerializeField] public Sprite goldIcon { get; private set; }
        [field: BoxGroup("Item Icon")]
        [field: SerializeField] public Sprite weaponIcon { get; private set; }
        [field: BoxGroup("Item Icon")]
        [field: SerializeField] public Sprite armorIcon { get; private set; }
        [field: BoxGroup("Item Icon")]
        [field: SerializeField] public Sprite consumableIcon { get; private set; }
        [field: BoxGroup("Item Icon")]
        [field: SerializeField] public Sprite seedIcon { get; private set; }
        [field: BoxGroup("Item Icon")]
        [field: SerializeField] public Sprite toolIcon { get; private set; }
    }
}
