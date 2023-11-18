using Player;
using UnityEngine;

namespace Items.ItemData.Tools
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Tools/WateringCanData", fileName = "New WateringCanData")]
    public class WateringCanData : ToolData
    {
        public int MaxUsages = 10;
        public int CurrentUsage;
        
        public override bool UseTool(PlayerEquipment playerEquipment_)
        {
            // return playerEquipment_.farmTools.WaterTile();
            return true;
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            CurrentUsage = MaxUsages;
            ItemType = ItemType.Tool;
        }

        public void ReduceUsage()
        {   
            if(CurrentUsage > 0) CurrentUsage--;
        }

        public void RefreshUsage()
        {
            Debug.Log("Refilling");
            CurrentUsage = MaxUsages;
        }
        
    }
}