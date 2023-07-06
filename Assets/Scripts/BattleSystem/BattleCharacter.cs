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
using UI.Battle;
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
        
        [field: BoxGroup("Animation Parameters")] [field: AnimatorParam("animator")]
        [field: SerializeField] public int isIdleHash { get; private set; }
        
        [field: BoxGroup("Animation Parameters")] [field: AnimatorParam("animator")]
        [field: SerializeField] public int groundedHash { get; private set; }

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

        private float xOffset;

        public virtual BattleCharacter Initialize(BattleStation battleStation_, int level_, float xOffset_)
        {
            battleStation = battleStation_;
            Level = level_;
            
            transform.SetParent(battleStation.transform);
            
            transform.ResetLocalTransform();
            
            defaultPosition = transform.position;
            spellUser = new SpellUser(character);
            xOffset = xOffset_;
            animator.SetFloat(xSpeedAnimationHash, xOffset);
            animator.SetBool(isIdleHash, true); 
            character.SetLevel(level_);
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
            //return _velocity.magnitude;
            return _velocity.x;
        }
        
        public IEnumerator GoToPosition(Vector3 position_, float duration_ = 0.5f)
        {
            var _x = position_.x > transform.position.x ? 1 : -1;
            
            transform.SetY(defaultPosition.y);
            animator.SetTrigger(moveAnimationHash);
            animator.SetBool(isIdleHash, false);
            
            var _moveTween = transform.DOMove(position_.SetY(defaultPosition.y), duration_);
            
            _moveTween.onUpdate += () =>
            {
                animator.SetFloat(xSpeedAnimationHash, _x);
            };

            yield return _moveTween.WaitForCompletion();
            
            animator.ResetTrigger(moveAnimationHash);
            animator.SetBool(isIdleHash, true);
        }

        public IEnumerator BasicAttack(BattleCharacter target_)
        {
            if(!character.IsAlive) yield break;
            DamageInfo _tempDamageInfo = new DamageInfo(TotalStats.strength, gameObject);
            AttackResult _attackResult = this.AttackTarget(target_, _tempDamageInfo);
            
            animator.SetFloat(xSpeedAnimationHash,xOffset);
            animator.SetTrigger(attackAnimationHash);
            yield return animator.WaitForAnimationEvent(AnimEvent_AttackHit, 0.5f);

            target_.Hit(_attackResult);

            yield return animator.WaitForAnimationEvent(AnimEvent_AnimEnd, 0.5f);
            
            animator.ResetTrigger(attackAnimationHash);
            yield return CoroutineHelper.GetWait(0.2f);
        }

        public IEnumerator AttackTarget(BattleCharacter target_)
        {
            if(!character.IsAlive) yield break;
            yield return GoToPosition(target_.battleStation.attackPosition);
            yield return BasicAttack(target_);
            yield return GoToPosition(battleStation.stationPosition);
            
            animator.SetFloat(xSpeedAnimationHash, xOffset);
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
            DamageTextUI.ShowDamageText.Invoke(transform.position,atkResult_);
            animator.ResetTrigger(hurtAnimationHash);
            animator.ResetTrigger(deathAnimationHash);
            
            if (atkResult_.attackResultType != AttackResultType.Miss)
            {
                character.TakeDamage(atkResult_.damageInfo);
                var _trigger = characterHealth.IsAlive ? hurtAnimationHash : deathAnimationHash;
                animator.SetFloat(xSpeedAnimationHash, xOffset);
                animator.SetTrigger(_trigger);
                if(characterHealth.IsAlive) transform.DoHitEffect();
                var _damage = -atkResult_.damageInfo.DamageAmount;
            }
            else
            {
                StartCoroutine(EvadeAnimation());
            }
        }

        public IEnumerator PlayDeathAnim()
        {
            animator.SetTrigger(deathAnimationHash);
            animator.SetFloat(xSpeedAnimationHash, xOffset);
            yield return animator.WaitForAnimationEvent(AnimEvent_AnimEnd, 2);
            animator.ResetTrigger(deathAnimationHash);
        }

        public IEnumerator PlaySpellCastAnim()
        {
            if(!character.IsAlive) yield break;
            animator.SetFloat(xSpeedAnimationHash, xOffset);
            animator.SetTrigger(spellAnimationHash);
            yield return animator.WaitForAnimationEvent(AnimEvent_SpellCast, 0.5f);
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