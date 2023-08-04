using System;
using System.Collections;
using Cinemachine;
using CustomHelpers;
using FungusWrapper;
using NaughtyAttributes;
using UnityEngine;

namespace GameTutorial
{
    public class FungusTemporaryCam : MonoBehaviour
    {
        public GameObject targetCam;
        public string message;
        public float duration = 5f;
        public bool disableMenu = true;
        [Tag][ShowIf("disableMenu")] public string menuTag;
        public bool AutoDestroy = true;
    
        CinemachineVirtualCamera cineCam {get ; set;}
        private GameObject menu;

        private void Awake()
        {
            cineCam = targetCam.GetComponent<CinemachineVirtualCamera>();
            menu = GameObject.FindGameObjectWithTag(menuTag);
        }
        
        private void OnEnable()
        {
            FungusReceiver.OnReceiveMessage.AddListener(StartCam);
        }

        private void OnDisable()
        {
            FungusReceiver.OnReceiveMessage.RemoveListener(StartCam);
        }
    
        private void StartCam(string msg_)
        {
            if(message.ToHash() != msg_.ToHash()) return;
            StartCoroutine(Co_StartCam());
        }
    
        public IEnumerator Co_StartCam()
        {
            yield return null;
            
            if(disableMenu && menu.IsValid()) menu.SetActive(false);
            
            targetCam.SetActive(true);
            cineCam.Priority = 20;

            yield return new WaitForSecondsRealtime(duration);
            cineCam.Priority = 0;
            targetCam.SetActive(false);
            
            if(disableMenu && menu.IsValid()) menu.SetActive(true);
            
            if(AutoDestroy) Destroy(gameObject);
        }
    }
}
