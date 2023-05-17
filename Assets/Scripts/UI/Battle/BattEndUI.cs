using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using TMPro;
using UnityEngine;

public class BattEndUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI ResultTXT;
    
    private void Awake()
    {
        BattleManager.OnBattleEnd.AddListener(ShowResult);
    }
    
    private void OnDestroy()
    {
        BattleManager.OnBattleEnd.RemoveListener(ShowResult);
    }

    private void ShowResult(bool playerWon_)
    {
        Time.timeScale = 0f;
        ResultTXT.text = playerWon_ ? "You Won!" : "You Lost!";
        panel.SetActive(true);
    }
}
