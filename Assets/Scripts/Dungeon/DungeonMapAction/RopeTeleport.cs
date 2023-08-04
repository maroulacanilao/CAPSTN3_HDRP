using System;
using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeon.DungeonMapAction
{
    public class RopeTeleport : DungeonAction
    {
        [SerializeField] private Transform teleportPoint;

        private GameObject player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        protected override void RemoveBlockerHandler()
        {
            player.transform.position = teleportPoint.position;
        }
    }
}
