using BaseCore;
using Character.CharacterComponents;
using DG.Tweening;
using Items.ItemData.Tools;
using Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WateringCanRefill : MonoBehaviour
{
    [SerializeField] private Slider slider;

    // Start is called before the first frame update
    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    private void OnEnable()
    {
        PlayerEquipment.OnRefillAction.AddListener(SetMaxUsage);
        PlayerEquipment.OnRefillReduced.AddListener(SetUsage);
    }

    private void OnDisable()
    {
        PlayerEquipment.OnRefillAction.RemoveListener(SetMaxUsage);
        PlayerEquipment.OnRefillReduced.RemoveListener(SetUsage);
    }

    public void SetUsage(int usage)
    {
        slider.value = usage;
        if (usage < 0) slider.value = 0;
    }

    private void SetMaxUsage(int usage)
    {
        slider.maxValue = usage;
        slider.value = usage;
    }
}
