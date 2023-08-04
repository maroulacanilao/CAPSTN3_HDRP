using System.Collections;
using Farming;
using Items;
using Items.ItemData;
using Managers;
using ObjectPool;
using Others.VFX;
using ScriptableObjectData;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

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
            if(string.IsNullOrEmpty(ID_)) return null;
            return AssetDataBase.prefabs.TryGetValue(ID_, out GameObject _prefab) ? _prefab : null;
        }
        
        public static GameObject GetInstanceFromPrefabList(string assetId_)
        {
            if(string.IsNullOrEmpty(assetId_)) return null;
            if(string.IsNullOrWhiteSpace(assetId_)) return null;
            var _prefab = GetPrefab(assetId_);
            if (_prefab == null) return null;
            return _prefab.GetInstance();
        }
        
        public static Sprite GetSelectedButtonSprite()
        {
            return AssetDataBase.selectedButtonSprite;
        }
        
        public static Sprite GetDeselectedButtonSprite()
        {
            return AssetDataBase.deselectedButtonSprite;
        }
        
        public static bool PlayEffect(string assetID_, Vector3 position_, Quaternion rotation_, out GameObject effectObj_)
        {
            effectObj_ = null;
            if(string.IsNullOrEmpty(assetID_)) return false;
            if(string.IsNullOrWhiteSpace(assetID_)) return false;
            var _prefab = GetPrefab(assetID_);
            
            if (_prefab == null) return false;

            effectObj_ = _prefab.GetInstance();
            
            if(effectObj_.IsEmptyOrDestroyed()) return false;
            
            effectObj_.transform.position = position_;
            effectObj_.transform.rotation = rotation_;

            if (effectObj_.TryGetComponent(out ParticleSystem _particleSystem))
            {
                _particleSystem.Play();
                return true;
            }
            if (effectObj_.TryGetComponent(out VisualEffect _visualEffect))
            {
                _visualEffect.Play();
                return true;
            }
            
            var _particleSystemInChild = effectObj_.GetComponentInChildren<ParticleSystem>();
            
            if (_particleSystemInChild.IsValid())
            {
                _particleSystemInChild.Play();
                return true;
            }
            
            var _visualEffectInChild = effectObj_.GetComponentInChildren<VisualEffect>();
            
            if (_visualEffectInChild.IsValid())
            {
                _visualEffectInChild.Play();
                return true;
            }
            
            return false;
        }
        
        public static bool PlayEffect(string assetID_, Vector3 position_ = default, Quaternion rotation_ = default)
        {
            if(string.IsNullOrEmpty(assetID_)) return false;
            var _instance = GetInstanceFromPrefabList(assetID_);
            if(_instance.IsEmptyOrDestroyed()) return false;
            
            _instance.transform.position = position_;
            _instance.transform.rotation = rotation_;
            
            if(!_instance.TryGetComponent(out EffectsPlayer _effectsPlayer)) return false;
            _effectsPlayer.Play();
            return true;
        }
        
        public static IEnumerator Co_PlayEffect(string assetID_, Vector3 position_, Quaternion rotation_, float duration_ = 0)
        {
            var _instance = GetInstanceFromPrefabList(assetID_);
            if(_instance.IsEmptyOrDestroyed()) yield break;
            _instance.transform.position = position_;
            _instance.transform.rotation = rotation_;
            
            if(_instance.TryGetComponent(out EffectsPlayer _effectsPlayer)) yield return _effectsPlayer.Co_Play(duration_);
        }
        
        public static IEnumerator Co_PlayEffect(string assetID_,Transform parent_, Vector3 position_ = default, Quaternion rotation_= default, float duration_ = 0)
        {
            var _instance = GetInstanceFromPrefabList(assetID_);
            if(_instance.IsEmptyOrDestroyed()) yield break;
            _instance.transform.SetParent(parent_);
            _instance.transform.localPosition = position_;
            _instance.transform.localRotation = rotation_;
            
            if(_instance.TryGetComponent(out EffectsPlayer _effectsPlayer)) yield return _effectsPlayer.Co_Play(duration_);
        }
        
        public static bool PlayEffectCoroutine(string assetID_, Vector3 position_, Quaternion rotation_, float duration_ = 0)
        {
            var _instance = GetInstanceFromPrefabList(assetID_);
            if(_instance.IsEmptyOrDestroyed()) return false;
            _instance.transform.position = position_;
            _instance.transform.rotation = rotation_;
            
            if(!_instance.TryGetComponent(out EffectsPlayer _effectsPlayer)) return false;
            _effectsPlayer.Co_Play(duration_).StartCoroutine();
            return true;
        }

        public static bool PlayEffectCoroutine(string assetID_, Transform parent_, Vector3 position_ = default, Quaternion rotation_ = default, float duration_ = 0)
        {
            var _instance = GetInstanceFromPrefabList(assetID_);
            if(_instance.IsEmptyOrDestroyed()) return false;
            _instance.transform.SetParent(parent_);
            _instance.transform.localPosition = position_;
            _instance.transform.localRotation =  rotation_;

            Debug.Log(_instance.transform.parent.name);
            
            if(!_instance.TryGetComponent(out EffectsPlayer _effectsPlayer)) return false;
            _effectsPlayer.Co_Play(duration_).StartCoroutine();
            return true;
        }

        public static void PlayHitEffect(Vector3 position_, Quaternion rotation_, float duration_ = 0)
        {
            PlayEffectCoroutine("Hit", position_, rotation_, duration_);
            AudioManager.PlayHit();
        }
    }
}
