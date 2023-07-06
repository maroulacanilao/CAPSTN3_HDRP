using Farming;
using Items;
using Items.ItemData;
using ScriptableObjectData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomHelpers
{
    public static class AssetHelper
    {
        private static GameDataBase GameDataBase;
        private static ItemDatabase ItemDatabase;
        private static AssetDataBase AssetDataBase;
        private static AudioDataBase AudioDataBase;
        private static StatsDataBase StatsDataBase;
        private static bool hasInitialized;

        public static void Initialize(GameDataBase gameDataBase_)
        {
            GameDataBase = gameDataBase_;
            ItemDatabase = GameDataBase.itemDatabase;
            AssetDataBase = GameDataBase.assetDataBase;
            AudioDataBase = GameDataBase.audioDataBase;
            StatsDataBase = GameDataBase.statsDataBase;
        }
        
        public static string GetColoredText(this RarityType rarityType_)
        {
            string _rarityText = "";
            switch (rarityType_)
            {
                case RarityType.Uncommon:
                    _rarityText = ("Uncommon").SurroundWithColor(ItemDatabase.RarityColorDictionary[RarityType.Uncommon]);
                    break;
                case RarityType.Rare:
                    _rarityText = ("Rare").SurroundWithColor(ItemDatabase.RarityColorDictionary[RarityType.Rare]);
                    break;
                case RarityType.Epic:
                    _rarityText = ("Epic").SurroundWithColor(ItemDatabase.RarityColorDictionary[RarityType.Epic]);
                    break;
                case RarityType.Common:
                default:
                    _rarityText = ("Common").SurroundWithColor(ItemDatabase.RarityColorDictionary[RarityType.Common]);
                    break;
            }
        
            return _rarityText;
        }
    
        public static StatType GetStatType(this ConsumableData data_)
        {
            return StatsDataBase.GetStatTypeByConsumable(data_);
        }

        public static Sprite GetSpriteIcon(this StatType statType_)
        {
            return StatsDataBase.statSprites[statType_];
        }

        public static Sprite GetSpriteIcon(this ItemType itemType_)
        {
            return AssetDataBase.itemTypeIcons[itemType_];
        }

        public static Mesh GetTileMesh(this TileState tileState_)
        {
            return AssetDataBase.tileStateMesh[tileState_];
        }
        
        public static Material GetTileMaterial(bool isWatered_)
        {
            return AssetDataBase.tileMaterial[isWatered_];
        }
        
        public static AudioClip GetMusicForScene(string sceneName_)
        {
            return AudioDataBase.GetSceneMusic(sceneName_);
        }

        public static AudioClip GetMusicForScene(this Scene scene_)
        {
            return GetMusicForScene(scene_.name);
        }
        
        public static AudioClip GetMusicForActiveScene()
        {
            return SceneManager.GetActiveScene().GetMusicForScene();
        }

        public static AudioClip GetSfx(string ID_)
        {
            return AudioDataBase.GetSfx(ID_);
        }
        
        public static GameObject GetPrefab(string ID_)
        {
            return AssetDataBase.prefabs.TryGetValue(ID_, out GameObject _prefab) ? _prefab : null;
        }
    }
}
