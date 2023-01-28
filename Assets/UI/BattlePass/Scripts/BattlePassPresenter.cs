using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace BattlePass
{
    public class BattlePassPresenter : MonoBehaviour
    {
        [SerializeField] private BattlePassPanel _BattlePassPanel;

        private List<BattlePassTier> _BattlePassTierList = new List<BattlePassTier>();

        private void Start()
        {
            // star.Subscribe((v) => OnChangedStarValue(v))
            //     .AddTo(gameObject);

            // pearl.Subscribe((v) => OnChangedPearlValue(v))
            //     .AddTo(gameObject);

            // gold.Subscribe((v) => OnChangedGoldValue(v))
            //     .AddTo(gameObject);

            // diamond.Subscribe((v) => OnChangedDaimondValue(v))
            //         .AddTo(gameObject);
        }

        public void UpdateTier(int tier, bool isOpenedBattlePass)
        {
            _BattlePassPanel.UpdateTierUI(tier, tier - 1, isOpenedBattlePass);
        }

        public void BuyBattlePass(int necessaryDiamond)
        {
            bool isSuccess = UserManager.Instance.UseDiamond(necessaryDiamond);
            if (!isSuccess)
            {
                return;
            }

            int tier = UserManager.Instance.GetUserTier();
            for (int i = 0; i < tier; i++)
            {
                BattlePassTier battlePassTier = _BattlePassTierList[i];
                _BattlePassPanel.OpenBattlePassElement(i);

                if (battlePassTier.HaveFreePassItem)
                {
                    _BattlePassPanel.ChangeHaveBattlePassItemUI(i, true);
                    UpdateUserData(battlePassTier.battlePassTierData.battlePassItemData.id, battlePassTier.battlePassTierData.battlePassItemData.itemValue);
                }
            }

            _BattlePassPanel.DeactivateBuyBattlePassButton();
        }

        public void UpdateUserEXP(int exp)
        {
            UIManager.Instance.UpdateUserEXP(exp);
        }

        public void SetUserGoodsUI(int star, int pearl, int gold, int diamond)
        {
            _BattlePassPanel.UpdateUserGoodsUI(star, pearl, gold, diamond);
        }

        public void SetUserEXP(int curExp, int maxExp)
        {
            _BattlePassPanel.UpdateUserEXP(curExp, maxExp);
        }

        public void CreateBattlePassElement(List<BattlePassTierData> battlePassTierDataList)
        {
            if (battlePassTierDataList == null)
            {
                return;
            }

            _BattlePassTierList.Clear();
            _BattlePassPanel.ClearBattlePassElement();

            int count = battlePassTierDataList.Count;
            _BattlePassPanel.CreateBattlePassElement(count);

            for (int i = 0; i < battlePassTierDataList.Count; i++)
            {
                BattlePassTier battlePassTier = new BattlePassTier(battlePassTierDataList[i]);
                _BattlePassTierList.Add(battlePassTier);
            }

            for (int i = 0; i < battlePassTierDataList.Count; i++)
            {
                UpdateBattlePassElementUI(i);
            }
        }

        public BattlePassPurchasePopup.BattlePassPurchaseData GetBattlePassPurchaseData(int tier)
        {
            BattlePassTierData tierData = _BattlePassTierList[tier].battlePassTierData;

            var data = new BattlePassPurchasePopup.BattlePassPurchaseData();
            try
            {
                data.battlePassItemSprtie = tierData.battlePassItemData.itemSprite;
                data.battlePassItemValue = tierData.battlePassItemData.itemValue.ToString();
                data.freePassItemSprite = tierData.freePassItemData.itemSprite;
                data.freePassItemValue = tierData.freePassItemData.itemValue.ToString();
                data.tier = tierData.tier;
                data.isLockBattlePass = tierData.battlePassItemData.isLock;
                data.hasItem = _BattlePassTierList[tier].HaveFreePassItem;
            }
            catch (System.Exception e)
            {
            }

            return data;
        }

        public void AddBattlePassTierListData(BattlePassTierData battlePassTierData)
        {
            if (battlePassTierData == null)
            {
                return;
            }

            BattlePassTier battlePassTier = new BattlePassTier(battlePassTierData);
            _BattlePassTierList.Add(battlePassTier);

            int index = _BattlePassTierList.FindIndex((v) => v == battlePassTier);
            UpdateBattlePassElementUI(index);
        }

        public void AddUserDiamond(int count)
        {
            UserManager.Instance.AddUserDiamond(count);
        }

        public void UpdateUserDiamondUI(int count)
        {
            _BattlePassPanel.UpdateUserDiamondUI(count);
        }

        public void PurchaseBattlePass(int tier)
        {
            bool isOpenedBattlePass = UserManager.Instance.IsOpenedBattlePass();

            BattlePassTier battlePassTier = _BattlePassTierList.Find((v) => v.battlePassTierData.tier == tier);
            if (battlePassTier == null)
            {
                return;
            }

            if (isOpenedBattlePass)
            {
                battlePassTier.HaveBattlePassItem = true;
            }

            battlePassTier.HaveFreePassItem = true;
            _BattlePassPanel.ChangeHaveBattlePassItemUI(tier - 1, isOpenedBattlePass);

            UpdateUserData(battlePassTier.battlePassTierData.freePassItemData.id, battlePassTier.battlePassTierData.freePassItemData.itemValue);
            if (!isOpenedBattlePass)
            {
                return;
            }

            UpdateUserData(battlePassTier.battlePassTierData.battlePassItemData.id, battlePassTier.battlePassTierData.battlePassItemData.itemValue);
        }

        public List<Sprite> GetUserItemSpriteList()
        {
            return UIManager.Instance.GetUserItemSpriteList();
        }

        private void UpdateUserData(string id, int value)
        {
            switch (id)
            {
                case "star":
                    UIManager.Instance.UpdateUserStar(value);
                    _BattlePassPanel.UpdatStarText(UserManager.Instance.GetUserStar().ToString());
                    break;
                case "pearl":
                    UIManager.Instance.UpdateUserPearl(value);
                    _BattlePassPanel.UpdatePearlText(UserManager.Instance.GetUserPearl().ToString());
                    break;
                case "gold":
                    UIManager.Instance.UpdateUserGold(value);
                    _BattlePassPanel.UpdateGoldText(UserManager.Instance.GetUserGold().ToString());
                    break;
                case "diamond":
                    UIManager.Instance.UpdateUserDiamond(value);
                    _BattlePassPanel.UpdateDiamondText(UserManager.Instance.GetUserDiamond().ToString());
                    break;
                case "sword":
                case "shield":
                case "crown":
                    UIManager.Instance.GainItem(id);
                    break;
                    //Sword, Shield, Crown
            }
        }

        private void UpdateBattlePassElementUI(int index)
        {
            BattlePassTier battlePassTier = _BattlePassTierList[index];
            if (battlePassTier == null)
            {
                Debug.LogWarning("There isn't battlePassTier Data");
                return;
            }

            BattlePassTierData data = battlePassTier.battlePassTierData;

            _BattlePassPanel.UpdateBattlePassTier(index, data.tier.ToString());
            _BattlePassPanel.UpdateBattlePassElementUI(index
                                                            , data.battlePassItemData.isLock
                                                            , data.battlePassItemData.itemSprite
                                                            , data.battlePassItemData.itemValue);

            _BattlePassPanel.UpdateFreePassElementUI(index
                                                            , data.freePassItemData.isLock
                                                            , data.freePassItemData.itemSprite
                                                            , data.freePassItemData.itemValue);
        }

        // private void OnChangedStarValue(int value)
        // {
        //     if (_User == null)
        //     {
        //         return;
        //     }

        //     _BattlePassPanel.UpdatStarText(value.ToString());
        //     _User.Star += value;
        // }

        // private void OnChangedPearlValue(int value)
        // {
        //     if (_User == null)
        //     {
        //         return;
        //     }

        //     _BattlePassPanel.UpdatePearlText(value.ToString());
        //     _User.Pearl += value;
        // }

        // private void OnChangedGoldValue(int value)
        // {
        //     if (_User == null)
        //     {
        //         return;
        //     }

        //     _BattlePassPanel.UpdateGodText(value.ToString());
        //     _User.Gold += value;
        // }

        // private void OnChangedDaimondValue(int value)
        // {
        //     if (_User == null)
        //     {
        //         return;
        //     }

        //     _BattlePassPanel.UpdateDiamondText(value.ToString());
        //     _User.Diamond += value;
        // }

    }
}