using System;
using System.Collections;
using BattleSystem;
using Character;
using Character.CharacterComponents;
using CustomHelpers;
using Managers;
using UI.Battle;
using UnityEngine;
using UnityEngine.VFX;

namespace Spells.Base
{
	public abstract class SpellObject : MonoBehaviour
	{
		[field: SerializeField] public SpellData spellData { get; protected set;}
		
		protected CharacterBase character;
		protected BattleCharacter battleCharacter;
		protected BattleCharacter target;
		
		public int damage
		{
			get
			{
				switch (spellData.spellType)
				{
					case SpellType.Damage when spellData.isPhysical:
					{
						var _magDmg = character.stats.strength;
						return Mathf.RoundToInt(_magDmg * spellData.damageModifier);
					}
					case SpellType.Damage when  !spellData.isPhysical:
					{
						var _magDmg = character.stats.intelligence;
						return Mathf.RoundToInt(_magDmg * spellData.damageModifier);
					}
						
					default:
						return 0;
				}
			}
		}
		
		public SpellUser user { get; protected set; }

		public SpellObject Initialize(SpellUser user_, SpellData spellData_)
		{
			user = user_;
			spellData = spellData_;
			character = user.character;
			battleCharacter = user.battleCharacter;

			if (battleCharacter == null) throw new Exception("Battle Character is null");
			return this;
		}

		protected abstract IEnumerator OnActivate();

		// Deactivating without cooldown
		protected abstract IEnumerator OnDeactivate();

		protected abstract void OnRemoveSkill();
		
		protected virtual void OnTick() { }
		
		// for using the ability
		public IEnumerator Activate(BattleCharacter target_)
		{
			target = target_;
			yield return OnActivate();
		}

		// for starting cooldown
		public IEnumerator Deactivate()
		{
			target = null;
			character.mana.UseMana(spellData.manaCost);
			yield return OnDeactivate();
		}

		public void RemoveSkill()
		{
			OnRemoveSkill();
			Destroy(gameObject);
		}
		
		protected IEnumerator Co_StunTarget(AttackResult _atkResult)
		{
			if(target == null) yield break;
			if (_atkResult.attackResultType != AttackResultType.Weakness) yield break;

			var _stunPrefab = GameManager.Instance.GameDataBase.battleData.skipTurnSE;

			var _effectInstance = Instantiate(_stunPrefab, Vector3.zero, Quaternion.identity);
                
			var _dif = character.level - target.character.level;
			_dif = Mathf.Clamp(_dif, 1, 3);
			_effectInstance.SetDuration(_dif);
			
			yield return target.character.statusEffectReceiver.ApplyStatusEffect(_effectInstance, character.gameObject);
                
			var _txt = $"{target.character.characterData.characterName} is stunned for {_dif} turn(s)!";
			yield return BattleTextManager.DoWrite(_txt);
		}
		
		protected bool PlayAura(GameObject auraObj_)
		{
			if (auraObj_.IsEmptyOrDestroyed()) return false;
			if(target.IsEmptyOrDestroyed()) return false;
            
			auraObj_.transform.position = target.transform.position;
			auraObj_.transform.rotation = Quaternion.identity;

			if (auraObj_.TryGetComponent(out ParticleSystem _particleSystem))
			{
				_particleSystem.Play();
				return true;
			}
			if (auraObj_.TryGetComponent(out VisualEffect _visualEffect))
			{
				_visualEffect.Play();
				return true;
			}
            
			var _particleSystemInChild = auraObj_.GetComponentInChildren<ParticleSystem>();
            
			if (_particleSystemInChild.IsValid())
			{
				_particleSystemInChild.Play();
				return true;
			}
            
			var _visualEffectInChild = auraObj_.GetComponentInChildren<VisualEffect>();
            
			if (_visualEffectInChild.IsValid())
			{
				_visualEffectInChild.Play();
				return true;
			}
            
			return false;
		}
	}
}