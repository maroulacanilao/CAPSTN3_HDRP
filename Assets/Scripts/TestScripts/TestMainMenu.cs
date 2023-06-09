using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMainMenu : MonoBehaviour
{
    [NaughtyAttributes.Scene] public string farmScene;
    [NaughtyAttributes.Scene] public string battleScene;

    public Image black;
    public Animator animator;

    bool hasFaded;

    public Camera mainCamera;
    public float speed = 1.0f;
    bool playClicked = false;

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
            if(mainCamera.fieldOfView >= 45)
            {
                mainCamera.fieldOfView -= 1;
            }
        }
    }

    public void OnClickFarm()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(farmScene);
    }

    public void OnClickBattle()
    {
        playClicked = true;
        black.enabled = true;
        StartCoroutine(Fading());
    }

    IEnumerator Fading()
    {
        animator.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        UnityEngine.SceneManagement.SceneManager.LoadScene(battleScene);
    }
}
