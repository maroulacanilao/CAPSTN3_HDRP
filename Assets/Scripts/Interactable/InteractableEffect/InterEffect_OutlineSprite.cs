using System;
using UnityEngine;

namespace Interactable.InteractableEffect
{
    public class InterEffect_OutlineSprite : MonoBehaviour, IInteractableEffect
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Material outlineMaterialPrefab;
        [SerializeField] private Color outlineColor = Color.white;
        [SerializeField] private float outlineWidth = 1f;
        
        private Material defaultMaterial;
        private Material outlineMaterial;

        private void Reset()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        
        private void Awake()
        {
            if(spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            defaultMaterial = spriteRenderer.material;
            
            if(outlineMaterialPrefab == null) return;
            outlineMaterial = Instantiate(outlineMaterialPrefab);
            outlineMaterial.SetColor("_OutlineColor", outlineColor);
            outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);
        }
        
        public void OnInteract()
        {
            
        }
        
        public void OnEnter()
        {
            spriteRenderer.material = GetOutlineMaterial();
        }
        
        public void OnExit()
        {
            spriteRenderer.material = defaultMaterial;
        }
        
        public Material GetOutlineMaterial()
        {
            return outlineMaterial != null ? outlineMaterial : defaultMaterial;
        }
    }
}
