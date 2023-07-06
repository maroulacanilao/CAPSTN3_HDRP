using System;
using System.Threading.Tasks;
using BattleSystem;
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

    
        private bool didPlayerWon;
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

        private void ShowResult(bool playerWon_)
        {
            Time.timeScale = 0f;
            didPlayerWon = playerWon_;

            panel.SetActive(true);
        
            if (playerWon_) DisplayVictoryScreen();
            else DisplayLoseScreen();
        
            ResultTXT.text = playerWon_ ? "Victory!" : "You Lost!";
        }

        private void ReturnToFarmScene()
        {
            GameManager.OnExitBattle.Invoke(didPlayerWon);
        }

        private async void DisplayVictoryScreen()
        {
            levelUpEffect.SetActive(false);
            returnBTN.gameObject.SetActive(false);
            statsInfo.gameObject.SetActive(false);

            var _exp = BattleManager.Instance.GetTotalExp();
            var _lvlData = playerData.LevelData;
            var _prevExp = _lvlData.TotalExperience;

            ResultTXT.text = "Victory!";
        
            LevelTXT.text = $"Level: {_lvlData.CurrentLevel}";
            expTXT.text = $"{_prevExp:0}/{_lvlData.NextLevelExperience:0}";
            expBar.fillAmount = (float) _lvlData.CurrentLevelExperience / _lvlData.NextLevelExperience;
        
            var _newExp = _lvlData.TotalExperience + _exp;

            await Task.Delay(50);
            var _nxtLvlExp = _lvlData.NextLevelExperience;
            var _txtTween = DOTween.To(() => _prevExp, x => _prevExp = x, _newExp, 1f).OnUpdate(() =>
            {
                expTXT.text = $"{Mathf.Clamp(_prevExp,0,_nxtLvlExp):0}/{_lvlData.NextLevelExperience:0}";
            }).SetUpdate(true);

            await expBar.DOFillAmount(((float) _lvlData.CurrentLevelExperience + _exp)/ _lvlData.NextLevelExperience, 1f).SetUpdate(true).AsyncWaitForCompletion();
        
        
            if (_lvlData.AddExp(_exp))
            {
                expBar.fillAmount = 0;
            
                levelUpEffect.SetActive(true);
            
                LevelTXT.text = $"Level: {_lvlData.CurrentLevel}";
            
                _txtTween.Kill(true);
            
                _prevExp = _lvlData.PrevLevelExperience;
            
                _txtTween = DOTween.To(() => _prevExp, x => _prevExp = x, _newExp, 1f).OnUpdate(() =>
                {
                    expTXT.text = $"{_prevExp:0}/{_lvlData.NextLevelExperience:0}";
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
            
            returnBTN.gameObject.SetActive(true);
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
