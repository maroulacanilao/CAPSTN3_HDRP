using Player;
using UnityEngine;

namespace Items.ItemData.Tools
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Tools/FishingPoleData", fileName = "New FishingPoleData")]
    public class FishingPoleData : ToolData
    {
        public override bool UseTool(PlayerEquipment playerEquipment_)
        {
            return true;
        }
    }
}
