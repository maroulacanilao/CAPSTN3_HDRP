using System;
using CustomEvent;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(InputSystemUIInputModule))]
public class InputUIManager : MonoBehaviour
{
    public static InputSystemUIInputModule inputSystemUi { get; private set; }
    
    public static readonly Evt OnCancel = new Evt();
    public static readonly Evt<InputAction.CallbackContext> OnMove = new Evt<InputAction.CallbackContext>();
    
    // specific UI Buttons
    public static readonly Evt OnSwap = new Evt();
    public static readonly Evt OnMenu = new Evt();
    public static readonly Evt OnNext = new Evt();
    public static readonly Evt OnPrev = new Evt();
    public static readonly Evt OnInventoryMenu = new Evt();
    public static readonly Evt OnCharacterInfo = new Evt();
    public static readonly Evt OnCodexMenu = new Evt();
    

    public void OnEnable()
    {
        inputSystemUi = GetComponent<InputSystemUIInputModule>();
        
        inputSystemUi.cancel.ToInputAction().started += this.Cancel;
        inputSystemUi.move.action.performed += this.Move;
    }

    public void OnDisable()
    {
        inputSystemUi.cancel.ToInputAction().started -= this.Cancel;
        inputSystemUi.move.action.performed -= this.Move;
    }

    public void Move(InputAction.CallbackContext context_)
    {
        if(!context_.started) return;
        OnMove.Invoke(context_);
    }

    public void Cancel(InputAction.CallbackContext context_)
    {
        if(!context_.started) return;
        OnCancel.Invoke();
    }
    
    public void Swap(InputAction.CallbackContext context_)
    {
        if(!context_.started) return;
        OnSwap.Invoke();
    }
    
    public void Menu(InputAction.CallbackContext context_)
    { 
        if(!context_.started) return;
        OnMenu.Invoke();
    }
    
    public void Next(InputAction.CallbackContext context_)
    {
        if(!context_.started) return;
        OnNext.Invoke();
    }
    
    public void Prev(InputAction.CallbackContext context_)
    {
        if(!context_.started) return;
        OnPrev.Invoke();
    }
    
    public void InventoryMenu(InputAction.CallbackContext context_)
    {
        if(!context_.started) return;
        OnInventoryMenu.Invoke();
    }
    
    public void CharacterInfo(InputAction.CallbackContext context_)
    {
        if(!context_.started) return;
        OnCharacterInfo.Invoke();
    }
    
    public void CodexMenu(InputAction.CallbackContext context_)
    {
        if(!context_.started) return;
        OnCodexMenu.Invoke();
    }
}
