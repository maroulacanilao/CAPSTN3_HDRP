using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabMenu.Codex
{
    public struct CodexInfo
    {
        public string name;
        public string description;
        public string quantity;
        public Sprite sprite;
    }
    
    public class CodexInfoDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText, descriptionText, quantityText;
        [SerializeField] private Image icon;
        
        public void DisplayInfo(CodexInfo codexInfo_)
        {
            nameText.text = codexInfo_.name;
            descriptionText.text = codexInfo_.description;
            quantityText.text = codexInfo_.quantity;
            icon.sprite = codexInfo_.sprite;
            gameObject.SetActive(true);
        }
    }
}
