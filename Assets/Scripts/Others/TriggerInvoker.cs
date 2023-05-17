using System;
using System.Collections.Generic;
using System.Linq;
using CustomEvent;
using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

public class TriggerInvoker : MonoBehaviour
{
    [SerializeField] private bool checkTag = true;
    [Tag] [ShowIf("checkTag")]
    [SerializeField] private string objectTag;

    [SerializeField] private bool checkLayer = true;
    [ShowIf("checkLayer")]
    [SerializeField] private LayerMask layersToTrigger = default;

    private List<Collider> colliderList;

    public readonly Evt<GameObject> OnEnter = new Evt<GameObject>();
    public readonly Evt<GameObject> OnExit = new Evt<GameObject>();

    /// <summary>
    /// For when the collider gets disabled
    /// </summary>
    public readonly Evt<GameObject> OnForceExit = new Evt<GameObject>();

    private void Awake()
    {
    }

    private void OnEnable()
    {
        colliderList = new List<Collider>();
    }

    private void OnDisable()
    {
        foreach (var col in colliderList.Where(col => !col.IsDestroyed()))
        {
            OnForceExit.Invoke(col.gameObject);
        }
        colliderList.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (checkTag && !other.CompareTagHash(objectTag)) return;
        if (checkLayer && !other.CompareLayer(layersToTrigger)) return;
        if (!colliderList.AddUnique(other)) return;
        
        OnEnter.Invoke(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        // Early returns
        if (checkTag && !other.CompareTagHash(objectTag)) return;
        if (checkLayer && !other.CompareLayer(layersToTrigger)) return;
        if (!colliderList.Remove(other)) return;

        OnExit.Invoke(other.gameObject);
    }
}