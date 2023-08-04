using System;
using System.Threading.Tasks;
using BattleSystem;
using CustomHelpers;
using DG.Tweening;
using Managers;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Battle
{
    public class BattEndUI : MonoBehaviour
    {
        [SerializeField] private GameDataBase gameDataBase;
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject levelPanel;
        [SerializeField] private GameObject levelUpEffect;
        [SerializeField] private TextMeshProUGUI expTXT, LevelTXT;
        [SerializeField] private TextMeshProUGUI ResultTXT;
        [SerializeField] private Image expBar;
        [SerializeField] private StatsInfo statsInfo;
        [SerializeField] private Button returnBTN;

    
        private BattleResultType result;
        private PlayerData playerData;
    
        private void Awake()
        {
            BattleManager.OnBattleEnd.AddListener(ShowResult);
            returnBTN.onClick.AddListener(ReturnToFarmScene);
            playerData = gameDataBase.playerData;
        }

        private void OnEnable()
        {
            panel.SetActive(false);
        }

        private void OnDestroy()
        {
            BattleManager.OnBattleEnd.RemoveListener(ShowResult);
            returnBTN.onClick.RemoveListener(ReturnToFarmScene);
        }

        private void ShowResult(BattleResultType battleResult_)
        {
            Time.timeScale = 0f;
            result = battleResult_;

            panel.SetActive(true);

            switch (battleResult_)
            {
                case BattleResultType.Win:
                    DisplayVictoryScreen();
                    break;
                
                case BattleResultType.Lose:
                    DisplayLoseScreen();
                    break;
                
                case BattleResultType.Flee:
                default:
                    break;
            }

            ResultTXT.text = battleResult_ == BattleResultType.Win ? "Victory!" : "You Lost!";
            ResultTXT.color = battleResult_ == BattleResultType.Win ? Color.green : Color.red;
        }

        private void ReturnToFarmScene()
        {
            GameManager.OnExitBattle.Invoke(result);
        }

        private async void DisplayVictoryScreen()
        {
            levelUpEffect.SetActive(false);
            returnBTN.gameObject.SetActive(false);
            statsInfo.gameObject.SetActive(false);

            var _exp = BattleManager.Instance.GetTotalExp();
            var _lvlData = playerData.LevelData;
            var _prevExp = _lvlData.CurrentLevelExperience;

            ResultTXT.text = "Victory!";
            var _addedTxt = $" <color=green>+{_exp}</color>";
        
            LevelTXT.text = $"Level: {_lvlData.CurrentLevel}";
            expTXT.text = $"{_prevExp:0}/{_lvlData.NextLevelExperience:0}";
            expBar.fillAmount = (float) _lvlData.CurrentLevelExperience / _lvlData.NextLevelExperience;
        
            var _newExp = _prevExp + _exp;
            
            _newExp = Mathf.Clamp(_newExp, 0, _lvlData.CurrentExperienceNeeded);

            await Task.Delay(50);
            var _nxtLvlExp = _lvlData.NextLevelExperience;
            var _txtTween = DOTween.To(() => _prevExp, x => _prevExp = x, _newExp, 1f).OnUpdate(() =>
            {
                expTXT.text = $"{Mathf.Clamp(_prevExp,0,_nxtLvlExp):0}/{_lvlData.NextLevelExperience:0} {_addedTxt}";
            }).SetUpdate(true);

            await expBar.DOFillAmount(((float) _lvlData.CurrentLevelExperience + _exp)/ _lvlData.NextLevelExperience, 1f).SetUpdate(true).AsyncWaitForCompletion();
        
        
            if (_lvlData.AddExp(_exp))
            {
                expBar.fillAmount = 0;
            
                levelUpEffect.SetActive(true);
            
                LevelTXT.text = $"Level: {_lvlData.CurrentLevel}";
            
                _txtTween.Kill(true);
            
                _prevExp = 0;
                _newExp = _lvlData.CurrentLevelExperience;
            
                _txtTween = DOTween.To(() => _prevExp, x => _prevExp = x, _newExp, 1f).OnUpdate(() =>
                {
                    expTXT.text = $"{_prevExp:0}/{_lvlData.CurrentExperienceNeeded:0} {_addedTxt}";
                }).SetUpdate(true);
                
                ShowStatIncrease();
            
                await expBar.DOFillAmount((float) _lvlData.CurrentLevelExperience / _lvlData.CurrentExperienceNeeded, 1f)
                    .SetUpdate(true).AsyncWaitForCompletion();

                await Task.Delay(500);

                _txtTween.Kill();
            }
        
            returnBTN.gameObject.SetActive(true);
        }

        private async void DisplayLoseScreen()
        {
            levelPanel.SetActive(false);
            statsInfo.gameObject.SetActive(false);
            
            returnBTN.gameObject.SetActive(false);
            
            await Task.Delay(1000);
            
            if(returnBTN.IsValid()) returnBTN.gameObject.SetActive(true);
        }

        private void ShowStatIncrease()
        {
            statsInfo.gameObject.SetActive(false);
            var _level = playerData.LevelData.CurrentLevel;
            var _prevStats = playerData.statsData.GetLeveledStats(_level - 1);
            var _currentStats = playerData.statsData.GetLeveledStats(_level);
            
            var _difference = _currentStats - _prevStats;
            
            statsInfo.DisplayIncreaseDynamic(_difference,true);
        }
    }
}
