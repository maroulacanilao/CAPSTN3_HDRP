using System;
using System.Collections;
using System.Collections.Generic;
using Managers.SceneManager;
using UnityEngine;

public class TestSceneLoader : MonoBehaviour
{
    [SerializeField] [NaughtyAttributes.Scene] private string sceneName;
    [SerializeField] LoadSceneType loadSceneType;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            SceneLoader.OnLoadScene.Invoke(sceneName, loadSceneType);
    }
}
