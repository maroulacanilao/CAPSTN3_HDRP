using Player;
using UnityEngine;

namespace Items.ItemData.Tools
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Tools/WateringCanData", fileName = "New WateringCanData")]
    public class WateringCanData : ToolData
    {
        public int MaxUsages;
        public int CurrentUsage;
        public int level;
        
        public override bool UseTool(PlayerEquipment playerEquipment_)
        {
            // return playerEquipment_.farmTools.WaterTile();
            return true;
        }
        
        private static bool initialized = false;

        private void OnEnable()
        {
            if (!initialized)
            {
                RefreshUsage();
                Debug.Log("CurrentUsage:" + CurrentUsage + " " + "MaxUsages:" + MaxUsages);
                initialized = true;
            }
        }
        
        protected override void OnValidate()
        {
            base.OnValidate();
            ItemType = ItemType.Tool;
        }

        public void ReduceUsage()
        {   
            if(CurrentUsage > 0) CurrentUsage--;
            Debug.Log("CurrentUsage:" + CurrentUsage + " " + "MaxUsages:" + MaxUsages);
        }

        public void RefreshUsage()
        {
            Debug.Log("Refilling");
            CurrentUsage = MaxUsages;
        }
        
    }
}