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
        [SerializeField] private Image barFill;
        
        private void OnEnable()
        {
            UpdateUI(tile, tile.tileState);
            tile.OnChangeState.AddListener(UpdateUI);
        }

        private void OnDisable()
        {
            tile.OnChangeState.RemoveListener(UpdateUI);
        }

        private void UpdateUI(FarmTile tile_, TileState state_)
        {
            panel.SetActive(tile.tileState != TileState.Empty && enabled);
            
            switch (tile.tileState)
            {
                case TileState.ReadyToHarvest:
                {
                    timeTxt.text = "Press E To Harvest";
                    break;
                }
                case TileState.Planted:
                {
                    timeTxt.text = "Has not watered yet";
                    break;
                }
                case TileState.Growing:
                {
                    UpdateGrowingTime();
                    break;
                }
                case TileState.Empty:
                default:
                    return;
            }
            
            cropNameText.text = tile.seedData.produceData.ItemName;
            icon.sprite = tile.seedData.produceData.Icon;
        }

        private void Update()
        {
            if(!panel.activeSelf) return;

            barFill.fillAmount = tile.progress;

            if (tile.tileState is 
                TileState.Empty or TileState.ReadyToHarvest or TileState.Planted) return;

            if (tile.tileState != TileState.Growing) return;

            UpdateGrowingTime();
        }

        private void UpdateGrowingTime()
        {
            var _span = tile.timeRemaining;
            timeTxt.SetText(_span.Hours > 0 ? $"{_span.Hours:00}:{_span.Minutes:00}" : $"{_span.Minutes:00} mins");
        }
    }
}
