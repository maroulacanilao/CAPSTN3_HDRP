using System;
using System.Linq;
using CustomHelpers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InventoryMenu
{
    public class InventoryActionButtonGroup : MonoBehaviour
    {
        [SerializeField] private SelectableMenuButton[] buttons;

        private bool HasValidButton() => buttons.Any(b => b.gameObject.activeSelf);
        
        private SelectableMenuButton GetValidButton() => buttons.FirstOrDefault(b => b.gameObject.activeSelf);

        public void Initialize()
        {
            Item_MenuItem.OnItemClick.AddListener(OnItemClick);
        }

        private void OnDestroy()
        {
            Item_MenuItem.OnItemClick.RemoveListener(OnItemClick);
        }

        private void OnItemClick()
        {
            if(!HasValidButton()) return;
            var _menuButton = GetValidButton();
            
            if(_menuButton.IsEmptyOrDestroyed()) return;
            
            _menuButton.button.Select();
        }

    }
}
