using System;
using System.Collections;
using System.Text;
using CustomEvent;
using CustomHelpers;
using DG.Tweening;
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

        private Vector3 originalPos;

        [SerializeField] private GameObject ObjectiveButton;

        private void Awake()
        {
            originalPos = panel.transform.localPosition;

            panel.gameObject.SetActive(false);
            ObjectiveButton.gameObject.SetActive(false);
            OnUpdateObjectiveText.AddListener(UpdateObjectiveText);
            OnSendMessage.AddListener(OnReceiveMessage);
            FungusReceiver.OnReceiveMessage.AddListener(OnReceiveMessage);

            GameHUDButtons.OnObjButtonClicked.AddListener(PlayObjectivePanelAnimation);
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

            ObjectiveButton.gameObject.SetActive(true);
            PlayObjectivePanelAnimation();
        }

        private void ClearObjectiveText()
        {
            objectiveText.text = "";
            panel.SetActive(false);
        }

        private void PlayObjectivePanelAnimation()
        {
            panel.transform.localPosition = new Vector3(500f, originalPos.y);

            var sequence = DOTween.Sequence();
            sequence.Append(panel.transform.DOLocalMoveX(originalPos.x, 1f));
            sequence.AppendInterval(5f);
            sequence.Append(panel.transform.DOLocalMoveX(500f, 1f));

        }
    }
}
