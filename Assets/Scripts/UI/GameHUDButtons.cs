using CustomEvent;
using System.Collections;
using System.Collections.Generic;
using UI.HUD;
using UI.TabMenu.InventoryMenu;
using UnityEngine;

public class GameHUDButtons : MonoBehaviour
{
    [SerializeField] private GameObject objButton;
    [SerializeField] private GameObject inventoryButton;

    public static readonly Evt OnObjButtonClicked = new();

    public void ObjectiveButton()
    {
        OnObjButtonClicked.Invoke();
    }

    public void InventoryButton()
    {
        InputUIManager.OnInventoryMenu.Invoke();
    }
}
