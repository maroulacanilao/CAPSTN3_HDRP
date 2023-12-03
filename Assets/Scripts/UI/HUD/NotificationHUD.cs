using System;
using System.Collections;
using CustomHelpers;
using DG.Tweening;
using Farming;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class NotificationHUD : MonoBehaviour
    {
        [SerializeField] private GameObject harvestNotification;
        [SerializeField] private Image harvestIcon;
        [SerializeField] private TextMeshProUGUI harvestNameTxt;
        [SerializeField] private TextMeshProUGUI harvestTxt;

        [SerializeField] private TextMeshProUGUI expTxt;

        private Vector3 defaultHarvestNotificationPos;
        private Vector3 defaultExpNotificationPos;
        private Transform Player;

        private Vector3 originalPos;

        private void Awake()
        {
            originalPos = harvestNotification.transform.localPosition;
        }

        private void Start()
        {
            defaultExpNotificationPos = expTxt.transform.position;
            defaultHarvestNotificationPos = harvestNotification.transform.position;
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void OnEnable()
        {
            FarmTileManager.OnSuccessHarvest.AddListener(OnSuccessHarvest);
            FishingManager.OnCatchSuccess.AddListener(OnSuccessHarvest);
            harvestNotification.SetActive(false);
            expTxt.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            FarmTileManager.OnSuccessHarvest.RemoveListener(OnSuccessHarvest);
            FishingManager.OnCatchSuccess.RemoveListener(OnSuccessHarvest);

            harvestNotification.transform.localPosition = originalPos;
        }

        private void OnSuccessHarvest(Item item, int exp_)
        {
            // StartCoroutine(Co_DisplayHarvest(item));
            PlayHarvestNotifAnim(item);
            StartCoroutine(Co_DisplayExp(exp_));
            Debug.Log(item.Data.name);
            Debug.Log(exp_);
        }
        
        private IEnumerator Co_DisplayHarvest(Item item)
        {
            harvestNotification.SetActive(false);
            if (item == null) yield break;
            harvestIcon.sprite = item.Data.Icon;
            if (harvestNameTxt != null) harvestNameTxt.text = item.Data.ItemName;
            harvestTxt.SetText($"+{item.StackCount}");
            harvestNotification.SetActive(true);

            yield return new WaitForSecondsRealtime(2f);
            harvestNotification.SetActive(false);
        }
        
        private IEnumerator Co_DisplayExp(int exp_)
        {
            expTxt.gameObject.SetActive(false);
            
            if (Player.IsEmptyOrDestroyed() || !Player.gameObject.activeInHierarchy)
            {
                yield break;
            }
            var _cam = gameObject.scene.GetFirstMainCameraInScene(false);
            
            if (_cam == null) yield break;
            
            expTxt.transform.position = defaultExpNotificationPos;
            expTxt.SetText($"+{exp_} EXP");
            expTxt.gameObject.SetActive(true);
            
            var _pos = _cam.WorldToScreenPoint(Player.position);
            expTxt.transform.position = _pos;
            yield return expTxt.transform.DOMoveY(_pos.y + 100f, 1f).SetEase(Ease.OutCubic).SetUpdate(true).WaitForCompletion();
            
            expTxt.gameObject.SetActive(false);
        }

        private void PlayHarvestNotifAnim(Item item)
        {
            harvestNotification.transform.localPosition = new Vector3(1600f, originalPos.y);
            harvestNotification.SetActive(true);

            if (item == null) return;
            harvestIcon.sprite = item.Data.Icon;
            if (harvestNameTxt != null) harvestNameTxt.text = item.Data.ItemName;
            harvestTxt.SetText($"+{item.StackCount}");

            var sequence = DOTween.Sequence();
            sequence.Append(harvestNotification.transform.DOLocalMoveX(originalPos.x, 0.5f));
            sequence.AppendInterval(1.5f);
            sequence.Append(harvestNotification.transform.DOLocalMoveX(1600f, 0.5f));
        }
    }
}
