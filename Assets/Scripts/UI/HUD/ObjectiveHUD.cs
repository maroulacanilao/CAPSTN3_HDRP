using System;
using System.Text;
using CustomEvent;
using CustomHelpers;
using FungusWrapper;
using ScriptableObjectData;
using UnityEngine;

namespace UI.HUD
{
    public class ObjectiveHUD : MonoBehaviour
    {
        [SerializeField] private ObjectiveData objectiveData;
        [SerializeField] private GameObject panel;
        [SerializeField] private TMPro.TextMeshProUGUI objectiveText;
        [SerializeField] private string clearKey;
        
        public static readonly Evt<string> OnSendMessage = new Evt<string>();
        public static readonly Evt<string> OnUpdateObjectiveText = new Evt<string>();
        public static readonly Evt OnClearObjectiveText = new Evt();

        private void Awake()
        {
            panel.gameObject.SetActive(false);
            OnUpdateObjectiveText.AddListener(UpdateObjectiveText);
            OnSendMessage.AddListener(OnReceiveMessage);
            FungusReceiver.OnReceiveMessage.AddListener(OnReceiveMessage);
        }

        private void OnDestroy()
        {
            OnUpdateObjectiveText.RemoveListener(UpdateObjectiveText);
            OnSendMessage.RemoveListener(OnReceiveMessage);
            FungusReceiver.OnReceiveMessage.RemoveListener(OnReceiveMessage);
        }
        
        private void OnReceiveMessage(string key_)
        {
            if (key_.ToHash() == clearKey.ToHash())
            {
                ClearObjectiveText();
                return;
            }
            
            if(objectiveData.TryGetObjectiveText(key_, out var _text)) UpdateObjectiveText(_text);
        }

        private void UpdateObjectiveText(string txt_)
        {
            var _sb = new StringBuilder();
            _sb.Append("\u2022 ");
            _sb.Append(txt_);
            objectiveText.text = _sb.ToString();
            panel.SetActive(true);
        }

        private void ClearObjectiveText()
        {
            objectiveText.text = "";
            panel.SetActive(false);
        }
    }
}
