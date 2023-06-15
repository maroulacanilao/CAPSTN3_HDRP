using System;
using System.Collections.Generic;
using Managers;
using ScriptableObjectData;
using UI.Farming;
using UnityEngine;

namespace UI.TabMenu.Codex
{
    public abstract class CodexMenu : FarmUI
    {
        [SerializeField] protected CodexItem codexItemPrefab;
        [SerializeField] protected Transform contentParent;
        [SerializeField] protected CodexInfoDisplay codexInfoDisplay;
        protected List<CodexItem> codexItems = new List<CodexItem>();
        
        protected GameDataBase dataBase;
        
        public override void Initialize()
        {
            dataBase = GameManager.Instance.GameDataBase;
        }

        protected virtual void OnEnable()
        {
            CodexItem.OnClickEvent.AddListener(ShowCodex);
        }

        protected void OnDisable()
        {
            CodexItem.OnClickEvent.RemoveListener(ShowCodex);
        }

        public override void OpenMenu()
        {
            
        }
        
        public abstract void ShowCodex(int index_);
        
        public void RemoveItems()
        {
            for (int i = codexItems.Count - 1; i >= 0; i--)
            {
                Destroy(codexItems[i].gameObject);
                codexItems.RemoveAt(i);
            }
            codexItems.Clear();
        }
    }
}
