using System;
using CustomHelpers;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace Others
{
    public class FarmFoliage : MonoBehaviour
    {
        [SerializeField]
        private GameObject model;

        private void Reset()
        {
            var _filer = GetComponentInChildren<MeshFilter>();
            if(_filer.IsValid()) model = _filer.gameObject;
        }

        private void Awake()
        {
            var _filer = GetComponentInChildren<MeshFilter>();
            if(_filer.IsValid()) model = _filer.gameObject;
        }

        public void SetModelActive(bool active_)
        {
            if(model == null) return;
            
            model.SetActive(active_);
        }

        [Button("AddParent")]
        public void AddParent()
        {
            var _oldParent = transform.parent;
            var _newParent = new GameObject(name + " parent");
            _newParent.transform.position = transform.position;
            _newParent.transform.SetParent(_oldParent);
            
            transform.SetParent(_newParent.transform);
        }
    }
}
