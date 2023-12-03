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

        [SerializeField] private Slider slider;
        [SerializeField] private Sprite waterIcon;
        [SerializeField] private bool isRevised = false;

        private void OnEnable()
        {
            if (!isRevised)
            {
                UpdateUI(tile, tile.tileState);
                tile.OnChangeState.AddListener(UpdateUI);
            }
            else
            {
                UpdateUIRevised(tile, tile.tileState);
                tile.OnChangeState.AddListener(UpdateUIRevised);
            }
        }

        private void OnDisable()
        {
            if (!isRevised)
            {
                tile.OnChangeState.RemoveListener(UpdateUI);
            }
            else
            {
                tile.OnChangeState.RemoveListener(UpdateUIRevised);
            }
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
            if (!isRevised)
            {
                if (!panel.activeSelf) return;

                barFill.fillAmount = tile.progress;

                if (tile.tileState is
                    TileState.Empty or TileState.ReadyToHarvest or TileState.Planted) return;

                if (tile.tileState != TileState.Growing) return;

                UpdateGrowingTime();
            }

            else
            {
                if (!panel.activeSelf) return;

                if (tile.tileState is TileState.Empty or TileState.ReadyToHarvest or TileState.Planted) return;

                if (tile.tileState != TileState.Growing) return;

                UpdateGrowingTimeRevised();
            }
        }

        private void UpdateGrowingTime()
        {
            var _span = tile.timeRemaining;
            timeTxt.SetText(_span.Hours > 0 ? $"{_span.Hours:00}:{_span.Minutes:00}" : $"{_span.Minutes:00} mins");
        }

        private void UpdateUIRevised(FarmTile tile_, TileState state_)
        {
            panel.SetActive(tile.tileState != TileState.Empty && enabled);

            switch (tile.tileState)
            {
                case TileState.ReadyToHarvest:
                    {
                        slider.value = 0;
                        icon.sprite = tile.seedData.produceData.Icon;
                        icon.color = Color.white;
                        break;
                    }
                case TileState.Planted:
                    {
                        slider.value = tile.totalMinutesDuration;
                        icon.sprite = waterIcon;
                        break;
                    }
                case TileState.Growing:
                    {
                        slider.value = 0;
                        slider.maxValue = tile.totalMinutesDuration;
                        icon.sprite = tile.seedData.produceData.Icon;
                        icon.color = new Color(0.25f, 0.25f, 0.25f, 255f);
                        UpdateGrowingTimeRevised();
                        break;
                    }
                case TileState.Empty:
                default:
                    return;
            }
        }

        private void UpdateGrowingTimeRevised()
        {
            slider.value = (float)tile.timeRemaining.TotalMinutes;
        }
    }
}
