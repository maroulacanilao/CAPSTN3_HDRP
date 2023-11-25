using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace UI.TabMenu.Codex
{
    public struct CodexInfo
    {
        public string name;
        public string description;
        public int quantity;
        public string quantityTxt;
        public Sprite sprite;
        public string errorMsg;
    }
    
    public class CodexInfoDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText, descriptionText, quantityText;
        [SerializeField] private ScrollRect scrollView;
        [SerializeField] private Image icon;

        [SerializeField] private GameObject blocker;
        [SerializeField] private TextMeshProUGUI hiddenDescriptionText;

        public void DisplayInfo(CodexInfo codexInfo_)
        {
            if (nameText != null) nameText.text = codexInfo_.name;
            quantityText.text = codexInfo_.quantityTxt;
            icon.sprite = codexInfo_.sprite;
            DisplayDescription(codexInfo_);
            
            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            scrollView.verticalNormalizedPosition = 1f;
        }
        
        private void DisplayDescription(CodexInfo codexInfo_)
        {
            var _description = codexInfo_.description;
            var _infoQuantity = codexInfo_.quantity;
            
            if (_infoQuantity == 0)
            {
                descriptionText.text = "";

                hiddenDescriptionText.text = "";
                
                return;
            }

            var entries = new List<string>(_description.Split(new string[] { ". " }, StringSplitOptions.RemoveEmptyEntries));
            var sb = new StringBuilder();

            for (int i = 0; i < entries.Count; i++)
            {
                if (i >= _infoQuantity)
                {
                    // Entry is locked, apply blurred effect
                    sb.Append("<color=yellow>"); // Set the text color to a grayish tone
                    sb.Append(codexInfo_.errorMsg.ToUpper());
                    sb.Append("</color>");
                    break;
                }
                sb.Append(entries[i]);
                sb.Append(". ");
            }

            descriptionText.text = sb.ToString();
        }



    }
}
