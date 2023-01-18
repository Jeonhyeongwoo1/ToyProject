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

        public void UpdateUserEXP(int exp)
        {
            UIManager.Instance.UpdateUserEXP(exp);
        }

        public void SetUserGoodsUI(int star, int pearl, int gold, int diamond)
        {
            _BattlePassPanel.UpdateUserGoodsUI(star, pearl, gold, diamond);
        }

        public void SetUserEXP(int tier, int curExp, int maxExp)
        {
            _BattlePassPanel.UpdateUserEXP(tier, curExp, maxExp);
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