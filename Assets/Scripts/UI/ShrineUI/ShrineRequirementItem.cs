using Items.ItemData;
using Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShrineUI
{
    public class ShrineRequirementItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameTxt, countTxt;
    
        public void Set(OfferRequirement offerRequirement_)
        {
            var _data = offerRequirement_.consumableData;
            var _count = offerRequirement_.count;
            icon.sprite = _data.Icon;
            nameTxt.text = _data.ItemName;
            countTxt.text = $"x{_count}";
        }
        
        public void SetNull()
        {
            icon.sprite = null;
            nameTxt.text = "";
            countTxt.text = "";
        }
    }
}
