using System;
using System.Collections;
using System.Collections.Generic;
using Settings;
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
    
    private SettingsValues settings => settingsData.settings;

    private void Awake()
    { 
        masterSlider.onValueChanged.AddListener(OnSliderValueChanged);
        musicSlider.onValueChanged.AddListener(OnSliderValueChanged);
        sfxSlider.onValueChanged.AddListener(OnSliderValueChanged);
        applyBtn.onClick.AddListener(OnApplyBtnClicked);
    }

    private void OnDestroy()
    {
        masterSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        musicSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        applyBtn.onClick.RemoveListener(OnApplyBtnClicked);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Display();
        masterSlider.Select();
        EventSystem.current.SetSelectedGameObject(masterSlider.gameObject);
        Canvas.ForceUpdateCanvases();
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
    }

    private void OnApplyBtnClicked()
    {
        SettingsUtil.SetVolumes(masterSlider.value, musicSlider.value, sfxSlider.value);
    }
}
