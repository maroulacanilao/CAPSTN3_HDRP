using System;
using System.Collections;
using BattleSystem;
using Character;
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
					case SpellType.Physical:
					{
						var _phyDmg = character.stats.physicalDamage;
						return Mathf.RoundToInt(_phyDmg * spellData.physicalDamageModifier);
					}
					case SpellType.Magical:
					{
						var _magDmg = character.stats.magicDamage;
						return Mathf.RoundToInt(_magDmg * spellData.magicDamageModifier);
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
			character.manaComponent.UseMana(spellData.manaCost);
			yield return OnDeactivate();
		}

		public void RemoveSkill()
		{
			OnRemoveSkill();
			Destroy(gameObject);
		}
	}
}