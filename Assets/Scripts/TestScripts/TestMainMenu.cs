using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using NaughtyAttributes;
using ScriptableObjectData;
using UnityEngine;
using UnityEngine.UI;

public class TestMainMenu : MonoBehaviour
{
    public GameDataBase GameDataBase;
    
    [NaughtyAttributes.Scene] [BoxGroup("Scenes")] public string tutorialScene;
    [NaughtyAttributes.Scene] [BoxGroup("Scenes")] public string farmScene;
    [NaughtyAttributes.Scene] [BoxGroup("Scenes")] public string battleScene;

    [BoxGroup("Effect UI")] public Image black;
    [BoxGroup("Effect UI")] public Animator animator;
    [BoxGroup("Effect UI")] public Camera mainCamera;
    [BoxGroup("Effect UI")] public float speed = 1.0f;
    
    [BoxGroup("Buttons")] public Button newGameButton;
    [BoxGroup("Buttons")] public Button loadGameButton;
    [BoxGroup("Buttons")] public Button quitGameButton;
    
    [BoxGroup("Panels")] public GameObject newGameConfirmPanel;
    [BoxGroup("Panels")] public GameObject loadPanel;
    [BoxGroup("Panels")] public GameObject loadUnsuccessfulPanel;

    private bool hasFaded;
    private bool playClicked = false;
    private ProgressionData progressionData => GameDataBase.progressionData;

    private void Awake()
    {
        newGameButton.onClick.AddListener(OnNewGameClick);
        loadGameButton.onClick.AddListener(OnLoadGameClick);
        quitGameButton.onClick.AddListener(OnClickQuit);
    }

    private void Start()
    {
        loadGameButton.gameObject.SetActive(progressionData.saveData != null);
        TimeManager.ResetData();
    }

    private void OnDestroy()
    {
        Debug.Log("Destroying");
    }

    public MainMenuCameraZoom cameraZoom;

    void LateUpdate()
    {
        if (hasFaded != true)
        {
            if (black.color.a == 0)
            {
                black.enabled = false;
                hasFaded = true;
            }
        }

        if(playClicked != false)
        {
            cameraZoom.ZoomActive = true;
        }
    }

    IEnumerator Fading()
    {
        animator.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        //UnityEngine.SceneManagement.SceneManager.LoadScene(battleScene);
    }

    public void OnNewGameClick()
    {
        if(progressionData.saveData != null)
        {
            newGameConfirmPanel.SetActive(true);
        }
        else
        {
            NewGame();
        }
    }

    private void OnLoadGameClick()
    {
        GameDataBase.ReInitialize();
        GameDataBase.sessionData.ReInitialize();
        progressionData.ResetProgress();
        StartCoroutine(Co_Load());
    }
    
    public void OnClickQuit()
    {
        Application.Quit();
    }
    
    
    public void NewGame()
    {
        playClicked = true;
        black.enabled = true;
        StartCoroutine(Fading());
        GameDataBase.ReInitialize();
        GameDataBase.sessionData.ReInitialize();
        GameDataBase.sessionData.farmLoadType = FarmLoadType.NewGame;
        GameDataBase.progressionData.ResetProgress();
        UnityEngine.SceneManagement.SceneManager.LoadScene(tutorialScene);
    }

    private IEnumerator Co_Load()
    {
        animator.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        loadPanel.SetActive(true);
        yield return progressionData.LoadProgression();
        
        if(progressionData.isLoadSuccessful)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(farmScene);
        }
        else
        {
            loadPanel.SetActive(false);
            loadUnsuccessfulPanel.SetActive(true);
        }
    }
}
