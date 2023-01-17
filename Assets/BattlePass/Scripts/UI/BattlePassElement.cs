using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        [Header("[BattlePassItem]")]
        [SerializeField] private Image _BattlePassItemImage;
        [SerializeField] private Text _BattlePassItemValueText;
        [SerializeField] private RectTransform _BattlePassBlockPanel;
        [SerializeField] private RectTransform _BattlePassLockTransform;
        [SerializeField] private RectTransform _BattlePassUnlockTrasnform;

        public void UpdateTier(string tier)
        {
            _TierText.text = tier;
        }

        public void UpdateBattlePassElementUI(bool isLock, Sprite itemSprite, int itemValue)
        {
            _BattlePassBlockPanel.gameObject.SetActive(isLock);
            _BattlePassUnlockTrasnform.gameObject.SetActive(!isLock);
            _BattlePassLockTransform.gameObject.SetActive(isLock);
            _BattlePassItemImage.sprite = itemSprite;
            _BattlePassItemValueText.text = "+" + itemValue.ToString();
        }

        public void UpdateFreePassElementUI(bool isLock, Sprite itemSprite, int itemValue)
        {
            _FreePassBlockPanel.gameObject.SetActive(isLock);
            _FreePassUnlockTransform.gameObject.SetActive(!isLock);
            _FreePassLockTransform.gameObject.SetActive(isLock);
            _FreePassItemImage.sprite = itemSprite;
            _FreePassItemValueText.text = "+" + itemValue.ToString();
        }
    }
}