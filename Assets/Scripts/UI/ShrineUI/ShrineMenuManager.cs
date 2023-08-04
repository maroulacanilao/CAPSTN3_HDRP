using System;
using AYellowpaper.SerializedCollections;
using CustomHelpers;
using FungusWrapper;
using UI.Farming;
using UnityEngine;

namespace UI.ShrineUI
{
    public class ShrineMenuManager : MonoBehaviour
    {
        [SerializedDictionary("message","index")]
        [SerializeField] private SerializedDictionary<string, ShrineMenu> menuDictionary;

        private void Awake()
        {
            foreach (var menu in menuDictionary)
            {
                menu.Value.Initialize();
            }
            FungusReceiver.OnReceiveMessage.AddListener(OnMessageReceived);
        }
        
        private void OnDestroy()
        {
            FungusReceiver.OnReceiveMessage.RemoveListener(OnMessageReceived);
        }
        
        protected void OnEnable()
        {
            CloseAllMenu();
        }

        public void CloseAllMenu()
        {
            foreach (var menu in menuDictionary)
            {
                menu.Value.gameObject.SetActive(false);
            }
        }
        
        protected void OnMessageReceived(string message_)
        {
            if (menuDictionary.TryGetValue(message_, out var _value))
            {
                CloseAllMenu();
                _value.gameObject.SetActive(true);
            }
        }
    }
}
