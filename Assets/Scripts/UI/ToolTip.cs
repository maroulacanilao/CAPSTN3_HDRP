using System;
using CustomEvent;
using CustomHelpers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ToolTip : MonoBehaviour
    {
        [SerializeField] private RectTransform toolTipPanel;
        [SerializeField] private RectTransform canvas;
        [SerializeField] private TextMeshProUGUI toolTipText;
        [SerializeField] private float textPaddingSize = 4f;
        public static readonly Evt<string> OnShowToolTip = new Evt<string>();
        public static readonly Evt OnHideToolTip = new Evt();

        private bool isShowing;
        private Camera cam;
        private void Start()
        {
            cam = gameObject.scene.GetFirstMainCameraInScene(false);
        }
        private void OnEnable()
        {
            isShowing = false;
            HideToolTip();
            OnShowToolTip.AddListener(ShowToolTip);
            OnHideToolTip.AddListener(HideToolTip);
        }

        private void OnDisable()
        {
            OnShowToolTip.RemoveListener(ShowToolTip);
            OnHideToolTip.RemoveListener(HideToolTip);
        }

        private void ShowToolTip(string message_)
        {
            if(string.IsNullOrEmpty(message_)) return;
            if(isShowing && toolTipText.text.GetHashCode() == message_.GetHashCode()) return;
            
            message_ = message_.Replace("\\n", "\n");
            toolTipText.text = message_;

            // var bgSize = toolTipText.GetPreferredValues(message_);
            var _bgSize = new Vector2(toolTipText.preferredWidth + textPaddingSize * 2, toolTipText.preferredHeight + textPaddingSize * 2);
            toolTipPanel.sizeDelta = _bgSize;
            toolTipPanel.gameObject.SetActive(true);
            isShowing = true;
            
            SetPosition();
        }
        
        private void HideToolTip()
        {
            toolTipText.text = "";
            toolTipPanel.gameObject.SetActive(false);
            isShowing = false;
        }

        private void SetPosition()
        {
            if (!isShowing) return;
            if(cam == null) cam = gameObject.scene.GetFirstMainCameraInScene(false);
            
            // RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, null, out var _pos);
            // _pos = /*new Vector2(toolTipPanel.sizeDelta.x / 2, -toolTipPanel.sizeDelta.y / 2) + */(Vector2)_pos;
            // Debug.Log($"Mouse Position: {Input.mousePosition}, Local Position: {_pos}");
            
            var _pos = new Vector2(toolTipPanel.sizeDelta.x / 2, -toolTipPanel.sizeDelta.y / 2) + (Vector2)Input.mousePosition + new Vector2(10, -10);
            //toolTipPanel.transform.localPosition = _pos;
            toolTipPanel.transform.position = _pos;
            toolTipPanel.ClampPositionToScreen();
        }
        
        private void Update()
        {
            if(!isShowing) return;
            SetPosition();
        }
    }
}
