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
    private void Awake()
    {
        if(!playerInput) return;
        playerInput.actions["Swap"].started += this.Swap;
    }
    
    private void OnEnable()
    {
        inputSystemUi = GetComponent<InputSystemUIInputModule>();
        
        inputSystemUi.cancel.ToInputAction().started += this.Cancel;
        inputSystemUi.move.action.performed += this.Move;
        
        // for specific UI Buttons
        if(!playerInput) return;
        playerInput.actions["Swap"].started += this.Swap;
    }

    private void OnDisable()
    {
        inputSystemUi.cancel.ToInputAction().started -= this.Cancel;
        inputSystemUi.move.action.performed -= this.Move;

        // for specific UI Buttons
        if(!playerInput) return;
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
}
