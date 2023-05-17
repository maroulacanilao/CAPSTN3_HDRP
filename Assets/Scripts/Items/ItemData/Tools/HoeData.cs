using Player;
using UnityEngine;

namespace Items.ItemData.Tools
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/Tools/HoeData", fileName = "New HoeData")]
    public class HoeData : ToolData
    {
        public override void UseTool(PlayerEquipment playerEquipment_)
        {
            playerEquipment_.farmTools.TillTile();
        }
    }
}
