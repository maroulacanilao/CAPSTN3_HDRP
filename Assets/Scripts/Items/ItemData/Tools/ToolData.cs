using Player;
using UnityEngine;

namespace Items.ItemData.Tools
{
    public abstract class ToolData : ItemData
    {
        private void Reset()
        {
            itemType = ItemType.Tool;
        }

        private void OnValidate()
        {
            itemType = ItemType.Tool;
        }
        
        public abstract void UseTool(PlayerEquipment playerEquipment_);
        
        public ItemTool GetItem()
        {
            return new ItemTool(this);
        }
    }
}
