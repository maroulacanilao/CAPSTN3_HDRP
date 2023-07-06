using System;
using System.Collections;
using BaseCore;
using CustomHelpers;
using DG.Tweening;
using ScriptableObjectData.CharacterData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class LevelHUD : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private TextMeshProUGUI levelText, expText;
        [SerializeField] private Image expBar, expBarBG;
        [SerializeField] private float effectDuration = 0.5f;

        private PlayerLevel lvlData => playerData.LevelData;

        private void OnEnable()
        {
            PlayerLevel.OnExperienceChanged.AddListener(UpdateExp);
            DisplayLevel();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            PlayerLevel.OnExperienceChanged.RemoveListener(UpdateExp);
        }

        private void UpdateExp(PlayerLevel.AddExpInfo addExpInfo_)
        {
            if(this.IsEmptyOrDestroyed()) return;
            if(!gameObject.activeInHierarchy) return;
            StartCoroutine(Co_UpdateExp(addExpInfo_));
        }

        private IEnumerator Co_UpdateExp(PlayerLevel.AddExpInfo addExpInfo_)
        {
            yield return Co_SetOtherObjectActive(true, 0);
            yield return new WaitForSeconds(0.1f);
            var _temp = addExpInfo_.prevScaledLvlExp + addExpInfo_.addedExp;
            var _fillAmount = _temp / (float) addExpInfo_.prevScaledLvlExpNeeded;
            
            var _neededExp = addExpInfo_.leveledUp ? addExpInfo_.prevNeededExp : lvlData.NextLevelExperience;
            var _currentExp = addExpInfo_.leveledUp ? _neededExp : lvlData.TotalExperience;
            
            Debug.Log($"Needed Exp: {_neededExp} | Current Exp: {_currentExp}");
            UpdateExpText(addExpInfo_.prevExp,_currentExp,_neededExp);
            
            yield return expBar.DOFillAmount(_fillAmount, effectDuration).SetUpdate(true).WaitForCompletion();


            if (!addExpInfo_.leveledUp)
            {
                yield return Co_SetOtherObjectActive(false, 2f);
                yield break;
            }
            
            // if Level Up
            levelText.text = $"Level {addExpInfo_.prevLevel}";

            var _size = levelText.transform.localScale;
            
            yield return levelText.transform.DOScale(_size*2f, effectDuration/4f)
                .SetUpdate(true).SetEase(Ease.OutBack).WaitForCompletion();
            
            yield return levelText.transform.DOScale(_size, effectDuration/4f)
                .SetUpdate(true).SetEase(Ease.OutBack).WaitForCompletion();
            
            levelText.text = $"Level {lvlData.CurrentLevel}";
            
            UpdateExpText(_currentExp,lvlData.TotalExperience,lvlData.NextLevelExperience);

            var _temp2 = lvlData.CurrentLevelExperience / (float) lvlData.CurrentExperienceNeeded;
            expBar.fillAmount = 0;
            
            yield return expBar.DOFillAmount(_temp2, effectDuration).SetUpdate(true).WaitForCompletion();
            
            DisplayLevel(false);
            
            yield return Co_SetOtherObjectActive(false, 2f);
        }
        
        private void UpdateExpText(int prevExp_, int newExp_, int neededExp_)
        {
            // var _txtTween = DOTween.To(() => prevExp_, x => _prevExp = x, _newExp, 1f).OnUpdate(() =>
            // {
            //     expTXT.text = $"{Mathf.Clamp(_prevExp,0,_nxtLvlExp):0}/{_lvlData.NextLevelExperience:0}";
            // }).SetUpdate(true);
            
            DOTween.To(() => prevExp_, x => prevExp_ = x, newExp_, effectDuration).OnUpdate(() =>
            {
                expText.text = $"{Mathf.Clamp(prevExp_,0,neededExp_):0} / {neededExp_:0}";
            }).SetUpdate(true);
        }
        
        private void DisplayLevel(bool willDisableOtherObjects_ = true)
        {
            levelText.text = $"Level {lvlData.CurrentLevel}";
            expText.text = $"{lvlData.TotalExperience} / {lvlData.NextLevelExperience}";
            
            expBar.fillAmount = lvlData.CurrentLevelExperience / (float) lvlData.CurrentExperienceNeeded;
            if(willDisableOtherObjects_) StartCoroutine(Co_SetOtherObjectActive(false, 0.5f));
        }
        
        private IEnumerator Co_SetOtherObjectActive(bool willActivate_, float initialDelay_)
        {
            yield return new WaitForSeconds(initialDelay_);
            expText.gameObject.SetActive(willActivate_);
            expBarBG.gameObject.SetActive(willActivate_);
        }
    }
}