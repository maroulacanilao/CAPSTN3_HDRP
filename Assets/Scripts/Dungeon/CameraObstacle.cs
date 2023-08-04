using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Dungeon
{
    public class CameraObstacle : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] meshRenderers;
        [SerializeField] private Material invisibleMaterial;

        private Dictionary<MeshRenderer,Material> rendererMaterialDictionary;
        private void Awake()
        {
            rendererMaterialDictionary = new Dictionary<MeshRenderer, Material>();
            foreach (var mesh in meshRenderers)
            {
                rendererMaterialDictionary.Add(mesh,mesh.material);
            }
        }
        
        private void Reset()
        {
            GetAllMeshRenderers();
        }

        public void SetMaterial(Material material_)
        {
            foreach (var _meshRenderer in meshRenderers)
            {
                _meshRenderer.material = material_;
            }
        }
        
        public void SetMaterial(Material[] materials_)
        {
            for (var _i = 0; _i < meshRenderers.Length; _i++)
            {
                meshRenderers[_i].material = materials_[_i];
            }
        }
        
        public void InvisibleMaterial()
        {
            SetMaterial(invisibleMaterial);
        }
        
        public void ResetMaterial()
        {
            foreach (var _meshRenderer in meshRenderers)
            {
                _meshRenderer.material = rendererMaterialDictionary[_meshRenderer];
            }
        }

        public void EnableRenderer(bool enable_)
        {
            foreach (var _meshRenderer in meshRenderers)
            {
                _meshRenderer.enabled = enable_;
            }
        }
        
        [Button("Get ALl Mesh Renderers")]
        public void GetAllMeshRenderers()
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
        }
    }
}
