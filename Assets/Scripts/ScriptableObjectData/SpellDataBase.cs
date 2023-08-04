using AYellowpaper.SerializedCollections;
using Items.ItemData;
using NaughtyAttributes;
using Shop;
using Spells.Base;
using UnityEngine;

namespace ScriptableObjectData
{
    [CreateAssetMenu(fileName = "SpellDataBase", menuName = "ScriptableObjects/SpellDataBase", order = 0)]
    public class SpellDataBase : ScriptableObject
    {
        [field:SerializeField] public SerializedDictionary<string, SpellData> spellDataLookup { get; private set; }
        [field:SerializeField] public SerializedDictionary<SpellData, OfferRequirement> spellOffers { get; private set; }


        [Button("Get All Spell Data")]
        private void GetAllSpellData()
        {
            spellDataLookup ??= new SerializedDictionary<string, SpellData>();
            spellOffers ??= new SerializedDictionary<SpellData, OfferRequirement>();
            
            var _spellDataList = Resources.LoadAll<SpellData>("Data/SpellData");

            foreach (var _spell in _spellDataList)
            {
                if(_spell.ForEnemyOnly) continue;
                spellDataLookup.TryAdd(_spell.name, _spell);
                spellOffers.TryAdd(_spell, new OfferRequirement());
            }
        }
    }
}