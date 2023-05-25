using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMainMenu : MonoBehaviour
{
    [NaughtyAttributes.Scene] public string farmScene;
    [NaughtyAttributes.Scene] public string battleScene;
    
    public void OnClickFarm()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(farmScene);
    }
    
    public void OnClickBattle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(battleScene);
    }
}
