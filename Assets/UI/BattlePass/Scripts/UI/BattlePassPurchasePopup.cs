using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UniRx;

namespace BattlePass
{
    public class BattlePassPurchasePopup : BasePopup
    {
        public bool IsOpened => _IsOpened;

        [Serializable]
        public struct BattlePassPurchaseData
        {
            public int tier;
            public Sprite freePassItemSprite;
            public Sprite battlePassItemSprtie;
            public string freePassItemValue;
            public string battlePassItemValue;
            public bool isLockBattlePass;
            public bool hasItem;
        }

        [SerializeField] private RectTransform _PurchasePopupRectTransform;
        [SerializeField] private Button _CloseButton;
        [SerializeField] private Image _FreePassItemImage;
        [SerializeField] private Text _FreePassItemValueText;
        [SerializeField] private Image _BattlePassItemImage;
        [SerializeField] private Text _BattlePassItemValueText;
        [SerializeField] private Button _PurchaseClickButton;
        [SerializeField] private Text _TierText;
        [SerializeField] private Text _BuyBattlePassAlarmText;
        [SerializeField] private Image _BattlePassItemBackgroundImage;

        [SerializeField] private Color _ActiveItemColor;
        [SerializeField] private Color _DisableItemColor;

        private event Action<int> _OnPurchaseBattlePassEvent;
        private bool _IsOpened;
        private int _Tier;

        public override void OpenPopup(object data)
        {
            BattlePassPurchaseData bData = (BattlePassPurchaseData) data;
            _FreePassItemImage.sprite = bData.freePassItemSprite;
            _FreePassItemValueText.text = bData.freePassItemValue;
            _BattlePassItemImage.sprite = bData.battlePassItemSprtie;
            _BattlePassItemValueText.text = bData.battlePassItemValue;
            _TierText.text = "Tier : " + bData.tier.ToString();

            _Tier = bData.tier;
            _PurchaseClickButton.transform.GetChild(0).TryGetComponent<Text>(out var text);
            _PurchaseClickButton.enabled = !bData.hasItem;
            text.text = bData.hasItem ? "Claimed" : "Claim";

            _IsOpened = true;
            _BuyBattlePassAlarmText.gameObject.SetActive(bData.isLockBattlePass);
            _BattlePassItemBackgroundImage.color = !bData.isLockBattlePass ? _ActiveItemColor : _DisableItemColor;
            _PurchasePopupRectTransform.localScale = Vector3.zero;
            gameObject.SetActive(true);

            base.OpenPopup(data);
            ResizeScale(_PurchasePopupRectTransform, true);
        }

        public void SetPurchaseBattlePassEvent(Action<int> handler) => _OnPurchaseBattlePassEvent = handler;

        public override void ClosePopup()
        {
            _IsOpened = false;
            ResizeScale(_PurchasePopupRectTransform, false, () => gameObject.SetActive(false));
        }

        private void PurchaseBattlePass() => _OnPurchaseBattlePassEvent?.Invoke(_Tier);

        private void Start()
        {
            _CloseButton.OnClickAsObservable()
                        .Subscribe((v) => ClosePopup());

            _PurchaseClickButton.OnClickAsObservable()
                        .Subscribe((v) => PurchaseBattlePass());
        }
    }
}
