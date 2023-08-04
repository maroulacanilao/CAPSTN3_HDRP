using System;
using System.Collections;
using Autotiles3D;
using BaseCore;
using CustomHelpers;
using Fungus;
using FungusWrapper;
using Managers;
using UnityEngine;

namespace UI.Battle
{
    public class BattleTextManager : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Writer writer;
        private bool flag = false;
        
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
            if(!GameManager.IsBattleSceneActive()) yield break;
            if(Instance == null) yield break;
            if(Instance.writer == null) yield break;
            if(Instance.panel == null) yield break;
            
            Instance.flag = false;
            Instance.StartCoroutine(Instance.DoWriteWrapper(text_));
            Instance.StartCoroutine(Instance.Co_FallBack());
            
            yield return new WaitUntil(() => Instance.flag == true);
            Instance.writer.Stop();
            Instance.StopAllCoroutines();
        }
        
        private IEnumerator DoWriteWrapper(string text_)
        {
            flag = false;
            panel.SetActive(true);
            if(!GameManager.IsBattleSceneActive()) yield break;
            yield return writer.Write(text_, true, true, false, false, null, null);
            panel.SetActive(false);
            flag = true;
        }
        
        IEnumerator Co_DoWrite(string text_)
        {
            yield return writer.Write(text_, true, true, false, false, null, null);
        }

        IEnumerator Co_FallBack()
        {
            yield return new WaitForSecondsRealtime(7.5f);
            flag = true;
        }

        public static void Stop()
        {
            if(!GameManager.IsBattleSceneActive()) return;
            if(Instance == null) return;
            if(Instance.writer == null) return;
            if(Instance.panel == null) return;
            
            Instance.writer.Stop();
            Instance.StopAllCoroutines();
            Instance.flag = false;
            Instance.panel.SetActive(false);
        }
    }
}
