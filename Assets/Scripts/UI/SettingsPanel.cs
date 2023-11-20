using System;
using System.Collections;
using System.Collections.Generic;
using Settings;
using TMPro;
using UI.Farming;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsPanel : PlayerMenu
{
    [SerializeField] private SettingsData settingsData;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button applyBtn;
    [SerializeField] private Button resetBtn;

    [SerializeField] private TextMeshProUGUI masterVolumeText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;

    private SettingsValues settings => settingsData.settings;

    private void Awake()
    { 
        masterSlider.onValueChanged.AddListener(OnSliderValueChanged);
        musicSlider.onValueChanged.AddListener(OnSliderValueChanged);
        sfxSlider.onValueChanged.AddListener(OnSliderValueChanged);
        applyBtn.onClick.AddListener(OnApplyBtnClicked);
        resetBtn.onClick.AddListener(OnResetBtnClicked);
    }

    private void OnDestroy()
    {
        masterSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        musicSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        applyBtn.onClick.RemoveListener(OnApplyBtnClicked);
        resetBtn.onClick.RemoveListener(OnResetBtnClicked);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Display();
        masterSlider.Select();
        EventSystem.current.SetSelectedGameObject(masterSlider.gameObject);
        Canvas.ForceUpdateCanvases();
        UpdateVolumeTexts();
    }
    
    private void Display()
    {
        masterSlider.SetValueWithoutNotify(settings.masterVolume);
        musicSlider.SetValueWithoutNotify(settings.musicVolume);
        sfxSlider.SetValueWithoutNotify(settings.sfxVolume);
        applyBtn.interactable = false;
    }

    private void OnSliderValueChanged(float value_)
    {
        applyBtn.interactable = true;
        UpdateVolumeTexts();
    }

    private void OnApplyBtnClicked()
    {
        SettingsUtil.SetVolumes(masterSlider.value, musicSlider.value, sfxSlider.value);
    }

    private void OnResetBtnClicked()
    {
        settingsData.ResetSettings();
        // SettingsUtil.SetVolumes(masterSlider.value, musicSlider.value, sfxSlider.value);
    }

    private void UpdateVolumeTexts()
    {
        masterVolumeText.text = $"{Convert.ToInt16(Mathf.Clamp01(masterSlider.value) * 100)}";
        musicVolumeText.text = $"{Convert.ToInt16(Mathf.Clamp01(musicSlider.value) * 100)}";
        sfxVolumeText.text = $"{Convert.ToInt16(Mathf.Clamp01(sfxSlider.value) * 100)}";
    }
}
