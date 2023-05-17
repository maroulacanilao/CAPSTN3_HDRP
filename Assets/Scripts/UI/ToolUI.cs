using System;
using Items;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ToolUI : MonoBehaviour
    {
        [SerializeField] private Image toolIcon;
        private void Awake()
        {
            PlayerEquipment.OnChangeItemOnHand.AddListener(ChangeItem);
        }

        private void ChangeItem(Item item_)
        {
            // if (item_ == null)
            // {
            //     toolIcon.sprite = null;
            // }
            toolIcon.sprite = item_?.Data.Icon;
        }
    }
}
