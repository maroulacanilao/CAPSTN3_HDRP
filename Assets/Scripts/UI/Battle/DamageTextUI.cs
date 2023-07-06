using System;
using System.Collections;
using System.Text;
using BattleSystem;
using CustomEvent;
using CustomHelpers;
using DG.Tweening;
using UnityEngine;

namespace UI.Battle
{
    public class DamageTextUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TMPro.TextMeshProUGUI textMeshProUGUI;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float upValue = 2f, duration = 1f;
        [SerializeField] private Color damageColor, healColor;
        [SerializeField] private float textPadding = 10f;
        
        public static readonly Evt<Vector3, AttackResult> ShowDamageText = new Evt<Vector3, AttackResult>();
        private RectTransform rectPanel;
        private float defaultTextSize;
        
        private void Awake()
        {
            rectPanel = panel.GetComponent<RectTransform>();
            ShowDamageText.AddListener(ShowDamage);
            panel.SetActive(false);
            defaultTextSize = textMeshProUGUI.fontSize;
        }

        private void OnDestroy()
        {
            ShowDamageText.RemoveListener(ShowDamage);
        }
    
        private void ShowDamage(Vector3 pos_, AttackResult res_)
        {
            SetPosition(pos_);
            var message = "";
            
            switch (res_.attackResultType)
            {
                case AttackResultType.Miss:
                    message = "Miss";
                    textMeshProUGUI.fontSize = defaultTextSize;
                    break;
                case AttackResultType.Hit:
                    message = res_.damageInfo.DamageAmount.ToString();
                    textMeshProUGUI.fontSize = defaultTextSize;
                    break;
                case AttackResultType.Critical:
                    var _builder = new StringBuilder();
                    _builder.Append("<color=yellow>CRITICAL</color>");
                    _builder.Append("\n");
                    _builder.Append(res_.damageInfo.DamageAmount);
                    message = _builder.ToString();
                    textMeshProUGUI.fontSize = defaultTextSize * 2f;
                    break;
                
                case AttackResultType.Weakness:
                    var _builder2 = new StringBuilder();
                    _builder2.Append("<color=orange>WEAKNESS</color>");
                    _builder2.Append("\n");
                    _builder2.Append(res_.damageInfo.DamageAmount);
                    message = _builder2.ToString();
                    textMeshProUGUI.fontSize = defaultTextSize * 2f;
                    break;
            }
            
            StartCoroutine(ShowText(message, damageColor));
        }
    
        private void ShowHeal(Vector3 pos_, string val_)
        {
            SetPosition(pos_);
            StartCoroutine(ShowText(val_, healColor));
        }
    
        private IEnumerator ShowText(string val_, Color color_)
        {
            Debug.Log(val_);
            textMeshProUGUI.text = val_;
            textMeshProUGUI.color = color_;
            
            rectPanel.sizeDelta = new Vector2(textMeshProUGUI.preferredWidth + textPadding * 2, textMeshProUGUI.preferredHeight + textPadding * 2);
            
            panel.SetActive(true);
            
            var _targetY = transform.position.y + upValue;
            
            yield return panel.transform.
                DOMoveY(_targetY, duration).
                SetEase(Ease.Linear).SetUpdate(true).WaitForCompletion();
        
            panel.SetActive(false);
            panel.transform.localPosition = Vector3.zero;
        }
    
        private void SetPosition(Vector3 pos_)
        {
            if(transform.IsEmptyOrDestroyed()) return;
            transform.position = pos_ + offset;
        }
    }
}
