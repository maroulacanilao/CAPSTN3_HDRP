using System.Collections;
using CustomHelpers;
using Player;
using Player.ControllerState;
using TMPro;
using UnityEngine;

namespace UI.HUD
{
    [DefaultExecutionOrder(1)]
    public class EquipmentActionUI : MonoBehaviour
    {
        [SerializeField] private GameObject actionPanel, interactPanel;
        [SerializeField] private TextMeshProUGUI actionText, interactText;
        [SerializeField] private float tick = 0.1f;

        private PlayerInputController controller;
        
        private PlayerEquipment playerEquipment;
        private InteractDetector interactDetector;

        private bool canUseTool = true;
        private void OnEnable()
        {
            controller = PlayerInputController.Instance;
            playerEquipment = controller.playerEquipment;
            interactDetector = controller.interactDetector;
            SetToolText();
            StartCoroutine(Tick());
        }
    
        private void OnDisable()
        {
            StopCoroutine(Tick());
        }

        private IEnumerator Tick()
        {
            while (gameObject.IsValid() && gameObject.activeSelf)
            {
                if(controller.CanUseFarmTools) SetToolText();
                else SetAttackText();
                SetInteractText();
                yield return new WaitForSeconds(tick);
            }
        }

        private void SetToolText()
        {
            if (!canUseTool)
            {
                return;
            }
            
            string _message = "";
        
            var _action = playerEquipment.currAction;
        
            switch (_action)
            {
                case EquipmentAction.Till:
                    _message = "<color=orange>Plough field </color>";
                    break;
                case EquipmentAction.Water:
                    _message = "<color=orange>Water Plant</color>";
                    break;
                case EquipmentAction.Refill:
                    _message = "<color=orange>Refill</color>";
                    break;
                case EquipmentAction.Plant:
                    _message = $"<color=orange>Plant {playerEquipment.seedName} </color>";
                    break;
                case EquipmentAction.Harvest:
                    _message = "<color=orange>Harvest</color>";
                    break;
                case EquipmentAction.UnTill:
                    _message = "<color=red>Level field</color>";
                    break;
                case EquipmentAction.Consume:
                    _message = "<color=yellow>Consume</color>";
                    break;
                case EquipmentAction.Fish:
                    _message = "<color=orange>Fish</color>";
                    break;
                case EquipmentAction.None:
                default:
                    _message = "";
                    actionPanel.SetActive(false);
                    return;
            }

            if (!actionPanel.activeSelf) actionPanel.SetActive(true);
        
            actionText.text = _message;
        }

        private void SetAttackText()
        {
            if (controller.playerState != PlayerSate.Grounded)
            {
                if (actionPanel.activeSelf) actionPanel.SetActive(false);
                return;
            }
            else
            {
                if (!actionPanel.activeSelf) actionPanel.SetActive(true);
                actionText.text = "<color=orange>Attack</color>";
            }
        }

        private void SetInteractText()
        {
            if (!interactDetector.CanInteract())
            {
                interactPanel.SetActive(false);
            }
            else
            {
                interactPanel.SetActive(true);
                
                var _text = interactDetector.interactText;
                interactText.text = string.IsNullOrEmpty(_text) ? "Interact" : _text;
            }
        }
        
        public void SetCanUseTool(bool canUseTool_)
        {
            canUseTool = canUseTool_;
            actionPanel.SetActive(canUseTool);
        }
    }
}
