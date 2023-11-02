using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using BaseCore;
using CustomEvent;
using CustomHelpers;
using Items;
using Items.ItemData;
using Managers;
using NaughtyAttributes;
using ObjectPool;
using UI.Farming;
using UnityEngine;


namespace Farming
{
    public class FishingTile : MonoBehaviour
    {
        public WaterTileState waterTileState { get; private set; }

        public TileState tileState => currentState?.tileState ?? TileState.Water;

        [SerializeReference] private FarmTileState currentState;

        private void Start()
        {
            currentState = waterTileState;
        }
    }
}
