using UnityEngine;
using UnityEngine.UI;
using Character;
using DG.Tweening;
using TMPro;

namespace UI
{
    public class ManaBarUI : MonoBehaviour
    {
        [SerializeField] private CharacterMana characterMana;
        [SerializeField] private Image manaBarFill;
        [SerializeField] private TextMeshProUGUI manaBarText;
        [SerializeField] private float effectDuration = 0.1f;
        
        private void Awake()
        {
            characterMana.OnUseMana.AddListener(OnUpdateMana);
            characterMana.OnAddMana.AddListener(OnUpdateMana);
        }
        
        private void OnDestroy()
        {
            characterMana.OnUseMana.RemoveListener(OnUpdateMana);
            characterMana.OnAddMana.RemoveListener(OnUpdateMana);
        }

        private void Start()
        {
            manaBarText.text = $"Mana: {characterMana.CurrentMana}/{characterMana.MaxMana}";
        }

        private void OnUpdateMana(CharacterMana characterMana_)
        {
            var _percentage = (float)characterMana_.CurrentMana / characterMana_.MaxMana;
            UpdateHpText();
            manaBarFill.DOFillAmount(_percentage, effectDuration);
        }
        
        private void UpdateHpText()
        {
            var _prevVal = Mathf.FloorToInt(characterMana.MaxMana * manaBarFill.fillAmount);
            int _targetVal = characterMana.CurrentMana;
        
            DOTween.To(() => _prevVal, x => _prevVal = x, _targetVal, effectDuration).OnUpdate(() =>
            {
                manaBarText.text = $"Mana: {_prevVal}/{characterMana.MaxMana}";
            });
        }
    }
}
