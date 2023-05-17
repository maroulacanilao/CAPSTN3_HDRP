using UnityEngine;
using UnityEngine.UI;
using Character;
using DG.Tweening;
using TMPro;

namespace UI
{
    public class ManaBarUI : MonoBehaviour
    {
        [SerializeField] private ManaComponent manaComponent;
        [SerializeField] private Image manaBarFill;
        [SerializeField] private TextMeshProUGUI manaBarText;
        [SerializeField] private float effectDuration = 0.1f;
        
        private void Awake()
        {
            manaComponent.OnUseMana.AddListener(OnUpdateMana);
            manaComponent.OnAddMana.AddListener(OnUpdateMana);
        }
        
        private void OnDestroy()
        {
            manaComponent.OnUseMana.RemoveListener(OnUpdateMana);
            manaComponent.OnAddMana.RemoveListener(OnUpdateMana);
        }

        private void Start()
        {
            manaBarText.text = $"Mana: {manaComponent.CurrentMana}/{manaComponent.MaxMana}";
        }

        private void OnUpdateMana(ManaComponent manaComponent_)
        {
            var _percentage = (float)manaComponent_.CurrentMana / manaComponent_.MaxMana;
            Debug.Log(_percentage);
            UpdateHpText();
            manaBarFill.DOFillAmount(_percentage, effectDuration);
        }
        
        private void UpdateHpText()
        {
            var _prevVal = Mathf.FloorToInt(manaComponent.MaxMana * manaBarFill.fillAmount);
            int _targetVal = manaComponent.CurrentMana;
        
            DOTween.To(() => _prevVal, x => _prevVal = x, _targetVal, effectDuration).OnUpdate(() =>
            {
                manaBarText.text = $"Mana: {_prevVal}/{manaComponent.MaxMana}";
            });
        }
    }
}
