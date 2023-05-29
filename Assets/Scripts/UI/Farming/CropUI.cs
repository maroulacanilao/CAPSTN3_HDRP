using System;
using Farming;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Farming
{
    public class CropUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI cropNameText, timeTxt;
        [SerializeField] private Image icon;
        [SerializeField] private FarmTile tile;
        
        private void OnEnable()
        {
            UpdateUI(tile.tileState);
            tile.OnChangeState.AddListener(UpdateUI);
        }

        private void OnDisable()
        {
            tile.OnChangeState.RemoveListener(UpdateUI);
        }

        private void UpdateUI(TileState state_)
        {
            panel.SetActive(tile.tileState != TileState.Empty && enabled);
            
            switch (tile.tileState)
            {
                case TileState.Empty:
                {
                    return;
                }
                case TileState.ReadyToHarvest:
                {
                    timeTxt.text = "Press F To Harvest";
                    break;
                }
                case TileState.Planted:
                {
                    timeTxt.text = "Has not watered yet";
                    break;
                }
                case TileState.Growing:
                {
                    var _totalMinutes = tile.minutesRemaining;
                    timeTxt.text = $"{_totalMinutes / 60:00}:{_totalMinutes % 60:00}";
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            cropNameText.text = tile.seedData.produceData.ItemName;
            icon.sprite = tile.seedData.produceData.Icon;
        }

        private void Update()
        {
            if(!panel.activeSelf) return;

            if (tile.tileState is 
                TileState.Empty or TileState.ReadyToHarvest or TileState.Planted) return;

            if (tile.tileState != TileState.Growing) return;
            
            var _totalMinutes = tile.minutesRemaining;
            timeTxt.text = $"{_totalMinutes / 60:00}:{_totalMinutes % 60:00}";
        }
    }
}
