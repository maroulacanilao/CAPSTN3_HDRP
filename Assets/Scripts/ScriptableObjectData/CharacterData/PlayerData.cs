using System.Collections.Generic;
using BaseCore;
using Items.Inventory;
using UnityEngine;

namespace ScriptableObjectData.CharacterData
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/CharacterData/PlayerData", order = 0)]
    public class PlayerData : CharacterData
    {
        [field: SerializeField] public PlayerInventory playerInventory { get; private set; }
        [field: SerializeField] public PlayerLevel playerLevelData { get; private set; }

        [field: SerializeField] public AllyData AllyData { get; private set; }


        public void SetAlly(AllyData allyData_)
        {
            AllyData = allyData_;
        }
    }
}