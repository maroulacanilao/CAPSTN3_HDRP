using Farming;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Farming
{
    public class CropUIWorld : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI cropNameText, timeTxt;
        [SerializeField] private Image icon;

        private FarmTile currTile;

        // private void Awake()
        // {
        //     FarmTileInteractable.OnEnterFarmTile.AddListener(Enter);
        //     FarmTileInteractable.OnExitFarmTile.AddListener(Exit);
        //     TimeManager.OnMinuteTick.AddListener(UpdateUI);
        //     panel.SetActive(false);
        // }
        //
        // private void Enter(FarmTile obj_)
        // {
        //     if(obj_ == null) return;
        //     
        //     currTile = obj_;
        //     transform.position = currTile.transform.position.Add(0, 2.5f, 1f);
        //     panel.SetActive(true);
        //     var _produceData = currTile.seedData.produceData;
        //     cropNameText.text = _produceData.ItemName;
        //     icon.sprite = _produceData.Icon;
        //     UpdateUI(TimeManager.Instance);
        // }
        //
        // private void Exit(FarmTile obj_)
        // {
        //     if(obj_ == null) return;
        //     panel.SetActive(false);
        // }
        //
        // private void UpdateUI(TimeManager timeManager_)
        // {
        //     if(!panel.gameObject.activeSelf) return;
        //     if (currTile == null || currTile.tileState == TileState.Empty)
        //     {
        //         panel.SetActive(false);
        //         return;
        //     }
        //     if(currTile.tileState == TileState.ReadyToHarvest)
        //     {
        //         timeTxt.text = "Ready To Harvest";
        //         return;
        //     }
        //     var _totalMinutes = currTile.minutesRemaining;
        //     timeTxt.text = $"{_totalMinutes / 60:00}:{_totalMinutes % 60:00}";
        //
        // }
        //
        // private void OnEnable()
        // {
        //     if (currTile == null)
        //     {
        //         panel.SetActive(false);
        //     }
        // }
    }
}
