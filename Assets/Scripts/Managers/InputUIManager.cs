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

    private void OnEnable()
    {
        inputSystemUi = GetComponent<InputSystemUIInputModule>();
        
        inputSystemUi.cancel.ToInputAction().started += this.Cancel;
        inputSystemUi.move.action.performed += this.Move;
    }

    private void OnDisable()
    {
        inputSystemUi.cancel.ToInputAction().started -= this.Cancel;
        inputSystemUi.move.action.performed -= this.Move;

    }

    private void Move(InputAction.CallbackContext obj_)
    {
        OnMove.Invoke(obj_);
    }

    private void Cancel(InputAction.CallbackContext context_)
    {
        OnCancel.Invoke();
    }
}
