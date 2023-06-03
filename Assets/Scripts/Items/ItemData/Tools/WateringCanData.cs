using Player;
using UnityEngine;

namespace Items.ItemData.Tools
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Tools/WateringCanData", fileName = "New WateringCanData")]
    public class WateringCanData : ToolData
    {
        public override bool UseTool(PlayerEquipment playerEquipment_)
        {
            return playerEquipment_.farmTools.WaterTile();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            ItemType = ItemType.Tool;
        }
    }
}