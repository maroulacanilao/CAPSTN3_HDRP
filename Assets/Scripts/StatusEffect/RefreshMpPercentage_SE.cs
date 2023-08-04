using System.Collections;
using System.Text;
using Managers;
using UI.Battle;
using UnityEngine;

namespace StatusEffect
{
    public class RefreshMpPercentage_SE : StatusEffectBase
    {
        [Range(0,1f)]
        [SerializeField] private float refreshPercentage = 0.1f;
        
        protected override IEnumerator OnActivate()
        {
            var _mpComp = Target.character.mana;
            var _maxMp = _mpComp.MaxMana;
            Debug.Log(_maxMp);
            var _amount = Mathf.RoundToInt(_mpComp.MaxMana * refreshPercentage);
            _amount = Mathf.Clamp(_amount, 1, int.MaxValue);
            
            Debug.Log(_amount);
            
            _mpComp.AddMana(_amount);

            SelfRemove();
            
            if(!GameManager.IsBattleSceneActive()) yield break;
            
            var _sb = new StringBuilder();
            _sb.Append(Target.character.characterData.characterName);
            _sb.Append(" gained ");
            _sb.Append(_amount);
            _sb.Append(" mana!");
            
            yield return BattleTextManager.DoWrite(_sb.ToString());
        }
        protected override void OnDeactivate()
        {
            
        }
    }
}
