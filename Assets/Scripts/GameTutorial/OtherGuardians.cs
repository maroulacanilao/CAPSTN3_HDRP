using System;
using System.Collections;
using CustomHelpers;
using FungusWrapper;
using NaughtyAttributes;
using UnityEngine;

namespace GameTutorial
{
    public class OtherGuardians : MonoBehaviour
    {
        [SerializeField] private FungusTemporaryCam[] cams;
        [SerializeField] private GameObject guardians;
        [SerializeField] [Tag] private string menuTag;

        public string message;
        
        private GameObject menu;

        private void Awake()
        {
            FungusReceiver.OnReceiveMessage.AddListener(StartCam);
            menu = GameObject.FindGameObjectWithTag(menuTag);
        }

        private void OnDestroy()
        {
            FungusReceiver.OnReceiveMessage.RemoveListener(StartCam);
            if(guardians.IsValid()) guardians.SetActive(true);
        }
        
        private void StartCam(string obj_)
        {
            if(message.ToHash() != obj_.ToHash()) return;
            StartCoroutine(Co_StartCam());
        }
        
        IEnumerator Co_StartCam()
        {
            if(menu.IsValid()) menu.SetActive(false);
            guardians.SetActive(true);
            foreach (var c in cams)
            {
                if(c.IsEmptyOrDestroyed()) continue;

                yield return c.Co_StartCam();
            }
            if(menu.IsValid()) menu.SetActive(true);


            Destroy(gameObject);
        }
    }
}
