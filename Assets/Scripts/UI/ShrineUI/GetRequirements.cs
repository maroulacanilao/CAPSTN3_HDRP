using Items.Inventory;
using Items;
using Items.ItemData;
using ScriptableObjectData;
using Shop;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GetRequirements : MonoBehaviour
{
    [SerializeField] protected GameDataBase gameDataBase;

    protected PlayerInventory inventory => gameDataBase.playerInventory;
    protected ItemDatabase itemDatabase => gameDataBase.itemDatabase;

    [SerializeField] private Image requiredItemIcon;
    [SerializeField] private GameObject countBG;
    [SerializeField] private TextMeshProUGUI countTxt;

    private void Awake()
    {
        countBG.SetActive(false);
    }

    public void Set(OfferRequirement requirement_)
    {
        var _requirementData = requirement_.consumableData;

        if (!inventory.StackableDictionary.TryGetValue(_requirementData, out var _stackable))
        {
            countTxt.text = $"00/{requirement_.count}";
        }

        if (_stackable.StackCount < requirement_.count)
        {
            countTxt.text = $"{_stackable.StackCount}/{requirement_.count}";
        }

        requiredItemIcon.sprite = requirement_.consumableData.Icon;
        countBG.SetActive(true);
    }

}
