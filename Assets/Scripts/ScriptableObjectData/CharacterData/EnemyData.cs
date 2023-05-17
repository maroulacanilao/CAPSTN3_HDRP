using System;
using BaseCore;
using BattleSystem;
using NaughtyAttributes;
using ObjectPool;
using UnityEngine;

namespace ScriptableObjectData.CharacterData
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/CharacterData/EnemyData", order = 0)]
    public class EnemyData : CharacterData
    {
        [field: SerializeField] [field: BoxGroup("LootTable")]  public LootTable LootTable { get; private set; }

        public EnemyBattleCharacter SpawnBattleEnemy(int level_)
        {
            var _enemy = battlePrefab.GetInstance();
            return _enemy.GetComponent<EnemyBattleCharacter>().Initialize(this, level_) as EnemyBattleCharacter;
        }

        #if UNITY_EDITOR
        
        // [Button("Create Other Data")]
        // public void CreateAssetWithDependencies()
        // {
        //     string assetPath = AssetDatabase.GetAssetPath(this);
        //     
        //     if (!string.IsNullOrEmpty(assetPath))
        //     {
        //         int resourceIndex = assetPath.IndexOf("Resources/", StringComparison.Ordinal);
        //         if (resourceIndex >= 0)
        //         {
        //             string resourcePath = assetPath.Substring(resourceIndex + 10); // Add 10 to exclude "Resources/" from the path
        //             resourcePath = resourcePath.Substring(0, resourcePath.LastIndexOf('/')); // Remove the file name and extension
        //             Debug.Log("Resource Path: " + resourcePath);
        //         }
        //         else
        //         {
        //             Debug.Log("ScriptableObject is not in the Resources folder.");
        //         }
        //     }
        //     else
        //     {
        //         Debug.Log("ScriptableObject is not an asset in the project.");
        //     }
        // }
        
        #endif
    }
}
