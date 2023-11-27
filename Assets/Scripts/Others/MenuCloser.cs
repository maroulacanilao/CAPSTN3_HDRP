using System.Collections;
using System.Collections.Generic;
using UI.Farming;
using UnityEngine;

public class MenuCloser : MonoBehaviour
{
    public void CloseMenu() => PlayerMenuManager.OnCloseAllUI.Invoke();
    public void CloseMenuRevised() => RevisedPlayerMenuManager.OnCloseAllUIRevised.Invoke();
}
