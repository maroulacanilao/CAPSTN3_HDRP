using Player;
using UnityEngine;

namespace Items.ItemData.Tools
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Tools/WateringCanData", fileName = "New WateringCanData")]
    public class WateringCanData : ToolData
    {
        public override void UseTool(PlayerEquipment playerEquipment_)
        {
            playerEquipment_.farmTools.WaterTile();
        }
    }
}