using Player;
using UnityEngine;

namespace Items.ItemData.Tools
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Tools/HoeData", fileName = "New HoeData")]
    public class HoeData : ToolData
    {
        public override bool UseTool(PlayerEquipment playerEquipment_)
        {
            return playerEquipment_.farmTools.TillTile();
        }
    }
}
