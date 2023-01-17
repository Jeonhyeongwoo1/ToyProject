using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlePass
{
    public class BattlePassPresenter : MonoBehaviour
    {
        [SerializeField] private BattlePassPanel _BattlePassPanel;

        private List<BattlePassTier> battlePassTierList = new List<BattlePassTier>();

        public void CreateBattlePassElement(List<BattlePassTierData> battlePassTierDataList)
        {
            if (battlePassTierDataList == null)
            {
                return;
            }

            int count = battlePassTierDataList.Count;
            _BattlePassPanel.CreateBattlePassElement(count);

            battlePassTierList.Clear();
            for (int i = 0; i < battlePassTierDataList.Count; i++)
            {
                BattlePassTier battlePassTier = new BattlePassTier(battlePassTierDataList[i]);
                battlePassTierList.Add(battlePassTier);
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
            battlePassTierList.Add(battlePassTier);

            int index = battlePassTierList.FindIndex((v) => v == battlePassTier);
            UpdateBattlePassElementUI(index);
        }

        private void UpdateBattlePassElementUI(int index)
        {
            BattlePassTier battlePassTier = battlePassTierList[index];
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
    }
}