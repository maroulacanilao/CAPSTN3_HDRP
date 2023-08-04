using System;
using System.Collections;
using System.Collections.Generic;
using EnemyController.Inheritors;
using Fungus;
using Managers;
using Managers.SceneLoader.SceneTransition;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class FinalBattleSceneTransition : MonoBehaviour
{
    [SerializeField] private FadeTransition_Base fadeTransition;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private SidapaController sidapaController;
    [SerializeField] private GameObject cutsceneCam;
    [SerializeField] private string message = "SidapaDialog";

    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        fadeTransition.gameObject.SetActive(false);
        isPlaying = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StartCutscene();
    }
    private bool isPlaying;
    public async void StartCutscene()
    {
        if(isPlaying) return;
        isPlaying = true;
        fadeTransition.gameObject.SetActive(true);
        fadeTransition.Initialize();
        player.position = targetPosition.position;
        InputManager.DisableInput();
        TimeManager.PauseTime();
            
        await fadeTransition.StartTransition(false);
        InputManager.DisableInput();
        TimeManager.PauseTime();
        cutsceneCam.SetActive(true);
        

        sidapaController.DialogAnim();
        await Task.Delay(500);
        Time.timeScale = 1;
        await fadeTransition.StartTransition(true);
        
        await Task.Delay(100);
        Fungus.Flowchart.BroadcastFungusMessage(message);

        Destroy(gameObject);
    }
}
