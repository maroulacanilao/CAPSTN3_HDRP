using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem;
using Managers;
using Managers.SceneManager;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattEndUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI ResultTXT;
    [SerializeField] private BattleData battleData;
    [SerializeField] private EventQueueData eventQueueData;
    [SerializeField] private Button returnBTN;
    
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
        ResultTXT.text = playerWon_ ? "You Won!" : "You Lost!";
        returnBTN.gameObject.SetActive(playerWon_);
        panel.SetActive(true);
    }

    private void ReturnToFarmScene()
    {
        GameManager.OnExitBattle.Invoke(playerWon);
    }
}
