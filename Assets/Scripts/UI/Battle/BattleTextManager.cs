using System;
using System.Collections;
using BaseCore;
using Fungus;
using UnityEngine;

namespace UI.Battle
{
    public class BattleTextManager : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Writer writer;
        
        private static BattleTextManager Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
        
        public static IEnumerator DoWrite(string text_)
        {
            if(Instance == null) yield break;
            if(Instance.writer == null) yield break;
            if(Instance.panel == null) yield break;
            
            yield return Instance.DoWriteWrapper(text_);
        }
        
        private IEnumerator DoWriteWrapper(string text_)
        {
            panel.SetActive(true);
            yield return writer.Write(text_, true, true, false, false, null, null);
            panel.SetActive(false);
        }
    }
}
