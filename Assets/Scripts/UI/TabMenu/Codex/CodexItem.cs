using System;
using CustomEvent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabMenu.Codex
{
    public class CodexItem : SelectableMenuButton
    {
        [SerializeField] private TextMeshProUGUI nameText;
        
        public static readonly Evt<int> OnClickEvent = new Evt<int>();
        private int index;
        
        public CodexItem Initialize(int index, string name)
        {
            this.index = index;
            nameText.text = name;
            button.onClick.AddListener(OnClick);

            return this;
        }

        public void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        private void OnClick() => OnClickEvent.Invoke(index);
    }
}
