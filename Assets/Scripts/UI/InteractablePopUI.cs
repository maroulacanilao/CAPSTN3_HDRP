using BaseCore;
using CustomEvent;
using CustomHelpers;
using UnityEngine;

namespace UI
{
    public class InteractablePopUI : MonoBehaviour
    {
        // [SerializeField] private GameObject popUpUI;
        //
        // private void Awake()
        // {
        //     popUpUI.SetActive(false);
        //     InteractableObject.OnEnter.AddListener(ShowPopUpUI);
        //     InteractableObject.OnExit.AddListener(ClosePopUpUI);
        // }
        //
        // private void OnDestroy()
        // {
        //     InteractableObject.OnEnter.RemoveListener(ShowPopUpUI);
        //     InteractableObject.OnExit.RemoveListener(ClosePopUpUI);
        // }
        //
        // private void ShowPopUpUI(InteractableObject obj_)
        // {
        //     if(!obj_.showIcon) return;
        //     popUpUI.SetActive(true);
        //     transform.position = obj_.transform.position.Add(0,1f,0.25f);
        // }
        //
        // private void ClosePopUpUI(InteractableObject obj_)
        // {
        //     if(!obj_.showIcon) return;
        //     popUpUI.SetActive(false);
        // }
    }
}
