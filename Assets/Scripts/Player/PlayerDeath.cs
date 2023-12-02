using System;
using System.Collections;
using BaseCore;
using BattleSystem;
using Character.CharacterComponents;
using CustomHelpers;
using Managers;
using ScriptableObjectData.CharacterData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private string messageToDisplay = "OnDeath";
        [SerializeField] private GameObject canvas;

        private void OnEnable()
        {
            playerData.health.OnDeath.AddListener(OnDeath);
        }

        private void OnDisable()
        {
            playerData.health.OnDeath.RemoveListener(OnDeath);
        }
        
        private void OnDeath(CharacterHealth arg1_, DamageInfo arg2_)
        {
            if(!isActiveAndEnabled) return;
            if (SceneManager.GetActiveScene().name != "SampleDungeonScene") return;
            if(canvas != null) canvas.SetActive(true);
            
            // Debug.Log($"Current Scene: {SceneManager.GetActiveScene().name}");
            // Debug.Log($"IsDungeonSceneActive: {GameManager.IsDungeonSceneActive()}");
            //if(!GameManager.IsDungeonSceneActive()) return;
            
        }

        public void NextScene()
        {
            GameManager.OnExitBattle.Invoke(BattleResultType.Lose);
        }

        private IEnumerator SendMessage()
        {
            yield return new WaitForSeconds(0.1f);
            if(this.IsEmptyOrDestroyed()) yield break;
            Fungus.Flowchart.BroadcastFungusMessage(messageToDisplay);
        }
    }
}
