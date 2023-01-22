using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace BattlePass
{
    public class BattlePassElement : MonoBehaviour
    {
        [SerializeField] private Text _TierText;

        [Header("[FreePassItem]")]
        [SerializeField] private Image _FreePassItemImage;
        [SerializeField] private Text _FreePassItemValueText;
        [SerializeField] private RectTransform _FreePassBlockPanel;
        [SerializeField] private RectTransform _FreePassLockTransform;
        [SerializeField] private RectTransform _FreePassUnlockTransform;
        [SerializeField] private RectTransform _FreePassHaveItemTransform;
        [SerializeField] private Button _FreePassButton;

        [Header("[BattlePassItem]")]
        [SerializeField] private Image _BattlePassItemImage;
        [SerializeField] private Text _BattlePassItemValueText;
        [SerializeField] private RectTransform _BattlePassBlockPanel;
        [SerializeField] private RectTransform _BattlePassLockTransform;
        [SerializeField] private RectTransform _BattlePassUnlockTransform;
        [SerializeField] private RectTransform _BattlePassHaveItemTransform;
        [SerializeField] private Button _BattlePassButton;

        private event Action _OnOpenBattlePassPurchasePopup;

        public void Init(int tier, Action<int> openBattlePassPurchasePopup)
        {
            _OnOpenBattlePassPurchasePopup = () => openBattlePassPurchasePopup?.Invoke(tier);
        }

        public void UpdateTier(string tier)
        {
            _TierText.text = tier;
        }

        public void HaveItem(bool isOpenedBattlePassItem)
        {
            ActvieHaveFreePassItem();

            if (isOpenedBattlePassItem)
            {
                ActvieHaveBattlePassItem();
            }
        }

        public void OpenItem(bool isOpenedBattlePassItem)
        {
            ActiveFreePassItem();

            if (isOpenedBattlePassItem)
            {
                ActiveBattlePassItem();
            }
        }

        private void ActiveBattlePassItem()
        {
            _BattlePassUnlockTransform.gameObject.SetActive(true);
            _BattlePassLockTransform.gameObject.SetActive(false);
            _BattlePassHaveItemTransform.gameObject.SetActive(false);
            _BattlePassBlockPanel.gameObject.SetActive(false);
        }

        private void ActiveFreePassItem()
        {
            _FreePassUnlockTransform.gameObject.SetActive(true);
            _FreePassLockTransform.gameObject.SetActive(false);
            _FreePassHaveItemTransform.gameObject.SetActive(false);
            _FreePassBlockPanel.gameObject.SetActive(false);
        }

        private void ActvieHaveFreePassItem()
        {
            _FreePassUnlockTransform.gameObject.SetActive(false);
            _FreePassLockTransform.gameObject.SetActive(false);
            _FreePassHaveItemTransform.gameObject.SetActive(true);
        }

        private void ActvieHaveBattlePassItem()
        {
            _BattlePassUnlockTransform.gameObject.SetActive(false);
            _BattlePassLockTransform.gameObject.SetActive(false);
            _BattlePassHaveItemTransform.gameObject.SetActive(true);
        }

        public void UpdateBattlePassLockUI(bool isLock)
        {
            _BattlePassUnlockTransform.gameObject.SetActive(!isLock);
            _BattlePassLockTransform.gameObject.SetActive(isLock);
        }

        public void UpdateFreePassLockUI(bool isLock)
        {
            _FreePassUnlockTransform.gameObject.SetActive(!isLock);
            _FreePassLockTransform.gameObject.SetActive(isLock);
        }

        public void UpdateBattlePassElementUI(bool isLock, Sprite itemSprite, int itemValue)
        {
            _BattlePassBlockPanel.gameObject.SetActive(isLock);
            _BattlePassItemImage.sprite = itemSprite;
            _BattlePassItemValueText.text = "+" + itemValue.ToString();

            UpdateBattlePassLockUI(isLock);
        }

        public void UpdateFreePassElementUI(bool isLock, Sprite itemSprite, int itemValue)
        {
            _FreePassBlockPanel.gameObject.SetActive(isLock);
            _FreePassItemImage.sprite = itemSprite;
            _FreePassItemValueText.text = "+" + itemValue.ToString();

            UpdateFreePassLockUI(isLock);
        }

        private void OnClickPurchasableElement(Button button)
        {
            if (!button.enabled)
            {
                return;
            }

            _OnOpenBattlePassPurchasePopup?.Invoke();
        }

        private void Start()
        {
            _FreePassButton.OnClickAsObservable()
                            .Subscribe((v) => OnClickPurchasableElement(_FreePassButton));

            _BattlePassButton.OnClickAsObservable()
                            .Subscribe((v) => OnClickPurchasableElement(_BattlePassButton));
        }
    }
}