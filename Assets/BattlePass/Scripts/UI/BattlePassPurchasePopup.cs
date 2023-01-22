using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UniRx;

public class BattlePassPurchasePopup : MonoBehaviour
{
    [Serializable]
    public struct BattlePassPurchaseData
    {
        public string tier;
        public Sprite freePassItemSprite;
        public Sprite battlePassItemSprtie;
        public string freePassItemValue;
        public string battlePassItemValue;
        public bool isLockBattlePass;
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

    private event Action _OnPurchaseBattlePassEvent;

    public void OpenPopup(BattlePassPurchaseData data)
    {
        _FreePassItemImage.sprite = data.freePassItemSprite;
        _FreePassItemValueText.text = data.freePassItemValue;
        _BattlePassItemImage.sprite = data.battlePassItemSprtie;
        _BattlePassItemValueText.text = data.battlePassItemValue;
        _TierText.text = "Tier : " + data.tier;

        _BuyBattlePassAlarmText.gameObject.SetActive(data.isLockBattlePass);
        _BattlePassItemBackgroundImage.color = !data.isLockBattlePass ? _ActiveItemColor : _DisableItemColor;
        _PurchasePopupRectTransform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        Observable.FromCoroutine<Vector3>((observer) => ResizeScaleCor(observer, true))
                    .Subscribe((v) => _PurchasePopupRectTransform.localScale = v)
                    .AddTo(gameObject);
    }

    public void SetPurchaseBattlePassEvent(Action handler) => _OnPurchaseBattlePassEvent = handler;

    public void ClosePopup()
    {
        Observable.FromCoroutine<Vector3>((observer) => ResizeScaleCor(observer, false, () => gameObject.SetActive(false)))
                    .Subscribe((v) => _PurchasePopupRectTransform.localScale = v)
                    .AddTo(gameObject);
    }
    
    private void PurchaseBattlePass() => _OnPurchaseBattlePassEvent?.Invoke();

    private IEnumerator ResizeScaleCor(IObserver<Vector3> observer, bool isUp, Action done = null)
    {
        float elapsed = 0;
        float duration = 0.3f;
        Vector3 start = isUp ? Vector3.zero : Vector3.one;
        Vector3 end = isUp ? Vector3.one : Vector3.zero;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Vector3 value = Vector3.Lerp(start, end, elapsed / duration);
            observer.OnNext(value);
            yield return null;
        }

        done?.Invoke();
    }

    private void Start()
    {
        _CloseButton.OnClickAsObservable()
                    .Subscribe((v) => ClosePopup());

        _PurchaseClickButton.OnClickAsObservable()
                    .Subscribe((v) => PurchaseBattlePass());
    }
}