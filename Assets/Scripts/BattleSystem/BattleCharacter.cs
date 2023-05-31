using System;
using System.Collections;
using BaseCore;
using Character;
using Character.CharacterComponents;
using CustomEvent;
using CustomHelpers;
using DG.Tweening;
using NaughtyAttributes;
using ObjectPool;
using ScriptableObjectData.CharacterData;
using UnityEngine;

namespace BattleSystem
{
    public abstract class BattleCharacter : MonoBehaviour
    {
        #region Positions

        [field: SerializeField] public BattleStation battleStation { get; protected set; }
        

        #endregion
        
        #region Components

        [field: SerializeField] [field: Required()] [field: BoxGroup("Components")] 
        public CharacterBase character { get; private set; }

        [field: SerializeField] [field: Required()] [field: BoxGroup("Components")]
        public CharacterController controller { get; private set; }

        [field: SerializeField] [field: Required()] [field: BoxGroup("Components")] 
        public AnimationEventReceiver animEventReceiver { get; private set; }
        
        [field: SerializeField] [field: Required()] [field: BoxGroup("Components")] 
        public SpriteOutline spriteOutline { get; private set; }
        
        [field: SerializeField] [field: Required()] [field: BoxGroup("Components")] 
        public SpriteRenderer spriteRenderer { get; private set; }

        [field: SerializeField] [field: Required()] [field: BoxGroup("Components")] 
        public Animator animator { get; private set; }
        
        public SpellUser spellUser { get; protected set; }

        #endregion
        
        #region Animation Parameters
        [field: BoxGroup("Animation Parameters")] [field: AnimatorParam("animator")]
        [field: SerializeField] public int xSpeedAnimationHash { get; private set; }
        
        [field: BoxGroup("Animation Parameters")] [field: AnimatorParam("animator")]
        [field: SerializeField] public int moveAnimationHash { get; private set; }
        
        [field: BoxGroup("Animation Parameters")] [field: AnimatorParam("animator")]
        [field: SerializeField] public int attackAnimationHash { get; private set; }
        
        [field: BoxGroup("Animation Parameters")] [field: AnimatorParam("animator")]
        [field: SerializeField] public int spellAnimationHash { get; private set; }
        
        [field: BoxGroup("Animation Parameters")] [field: AnimatorParam("animator")]
        [field: SerializeField] public int hurtAnimationHash { get; private set; }
        
        [field: BoxGroup("Animation Parameters")] [field: AnimatorParam("animator")]
        [field: SerializeField] public int deathAnimationHash { get; private set; }

        #endregion

        #region AnimationEvents

        [field: BoxGroup("Animation Event")] [field: SerializeField]
        public string AnimEvent_AttackHit { get; private set; }
        
        [field: BoxGroup("Animation Event")] [field: SerializeField]
        public string AnimEvent_AnimEnd { get; private set; }
        
        [field: BoxGroup("Animation Event")] [field: SerializeField]
        public string AnimEvent_SpellCast { get; private set; }

        #endregion

        public CharacterData characterData => character.characterData;
        public int Level { get; protected set; }

        protected Vector3 defaultPosition;
        
        public static readonly Evt<BattleCharacter> OnSelectMenu = new Evt<BattleCharacter>();
        
        public CharacterHealth characterHealth => character.health;
        public CharacterMana characterMana => character.mana;
        public virtual CombatStats TotalStats => character.stats;
        public virtual CombatStats BaseStats => character.statsData.baseCombatStats;

        public virtual BattleCharacter Initialize(BattleStation battleStation_, int level_)
        {
            battleStation = battleStation_;
            Level = level_;
            
            transform.SetParent(battleStation.transform);
            
            transform.ResetLocalTransform();
            
            defaultPosition = transform.position;
            spellUser = new SpellUser(character);
            return this;
        }

        private void OnEnable()
        {
            OnSelectMenu.AddListener(Select);
        }

        private void OnDisable()
        {
            OnSelectMenu.RemoveListener(Select);
        }

        private void Select(BattleCharacter battleCharacter_)
        {
            spriteOutline.enabled = battleCharacter_ == this;
        }

        #region Movement
        
