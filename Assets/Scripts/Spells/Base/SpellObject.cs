using System;
using System.Collections;
using BattleSystem;
using Character;
using Character.CharacterComponents;
using Managers;
using UI.Battle;
using UnityEngine;

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
					case SpellType.Damage:
					{
						var _magDmg = character.stats.strength;
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
                
			target.character.statusEffectReceiver.ApplyStatusEffect(_effectInstance, character.gameObject);
                
			var _txt = $"{target.character.characterData.characterName} is stunned for {_dif} turn(s)!";
			yield return BattleTextManager.DoWrite(_txt);
		}
	}
}