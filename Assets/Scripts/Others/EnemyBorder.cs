using System;
using System.Collections;
using System.Collections.Generic;
using EnemyController;
using NaughtyAttributes;
using UnityEngine;

public class EnemyBorder : MonoBehaviour
{
    [SerializeField] [Tag] private string enemyTag;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(!other.CompareTag(enemyTag)) return;
        Debug.Log("Enemy Border Triggered");
        
        var _enemy = other.GetComponent<EnemyAIController>();
        var _station = _enemy.station;
        _enemy.gameObject.SetActive(false);
        _enemy.transform.position = _station.transform.position;
        _enemy.gameObject.SetActive(true);
    }
}
