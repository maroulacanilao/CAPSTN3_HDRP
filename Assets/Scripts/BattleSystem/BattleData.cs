using System;
using Character;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Persistent/BattleData", fileName = "BattleData")]
    public class BattleData : ScriptableObject
    {
        [field: SerializeField] public PlayerData playerData { get; private set; }
        [field: SerializeField] public int minutesJumped { get; private set; } = 5;
        [field: SerializeField] public EnemyData currentEnemyData { get; private set; }
        [field: SerializeField] public int currentEnemyLevel { get; private set; }
        
        public bool isPlayerFirst { get; private set; }

        public Vector3 currentEnemyPosition { get; private set; }
        
        [field: SerializeField] [field: BoxGroup("Battle Modifier - Evasion")] [field: Range(0,1)]
        public float evasionChcMod { get; private set; } = 0.9f;
        
        [field: SerializeField] [field: BoxGroup("Battle Modifier - Evasion")] [field: Min(0)]
        public float evaStrModifier { get; private set; } = 1.5f;
        
        [field: SerializeField] [field: BoxGroup("Battle Modifier - Evasion")] [field: MinMaxSlider(0,1)]
        public Vector2 evaMinMax { get; private set; } = new Vector2(0.02f, 0.9f);
        
        
        [field: SerializeField] [field: BoxGroup("Battle Modifier - Critical Chance")] [field: Min(0)]
        public float crtChcAtkStrModifier { get; private set; } = 0.1f;
        
        [field: SerializeField] [field: BoxGroup("Battle Modifier - Critical Chance")] [field: Min(0)]
        public float crtChcAtkSpdModifier { get; private set; } = 0.05f;

        [field: SerializeField] [field: BoxGroup("Battle Modifier - Critical Chance")] [field: Min(0)]
        public float crtChcTarSpdModifier { get; private set; } = 0.03f;
        
        [field: SerializeField] [field: BoxGroup("Battle Modifier - Critical Chance")] [field: Min(0f)]
        public float crtChcMod{ get; private set; } = 0.1f;
        
        [field: SerializeField] [field: BoxGroup("Battle Modifier - Critical Chance")] [field: MinMaxSlider(0,1)]
        public Vector2 crtMinMax { get; private set; } = new Vector2(0.02f, 0.9f);
        
        [field: SerializeField] [field: BoxGroup("Battle Modifier - Damage Modifer")] [field: Range(1,3)]
        public float crtDmgMod { get; private set; } = 1.5f;
        
        [field: SerializeField] [field: BoxGroup("Battle Modifier - Damage Modifer")] [field: Range(1,3)]
        public float weakDmgMod { get; private set; } = 2f;
        
        [field: SerializeField] [field: BoxGroup("On Weak Status Effect")] 
        public SkipTurn_SE skipTurnSE { get; private set; }
        
        

        private void OnEnable()
        {
            BattleHelper.battleData = this;
        }

        public void ResetData()
        {
            currentEnemyData = null;
        }

        public void EnterBattle(EnemyCharacter enemy_, bool isPlayerFirst_)
        {
            if (enemy_ == null)
            {
                Debug.LogWarning("Enemy is null");
            }
            currentEnemyData = enemy_.characterData as EnemyData;
            currentEnemyPosition = enemy_.transform.position;
            currentEnemyLevel = enemy_.level;
            isPlayerFirst = isPlayerFirst_;
        }
    }
}