using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [BoxGroup("Loading Panel")] public GameObject loadPanel;
    [BoxGroup("Loading Panel")] public Slider loadingSlider;
    [BoxGroup("Loading Panel")] public TextMeshProUGUI progressTxt;
    [BoxGroup("Loading Panel")] public GameObject sunIcon;
    [BoxGroup("Loading Panel")] public float spinningSpeed;

    private void Awake()
    {
        TestMainMenu.OnLoadingScreenStart.AddListener(OnLoadingStarted);
    }

    private void OnLoadingStarted(string sceneName)
    {
        StartCoroutine(CO_LoadingScene(sceneName));
    }

    private IEnumerator CO_LoadingScene(string sceneName)
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        loadPanel.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;
            progressTxt.text = $"{progress * 100}%";
            sunIcon.transform.Rotate(new Vector3(0, 0, 1) * spinningSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
