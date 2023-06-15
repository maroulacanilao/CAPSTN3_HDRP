using System;
using CustomEvent;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(InputSystemUIInputModule))]
public class InputUIManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    public static InputSystemUIInputModule inputSystemUi { get; private set; }
    
    public static readonly Evt OnCancel = new Evt();
    public static readonly Evt<InputAction.CallbackContext> OnMove = new Evt<InputAction.CallbackContext>();
    
    // specific UI Buttons
    public static readonly Evt OnSwap = new Evt();
    public static readonly Evt OnMenu = new Evt();
    public static readonly Evt OnNext = new Evt();
    public static readonly Evt OnPrev = new Evt();

    public static InputAction MenuAction { get; private set; }
    public static InputAction SwapAction { get; private set; }

    private void OnEnable()
    {
        inputSystemUi = GetComponent<InputSystemUIInputModule>();
        
        inputSystemUi.cancel.ToInputAction().started += this.Cancel;
        inputSystemUi.move.action.performed += this.Move;
        
        // for specific UI Buttons
        if(!playerInput) return;
        playerInput.actions["Menu"].started += this.Menu;
        playerInput.actions["Swap"].started += this.Swap;
        playerInput.actions["Next"].started += this.Next;
        playerInput.actions["Prev"].started += this.Prev;
        SwapAction = playerInput.actions["Swap"];
        MenuAction = playerInput.actions["Menu"];
    }

    private void OnDisable()
    {
        inputSystemUi.cancel.ToInputAction().started -= this.Cancel;
        inputSystemUi.move.action.performed -= this.Move;

        // for specific UI Buttons
        if(!playerInput) return;
        playerInput.actions["Menu"].started -= this.Menu;
        playerInput.actions["Swap"].started -= this.Swap;
    }

    private void Move(InputAction.CallbackContext obj_)
    {
        OnMove.Invoke(obj_);
    }

    private void Cancel(InputAction.CallbackContext context_)
    {
        OnCancel.Invoke();
    }
    
    private void Swap(InputAction.CallbackContext context_)
    {
        OnSwap.Invoke();
    }
    
    private void Menu(InputAction.CallbackContext context_)
    {
        if(context_.phase != InputActionPhase.Started) return;
        OnMenu.Invoke();
    }
    
    private void Next(InputAction.CallbackContext context_)
    {
        if(context_.phase != InputActionPhase.Started) return;
        OnNext.Invoke();
    }
    
    private void Prev(InputAction.CallbackContext context_)
    {
        if(context_.phase != InputActionPhase.Started) return;
        OnPrev.Invoke();
    }
}
