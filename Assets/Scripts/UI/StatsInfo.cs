using System;
using Character;
using Managers;
using NaughtyAttributes;
using ScriptableObjectData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatsInfo : MonoBehaviour
    {
        [BoxGroup("Text")] [SerializeField] private TextMeshProUGUI maxHpTxt, maxMpTxt, phyTxt, armTxt, magDmgTxt, magResTxt, spdTxt, accTxt;
        [BoxGroup("Icon")] [SerializeField] private Image hpIcon, mpIcon, phyIcon, armIcon, magDmgIcon, magResIcon, spdIcon, accIcon;
        [SerializeField] private AssetDataBase assetDatabase;
        
        private CombatStats combatStats;

        private void Awake()
        {
            if(assetDatabase == null) assetDatabase = GameManager.Instance.GameDataBase.assetDataBase;
            
            if(hpIcon!= null) hpIcon.sprite = assetDatabase.healthIcon;
            if(mpIcon != null) mpIcon.sprite = assetDatabase.manaIcon;
            if(phyIcon != null) phyIcon.sprite = assetDatabase.phyDmgIcon;
            if(armIcon != null) armIcon.sprite = assetDatabase.armIcon;
            if(magDmgIcon != null) magDmgIcon.sprite = assetDatabase.magDmgIcon;
            if(magResIcon != null) magResIcon.sprite = assetDatabase.magResIcon;
            if(spdIcon != null) spdIcon.sprite = assetDatabase.spdIcon;
            if(accIcon != null) accIcon.sprite = assetDatabase.accIcon;
        }

        public void SetStats(CombatStats combatStats_, bool includeHp_ = true, bool includeMp_ = true)
        {
            combatStats = combatStats_;
            if(!gameObject.activeInHierarchy) gameObject.SetActive(true);
            
            // TODO: Remove label
            if(maxHpTxt != null && includeHp_) maxHpTxt.text = "MaxHP: " + combatStats.maxHp.ToString();
            if(maxMpTxt != null && includeMp_) maxMpTxt.text = "MaxMp: " + combatStats.maxMana.ToString();
            if(phyTxt != null) phyTxt.text = "Physical Damage: " + combatStats.physicalDamage.ToString();
            if(armTxt != null) armTxt.text = "Armor: " + combatStats.armor.ToString();
            if(magDmgTxt != null) magDmgTxt.text = "Magic Damage: " + combatStats.magicDamage.ToString();
            if(magResTxt != null) magResTxt.text = "Magic Resistance: " + combatStats.magicResistance.ToString();
            if(spdTxt != null) spdTxt.text = "Speed: " + combatStats.speed.ToString();
            if(accTxt != null) accTxt.text = "Accuracy: " + combatStats.accuracy.ToString();
        }
    }
}
