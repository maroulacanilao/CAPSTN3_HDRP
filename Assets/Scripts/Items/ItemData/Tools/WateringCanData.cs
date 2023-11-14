using Player;
using UnityEngine;

namespace Items.ItemData.Tools
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Tools/WateringCanData", fileName = "New WateringCanData")]
    public class WateringCanData : ToolData
    {
        public int usages = 3;
        
        public override bool UseTool(PlayerEquipment playerEquipment_)
        {
            // return playerEquipment_.farmTools.WaterTile();
            return true;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            ItemType = ItemType.Tool;
        }

        public void ReduceUsage()
        {   
            if(usages > 0) usages--;
        }
        
    }
}