using Character;
using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

namespace Items.ItemData
{
    public abstract class GearData : ItemData
    {
        [field: MinMaxSlider(0, 100)] [field: BoxGroup("Possible Stats")]
        [field: SerializeField] public Vector2Int PossibleAddToHp { get; private set; }
        
        [field: MinMaxSlider(0, 100)] [field: BoxGroup("Possible Stats")]
        [field: SerializeField] public Vector2Int PossibleAddToMp { get; private set; }
        
        [field: MinMaxSlider(0, 100)] [field: BoxGroup("Possible Stats")]
        [field: SerializeField] public Vector2Int PossibleWpnDmg { get; private set; }
        
        [field: MinMaxSlider(0, 100)] [field: BoxGroup("Possible Stats")]
        [field: SerializeField] public Vector2Int PossibleArmVal { get; private set; }
        
        [field: MinMaxSlider(0, 100)] [field: BoxGroup("Possible Stats")]
        [field: SerializeField] public Vector2Int PossibleMagDmg { get; private set; }
        
        [field: MinMaxSlider(0, 100)] [field: BoxGroup("Possible Stats")]
        [field: SerializeField] public Vector2Int PossibleMagRes { get; private set; }
        
        [field: MinMaxSlider(0, 100)] [field: BoxGroup("Possible Stats")]
        [field: SerializeField] public Vector2Int PossibleAcc { get; private set; }
        
        [field: MinMaxSlider(0, 100)] [field: BoxGroup("Possible Stats")]
        [field: SerializeField] public Vector2Int PossibleSpeed { get; private set; }


        public virtual CombatStats GetRandomStats()
        {
            return new CombatStats()
            {
                maxHp = PossibleAddToHp.GetRandomInRange(),
                maxMana = PossibleAddToMp.GetRandomInRange(),
                physicalDamage = PossibleWpnDmg.GetRandomInRange(),
                armor = PossibleArmVal.GetRandomInRange(),
                magicDamage = PossibleMagDmg.GetRandomInRange(),
                magicResistance = PossibleMagRes.GetRandomInRange(),
                accuracy = PossibleAcc.GetRandomInRange(),
                speed = PossibleSpeed.GetRandomInRange()
            };
        }
    }
}