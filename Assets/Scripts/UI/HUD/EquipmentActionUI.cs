using System.Collections;
using CustomHelpers;
using Player;
using TMPro;
using UnityEngine;

namespace UI.HUD
{
    [DefaultExecutionOrder(1)]
    public class EquipmentActionUI : MonoBehaviour
    {
        [SerializeField] PlayerEquipment playerEquipment;
        [SerializeField] private TextMeshProUGUI actionText;
        [SerializeField] private float tick = 0.2f;
        [SerializeField] private GameObject infoPanel;

        private void Start()
        {
            SetText();
        }
        private void OnEnable()
        {
            StartCoroutine(Tick());
        }
    
        private void OnDisable()
        {
            StopCoroutine(Tick());
        }

        private IEnumerator Tick()
        {
            while (gameObject.activeSelf)
            {
                yield return new WaitForSeconds(tick);
                if(gameObject.IsEmptyOrDestroyed() || gameObject.activeSelf == false) yield break;
                SetText();
            }
        }

        private void SetText()
        {
            string _message = "";
        
            var _action = playerEquipment.GetEquipmentAction();
        
            switch (_action)
            {
                case EquipmentAction.Interact:
                    _message = "Interact";
                    break;
                case EquipmentAction.Till:
                    _message = "Till";
                    break;
                case EquipmentAction.Water:
                    _message = "Water Plant";
                    break;
                case EquipmentAction.Plant:
                    _message = "Plant Seed";
                    break;
                case EquipmentAction.Harvest:
                    _message = "Harvest";
                    break;
                case EquipmentAction.UnTill:
                    _message = "UnTill";
                    break;
                case EquipmentAction.Consume:
                    _message = "Consume Item";
                    break;
                case EquipmentAction.None:
                default:
                    _message = "";
                    infoPanel.SetActive(false);
                    return;
            }

            if (!infoPanel.activeInHierarchy) infoPanel.SetActive(true);
        
            actionText.text = _message;
        }
    }
}
