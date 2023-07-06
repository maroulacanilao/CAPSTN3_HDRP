using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonSelectFirst : MonoBehaviour
    {
        private Button button;
        private SelectableMenuButton selectableMenuButton;

        private void Reset()
        {
            button = GetComponent<Button>();
            selectableMenuButton = GetComponent<SelectableMenuButton>();
        }

        private void OnValidate()
        {
            button = GetComponent<Button>();
            selectableMenuButton = GetComponent<SelectableMenuButton>();
        }

        private void Awake()
        {
            if(button == null) button = GetComponent<Button>();
            
            button.Select();
        }

        private void OnEnable()
        {
            EventSystem.current.firstSelectedGameObject = button.gameObject;
            EventSystem.current.SetSelectedGameObject(button.gameObject);
            button.Select();
            if (selectableMenuButton != null)
            {
                selectableMenuButton.SelectButton();
            }
            InputUIManager.OnMove.AddListener(Move);
        }

        private void OnDisable()
        {
            InputUIManager.OnMove.RemoveListener(Move);
        }

        private void Move(InputAction.CallbackContext context_)
        {
            if(!gameObject.activeInHierarchy) return;
            var _selected = EventSystem.current.currentSelectedGameObject;
            if(_selected != null && _selected.activeInHierarchy) return;
        
            Debug.Log("Select");
            EventSystem.current.SetSelectedGameObject(button.gameObject);
            button.Select();
            Debug.Log(EventSystem.current.currentSelectedGameObject);
            if (selectableMenuButton != null)
            {
                selectableMenuButton.SelectButton();
            }
            Canvas.ForceUpdateCanvases();
        }
    }
}
