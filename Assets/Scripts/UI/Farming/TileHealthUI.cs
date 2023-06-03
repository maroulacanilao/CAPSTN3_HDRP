using System;
using BaseCore;
using Farming;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Farming
{
    public class TileHealthUI : MonoBehaviour
    {
        [SerializeField] FarmTile farmTile;
        [SerializeField] private GameObject barPanel;
        [SerializeField] Image healthBar;

        private void Start()
        {
            farmTile.health.OnHealthChanged.AddListener(UpdateBar);
            UpdateBar(farmTile.health);
        }

        private void OnDestroy()
        {
            farmTile.health.OnHealthChanged.AddListener(UpdateBar);
        }

        private void UpdateBar(GenericHealth health_)
        {
            if (health_.CurrentHealth == health_.MaxHealth)
            {
                barPanel.SetActive(false);
                return;
            }
            
            barPanel.SetActive(true);
            healthBar.fillAmount = health_.HealthPercentage;
        }
    }
}