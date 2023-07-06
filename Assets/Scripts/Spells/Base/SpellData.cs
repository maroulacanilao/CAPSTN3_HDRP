using System;
using System.Collections.Generic;
using Character.CharacterComponents;
using NaughtyAttributes;
using UnityEngine;

namespace Spells.Base
{
	[System.Serializable]
	public enum SpellType { Passive = 1, Damage = 2, Buff = 4, DeBuff = 5, Heal = 6 }

	[CreateAssetMenu(menuName = "ScriptableObjects/SpellData", fileName = "SpellData")]
	public class SpellData : ScriptableObject
	{
		[field: BoxGroup("Description")] [field: SerializeField] 
		public string spellName { get; protected set; }
		
		[field: BoxGroup("Description")] [field: SerializeField] 
		public SpellType spellType { get; protected set; }
		
		[field: BoxGroup("Description")] [field: SerializeField] 
		public int manaCost { get; protected set; }
		
		[field: BoxGroup("Description")] [field: SerializeField] [field: ResizableTextArea]
		public string Description { get; protected set; }
		
		[field: BoxGroup("Description")] [field: SerializeField] [field: ShowAssetPreview(32,32)]
		public Sprite icon { get; protected set; }

		
		[field: BoxGroup("Spell Behavior")] [field: SerializeField]
		public bool hasStatusEffect { get; protected set; }
		
		[field: BoxGroup("Spell Behavior")] [field: SerializeField] [field: ShowIf("hasStatusEffect")]
		public StatusEffectBase statusEffect { get; protected set; }

		[field: BoxGroup("Damage Properties")] [field: SerializeField] [field: Range(0,2f)]
		[field: ShowIf("spellType", SpellType.Damage )] 
		public float damageModifier { get; protected set; }

		[field: BoxGroup("Description")]
		[field: SerializeField] public List<string> tags { get; protected set; }
		
		[SerializeField] private SpellObject spellPrefab;
		
		public SpellObject GetSpellObject(SpellUser user_)
		{
			if (spellPrefab == null) throw new Exception("Spell Prefab is null");
			
			var _spell = Instantiate(spellPrefab);
			return _spell.Initialize(user_, this);
		}
	}
}