        public float GetHorizontalVelocity()
        {
            Vector2 _velocity = new Vector2(controller.velocity.x, controller.velocity.y);
            return _velocity.magnitude;
        }

        public void UpdateMoveAnim()
        {
            if (GetHorizontalVelocity().IsApproximatelyTo(0))
            {
                animator.SetFloat(xSpeedAnimationHash, 0);
            }
            
            animator.SetFloat(xSpeedAnimationHash, 1);
        }

        public IEnumerator GoToPosition(Vector3 position_, float duration_ = 0.5f)
        {
            transform.SetY(defaultPosition.y);
            animator.SetTrigger(moveAnimationHash);
            
            var _moveTween = transform.DOMove(position_.SetY(defaultPosition.y), duration_);
            
            _moveTween.onUpdate += UpdateMoveAnim;
            
            yield return _moveTween.WaitForCompletion();
            
            animator.SetFloat(xSpeedAnimationHash, 0);
            animator.ResetTrigger(moveAnimationHash);
        }

        public IEnumerator BasicAttack(BattleCharacter target_)
        {
            if(!character.IsAlive) yield break;
            DamageInfo _tempDamageInfo = new DamageInfo(TotalStats.physicalDamage, gameObject, DamageType.Weapon);
            AttackResult _attackResult = this.DamageTarget(target_, _tempDamageInfo);
            
            animator.SetTrigger(attackAnimationHash);
            yield return animator.WaitForAnimationEvent(AnimEvent_AttackHit, 2);
            
            Debug.Log(_attackResult.attackResultType);
            
            target_.Hit(_attackResult);

            yield return animator.WaitForAnimationEvent(AnimEvent_AnimEnd, 1);
            
            animator.ResetTrigger(attackAnimationHash);
            yield return CoroutineHelper.GetWait(0.2f);
        }

        public IEnumerator AttackTarget(BattleCharacter target_)
        {
            if(!character.IsAlive) yield break;
            yield return GoToPosition(target_.battleStation.attackPosition);
            yield return BasicAttack(target_);
            yield return GoToPosition(battleStation.stationPosition);
        }

        public IEnumerator EvadeAnimation()
        {
            if(!character.IsAlive) yield break;
            var _yPos = transform.position.y;
            yield return transform.DOMove(battleStation.evadePosition.SetY(_yPos), 0.1f);
            yield return CoroutineHelper.GetWait(0.1f);
            yield return transform.DOMove(battleStation.stationPosition.SetY(_yPos), 0.1f);
        }

        public void Hit(AttackResult atkResult_)
        {
            if(!character.IsAlive) return;
            
            if (atkResult_.attackResultType != AttackResultType.Miss)
            {
                character.TakeDamage(atkResult_.damageInfo);
                var _trigger = characterHealth.IsAlive ? hurtAnimationHash : deathAnimationHash;
                animator.SetTrigger(_trigger);
                if(characterHealth.IsAlive) transform.DoHitEffect();
                var _damage = -atkResult_.damageInfo.DamageAmount;
                DamageTextUI.ShowDamageText.Invoke(transform.position,_damage.ToString());
            }
            else
            {
                DamageTextUI.ShowDamageText.Invoke(transform.position,"Miss");
                StartCoroutine(EvadeAnimation());
            }
        }

        public IEnumerator PlayDeathAnim()
        {
            animator.SetTrigger(deathAnimationHash);
            yield return animator.WaitForAnimationEvent(AnimEvent_AnimEnd, 2);
            animator.ResetTrigger(deathAnimationHash);
        }

        public IEnumerator PlaySpellCastAnim()
        {
            if(!character.IsAlive) yield break;
            animator.SetTrigger(spellAnimationHash);
            yield return animator.WaitForAnimationEvent(AnimEvent_SpellCast, 2);
            animator.ResetTrigger(spellAnimationHash);
        }
        
        #endregion

        #region FOR DEBUGGING ONLY, DELETE THIS FUTURE ME
        
        [Header("FOR DEBUGGING ONLY")]
        public BattleCharacter opponent;
        public int LevelDebug;

        private void Start()
        {
            Level = LevelDebug;
        }


        [Button("Attack Opponent")]
        public void AttackOpponent()
        {
            StartCoroutine(AttackTarget(opponent));
        }
        
        #endregion
    }
}