using System.Collections.Generic;
using Items.Inventory;
using NaughtyAttributes;
using ScriptableObjectData.CharacterData;
using Trading;
using UnityEngine;

namespace ScriptableObjectData
{
    // [CreateAssetMenu(menuName = "ScriptableObjects/GameDataBase", fileName = "GameDataBase")]
    public class GameDataBase : ScriptableObject
    {
        [field: SerializeField] public ItemDatabase itemDatabase { get; private set; }
        [field: SerializeField] public EnemyDataBase enemyDataBase { get; private set; }
        
        [field: SerializeField] public List<RequestOrderTemplate> requestOrderTemplates { get; private set; }
    
        [field: Header("PlayerFields")]
        [field: SerializeField] public PlayerData playerData { get; private set; }
        [field: SerializeField] public PlayerInventory playerInventory { get; private set; }
        [field: SerializeField] public SeedDataBase seedDataBase { get; private set; }

        private Dictionary<int, EnemyData> enemyDataDictionary = new Dictionary<int, EnemyData>();
    
        public void Initialize()
        {
            enemyDataDictionary = new Dictionary<int, EnemyData>();
            playerData.Initialize(this);
            //seedDataBase.Initialize(playerData.playerLevelData.CurrentLevel);
        }
    
        public void DeInitialize()
        {
            playerData.DeInitialize();
        }
    
        public EnemyData GetEnemyData(int enemyID_)
        {
            enemyDataDictionary.TryGetValue(enemyID_, out var _enemyData);
            return _enemyData;
        }
        
        #if UNITY_EDITOR

        [Button(" Get All Request Template")]
        private void GetAllRequestTemplate()
        {
            requestOrderTemplates = new List<RequestOrderTemplate>();
            var _requestTemplates = Resources.LoadAll<RequestOrderTemplate>("Data/RequestTemplates");
            foreach (var _data in _requestTemplates)
            {
                requestOrderTemplates.Add(_data);
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        #endif
    }
}
