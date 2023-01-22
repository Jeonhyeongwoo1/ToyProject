using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UniRx;

namespace BattlePass
{
    public class BattlePassPanel : MonoBehaviour
    {
        [SerializeField] private BattlePassPresenter _BattlePassPresenter;
        [SerializeField] private BattlePassElement _BattlePassElement;
        [SerializeField] private Transform _BattlePassContentTransform;
        [SerializeField] private BattlePassPurchasePopup _BattlePassPurchasePopup;

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

        private List<BattlePassElement> _BattlePassElementList = new List<BattlePassElement>();

        public void UpdateUserDiamondUI(int count)
        {
            _DiamondText.text = count.ToString();
        }

        public void ChangeHaveBattlePassItemUI(int index, bool isOpenedBattlePassItem)
        {
            _BattlePassPurchasePopup.ClosePopup();
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

        public void UpdateGodText(string gold)
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

        }

        private void OnClickAddDiamondButton(int count)
        {
            _BattlePassPresenter.AddUserDiamond(count);
        }

        private void OnClickGainEXPButton()
        {
            _BattlePassPresenter.UpdateUserEXP(85);
        }

        private void OnPurchaseBattlePass()
        {
            _BattlePassPresenter.PurchaseBattlePass();
        }

        private void Start()
        {
            _InventoryButton.OnClickAsObservable()
                            .Subscribe((v) => OnClickInventoryButton());

            _AddDiamondButton.OnClickAsObservable()
                            .Subscribe((v) => OnClickAddDiamondButton(30));

            _GainEXPButton.OnClickAsObservable()
                            .Subscribe((v) => OnClickGainEXPButton());

            _BattlePassPurchasePopup.SetPurchaseBattlePassEvent(OnPurchaseBattlePass);
        }
    }

}