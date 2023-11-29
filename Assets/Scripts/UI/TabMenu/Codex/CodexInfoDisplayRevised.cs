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
    public struct CodexInfoRevised
    {
        public string name;
        public string description;
        public int quantity;
        public string quantityTxt;
        public Sprite sprite;
        public string errorMsg;

        public int quantityNeededCount;
    }

    public class CodexInfoDisplayRevised : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText, descriptionText, quantityText;
        [SerializeField] private ScrollRect scrollView;
        [SerializeField] private Image icon;

        [SerializeField] private GameObject blocker;
        [SerializeField] private TextMeshProUGUI basicDescriptionText;
        [SerializeField] private TextMeshProUGUI hiddenDescriptionText;

        public void DisplayInfo(CodexInfoRevised codexInfo_)
        {
            if (nameText != null) nameText.text = codexInfo_.name;
            quantityText.text = codexInfo_.quantityTxt;

            icon.sprite = codexInfo_.sprite;
            if (icon.sprite == null)
            {
                icon.gameObject.SetActive(false);
            }
            else
            {
                icon.gameObject.SetActive(true);
            }

            DisplayDescriptionRevised(codexInfo_);

            gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
            if (scrollView != null) scrollView.verticalNormalizedPosition = 1f;
        }
        
        private void DisplayDescriptionRevised(CodexInfoRevised codexInfo_)
        {
            var _infoQuantity = codexInfo_.quantity;

            if (_infoQuantity == 0)
            {
                basicDescriptionText.text = "";

                hiddenDescriptionText.text = "";

                return;
            }

            var _description = codexInfo_.description;
            var entries = new List<string>(_description.Split(". ", 3));
            
            var sb = new StringBuilder();
            sb.Append(entries[0]);
            sb.Append(". ");
            sb.Append(entries[1]);
            sb.Append(". ");

            basicDescriptionText.text = sb.ToString();


            var sbHidden = new StringBuilder();
            for (int i = 2; i < entries.Count; i++)
            {
                sbHidden.Append(entries[i]);
                sbHidden.Append(". ");
            }

            hiddenDescriptionText.text = sbHidden.ToString();

            var _quantityNeededCount = codexInfo_.quantityNeededCount;
            if (_quantityNeededCount > _infoQuantity)
            {
                blocker.SetActive(true);
            }
            else
            {
                blocker.SetActive(false);
            }
        }
    }
}

