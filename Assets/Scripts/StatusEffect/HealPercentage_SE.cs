using System.Collections;
using System.Text;
using BaseCore;
using CustomHelpers;
using Managers;
using NaughtyAttributes;
using ObjectPool;
using UI.Battle;
using UnityEngine;

namespace StatusEffect
{
    public class HealPercentage_SE : StatusEffectBase
    {
        [Range(0,1f)]
        [SerializeField] private float healPercentage = 0.1f;
        
        [SerializeField] private string vfxName;

        protected override IEnumerator OnActivate()
        {
            var _hpCom = Target.character.health;
            var _amount = Mathf.RoundToInt(_hpCom.MaxHp * healPercentage);
            
            HealInfo _healInfo = new HealInfo(_amount, Source);

            Target.character.Heal(_healInfo);

            SelfRemove();
            
            if(!GameManager.IsBattleSceneActive()) yield break;
            
            AssetHelper.PlayEffectCoroutine(vfxName, Target.character.transform, Vector3.zero, Quaternion.identity);
            
            var _sb = new StringBuilder();
            _sb.Append(Target.character.characterData.characterName);
            _sb.Append(" was healed for ");
            _sb.Append(_healInfo.HealAmount);
            _sb.Append(" HP!");
            
            yield return BattleTextManager.DoWrite(_sb.ToString());
        }
        
        protected override void OnDeactivate()
        {
            
        }
        
    }
}
