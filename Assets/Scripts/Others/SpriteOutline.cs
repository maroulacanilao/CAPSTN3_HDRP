using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOutline : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField] private float outlineWidth = 1f;
    
    private SpriteRenderer spriteRenderer;
    private Material defaultMaterial;
    private static readonly int OutlineThicknessID = Shader.PropertyToID("_OutlineThickness");
    private static readonly int OutlineColorID = Shader.PropertyToID("_OutlineColor");
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        enabled = false;
    }

    private void OnEnable()
    {
        spriteRenderer.material = outlineMaterial;
        spriteRenderer.material.SetColor(OutlineColorID, outlineColor);
        spriteRenderer.material.SetFloat(OutlineThicknessID, outlineWidth);
    }

    private void OnDisable()
    {
        spriteRenderer.material = defaultMaterial;  
    }
}
