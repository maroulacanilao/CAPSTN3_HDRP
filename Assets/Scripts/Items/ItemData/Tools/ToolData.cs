using Player;
using UnityEngine;

namespace Items.ItemData.Tools
{
    public abstract class ToolData : ItemData
    {
        private void Reset()
        {
            ItemType = ItemType.Tool;
        }

        protected override void OnValidate()
        {
            ItemType = ItemType.Tool;
        }
        
        public abstract void UseTool(PlayerEquipment playerEquipment_);
        
        public ItemTool GetItem()
        {
            return new ItemTool(this);
        }
    }
}
