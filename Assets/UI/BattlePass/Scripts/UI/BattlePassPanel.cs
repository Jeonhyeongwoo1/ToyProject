using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;
using UniRx;

namespace BattlePass
{
    public abstract class BasePopup : MonoBehaviour
    {
        public virtual void OpenPopup(object data)
        {
            gameObject.SetActive(true);
        }

        public virtual void ClosePopup()
        {
            gameObject.SetActive(false);
        }

        protected void ResizeScale(Transform target, bool isUp, Action done = null)
        {
            Observable.FromCoroutine<Vector3>((observer) => ResizeScaleCor(observer, isUp, done))
                     .Subscribe((v) => target.localScale = v)
                     .AddTo(gameObject);
        }

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
    }

    public class BattlePassPanel : MonoBehaviour
    {
        [SerializeField] private BattlePassPresenter _BattlePassPresenter;
        [SerializeField] private BattlePassElement _BattlePassElement;
        [SerializeField] private Transform _BattlePassContentTransform;
        [SerializeField] private BattlePassPurchasePopup _BattlePassPurchasePopup;
        [SerializeField] private InventoryPopup _InventoryPopup;

        [Header("[GoodsValueList]")]
        [SerializeField] private Text _StarText;
        [SerializeField] private Text _PearlText;
        [SerializeField] private Text _GoldText;
        [SerializeField] private Text _DiamondText;

        [SerializeField] private Button _InventoryButton;
        [SerializeField] private Button _AddDiamondButton;

        [SerializeField] private Text _UserTierText;
        [SerializeField] private Text _UserEXPText;
        [SerializeField] private Slider _UserEXPSlider;

        [SerializeField] private Button _GainEXPButton;
        [SerializeField] private Button _BuyBattlePassButton;

        [SerializeField] private Color _BuyBattlePassButtonDeactivateColor;

        private List<BattlePassElement> _BattlePassElementList = new List<BattlePassElement>();

        public void UpdateUserDiamondUI(int count)
        {
            _DiamondText.text = count.ToString();
        }

        public void ChangeHaveBattlePassItemUI(int index, bool isOpenedBattlePassItem)
        {
            if (_BattlePassPurchasePopup.IsOpened)
            {
                _BattlePassPurchasePopup.ClosePopup();
            }

            _BattlePassElementList[index].HaveItem(isOpenedBattlePassItem);
        }

        public void UpdateTierUI(int tier, int index, bool isOpenedBattlePassItem)
        {
            _UserTierText.text = "Tier " + tier.ToString();
            _BattlePassElementList[index].OpenItem(isOpenedBattlePassItem);
        }

        public void UpdateUserEXP(int curExp, int maxExp)
        {
            _UserEXPText.text = curExp.ToString() + "/" + maxExp.ToString();
            _UserEXPSlider.value = ((float)curExp / (float)maxExp);
        }

        public void UpdateBattlePassTier(int index, string tier)
        {
            _BattlePassElementList[index]!.UpdateTier(tier);
        }

        public void UpdateBattlePassElementUI(int index, bool isLock, Sprite itemSprite, int itemValue)
        {
            _BattlePassElementList[index]!.UpdateBattlePassElementUI(isLock, itemSprite, itemValue);
        }

        public void UpdateFreePassElementUI(int index, bool isLock, Sprite itemSprite, int itemValue)
        {
            _BattlePassElementList[index]!.UpdateFreePassElementUI(isLock, itemSprite, itemValue);
        }

        public void UpdateUserGoodsUI(int star, int pearl, int gold, int diamond)
        {
            _StarText.text = star.ToString();
            _PearlText.text = pearl.ToString();
            _GoldText.text = gold.ToString();
            _DiamondText.text = diamond.ToString();
        }

        public void UpdatStarText(string star)
        {
            _StarText.text = star;
        }

        public void UpdatePearlText(string pearl)
        {
            _PearlText.text = pearl;
        }

        public void UpdateGoldText(string gold)
        {
            _GoldText.text = gold;
        }

        public void UpdateDiamondText(string diamond)
        {
            _DiamondText.text = diamond;
        }

        [Button]
        public void CreateBattlePassElement(int count)
        {
            for (int i = 0; i < count; i++)
            {
                BattlePassElement element = Instantiate(_BattlePassElement, _BattlePassContentTransform);
                element.Init(i, OpenBattlePassPurchasePopup);
                _BattlePassElementList.Add(element);
            }
        }

        [Button]
        public void ClearBattlePassElement()
        {
            for (int i = 0; i < _BattlePassElementList.Count; i++)
            {
                Destroy(_BattlePassElementList[i].gameObject);
            }

            _BattlePassElementList.Clear();
        }

        public void DeactivateBuyBattlePassButton()
        {
            _BuyBattlePassButton.enabled = false;
            _BuyBattlePassButton.GetComponent<Image>().color = _BuyBattlePassButtonDeactivateColor;
            _BuyBattlePassButton.transform.GetChild(1).GetComponent<Image>().color = _BuyBattlePassButtonDeactivateColor;
        }

        public void OpenBattlePassElement(int index)
        {
            _BattlePassElementList[index].ActiveBattlePassItem();
        }

        private void OpenBattlePassPurchasePopup(int tier)
        {
            BattlePassPurchasePopup.BattlePassPurchaseData data = _BattlePassPresenter.GetBattlePassPurchaseData(tier);

            if (data.Equals(default))
            {
                Debug.LogError("BattlePass data null");
                return;
            }

            _BattlePassPurchasePopup.OpenPopup(data);
        }

        private void OnClickInventoryButton()
        {
            List<Sprite> list = _BattlePassPresenter.GetUserItemSpriteList();
            _InventoryPopup.OpenPopup(list);
        }

        private void OnClickAddDiamondButton(int count)
        {
            _BattlePassPresenter.AddUserDiamond(count);
        }

        private void OnClickGainEXPButton()
        {
            _BattlePassPresenter.UpdateUserEXP(85);
        }

        private void OnPurchaseBattlePass(int tier)
        {
            _BattlePassPresenter.PurchaseBattlePass(tier);
        }

        private void OnBuyBattlePass(int necessaryDiamond)
        {
            _BattlePassPresenter.BuyBattlePass(necessaryDiamond);
        }

        private void Start()
        {
            _InventoryButton.OnClickAsObservable()
                            .Subscribe((v) => OnClickInventoryButton());

            _AddDiamondButton.OnClickAsObservable()
                            .Subscribe((v) => OnClickAddDiamondButton(30));

            _GainEXPButton.OnClickAsObservable()
                            .Subscribe((v) => OnClickGainEXPButton());

            _BuyBattlePassButton.OnClickAsObservable()
                                .Subscribe((v) => OnBuyBattlePass(10));

            _BattlePassPurchasePopup.SetPurchaseBattlePassEvent(OnPurchaseBattlePass);
        }
    }

}