using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvent;
using CustomHelpers;
using DG.Tweening;
using UnityEngine;

public class DamageTextUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMPro.TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float upValue = 2f, duration = 1f;
    [SerializeField] private Color damageColor, healColor;
    public static readonly Evt<Vector3, string> ShowDamageText = new Evt<Vector3, string>();
    public static readonly Evt<Vector3, string> ShowHealText = new Evt<Vector3, string>();

    private void Awake()
    {
        ShowDamageText.AddListener(ShowDamage);
        ShowHealText.AddListener(ShowHeal);
        panel.SetActive(false);
    }

    private void OnDestroy()
    {
        ShowDamageText.RemoveListener(ShowDamage);
        ShowHealText.RemoveListener(ShowHeal);
    }
    
    private void ShowDamage(Vector3 pos_, string val_)
    {
        SetPosition(pos_);
        StartCoroutine(ShowText(val_, damageColor));
    }
    
    private void ShowHeal(Vector3 pos_, string val_)
    {
        SetPosition(pos_);
        StartCoroutine(ShowText(val_, healColor));
    }
    
    private IEnumerator ShowText(string val_, Color color_)
    {
        textMeshProUGUI.text = val_;
        textMeshProUGUI.color = color_;
        panel.SetActive(true);
        
        var _targetY = transform.position.y + upValue;
        yield return panel.transform.
            DOMoveY(_targetY, duration).
            SetEase(Ease.Linear).WaitForCompletion();
        
        panel.SetActive(false);
        panel.transform.localPosition = Vector3.zero;
    }
    
    private void SetPosition(Vector3 pos_)
    {
        if(transform.IsEmptyOrDestroyed()) return;
        transform.position = pos_ + offset;
    }
}
