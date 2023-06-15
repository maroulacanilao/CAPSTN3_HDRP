using System.Threading.Tasks;
using BattleSystem;
using DG.Tweening;
using Managers;
using ScriptableObjectData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Battle
{
    public class BattEndUI : MonoBehaviour
    {
        [SerializeField] private GameDataBase gameDataBase;
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI ResultTXT;
        [SerializeField] private Button returnBTN;
        [SerializeField] private TextMeshProUGUI expTXT, LevelTXT;
        [SerializeField] private Image expBar;
        [SerializeField] private GameObject levelUpEffect;
    
        private bool playerWon;
    
        private void Awake()
        {
            BattleManager.OnBattleEnd.AddListener(ShowResult);
            returnBTN.onClick.AddListener(ReturnToFarmScene);
        }
    
        private void OnDestroy()
        {
            BattleManager.OnBattleEnd.RemoveListener(ShowResult);
            returnBTN.onClick.RemoveListener(ReturnToFarmScene);
        }

        private void ShowResult(bool playerWon_)
        {
            Time.timeScale = 0f;
            playerWon = playerWon_;

            panel.SetActive(true);
        
            if (playerWon_) DisplayVictoryScreen();
        
            ResultTXT.text = playerWon_ ? "Victory!" : "You Lost!";
        }

        private void ReturnToFarmScene()
        {
            GameManager.OnExitBattle.Invoke(playerWon);
        }

        private async void DisplayVictoryScreen()
        {
            levelUpEffect.SetActive(false);
            returnBTN.gameObject.SetActive(false);

            var _exp = BattleManager.Instance.GetTotalExp();
            var _lvlData = gameDataBase.playerData.LevelData;
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
            
                await expBar.DOFillAmount((float) _lvlData.CurrentLevelExperience / _lvlData.NextLevelExperience, 1f)
                    .SetUpdate(true).AsyncWaitForCompletion();
            
                _txtTween.Kill();
            }
        
            returnBTN.gameObject.SetActive(true);
        }
    }
}
