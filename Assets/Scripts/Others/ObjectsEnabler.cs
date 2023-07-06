using System.Collections.Generic;
using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

namespace Others
{
    public class ObjectsEnabler : MonoBehaviour
    {
        [SerializeField] List<GameObject> objectsToEnable = new List<GameObject>();
        [SerializeField] private bool willOperateOnEnable = true;
        [SerializeField] [ShowIf("willOperateOnEnable")] private bool enableOnEnable = true;
        
        [SerializeField] private bool willOperateOnDisable = false;
        [SerializeField] [ShowIf("willOperateOnDisable")]private bool enableOnDisbale = false;
        
        private void OnEnable()
        {
            if(!willOperateOnEnable) return;
            DisableObjects(enableOnEnable);
        }
        
        private void OnDisable()
        {
            if (!willOperateOnDisable) return;
            DisableObjects(enableOnDisbale);
        }


        private void DisableObjects(bool willDisable)
        {
            foreach (var _obj in objectsToEnable)
            {
                if(_obj.IsEmptyOrDestroyed()) continue;
                
                _obj.SetActive(willDisable);
            }
        }
    }
}
