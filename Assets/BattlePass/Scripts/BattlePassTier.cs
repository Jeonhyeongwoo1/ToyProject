using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlePass
{
    public class BattlePassTier
    {
        public BattlePassTierData battlePassTierData { get; private set; }

        public bool HaveBattlePassItem
        {
            get => _HaveBattlePassItem;
            set => _HaveBattlePassItem = value;
        }

        public bool HaveFreePassItem
        {
            get => _HaveFreePassItem;
            set => _HaveFreePassItem = value;
        }

        private bool _HaveFreePassItem;
        private bool _HaveBattlePassItem;

        public BattlePassTier(BattlePassTierData battlePassTierData)
        {
            this.battlePassTierData = battlePassTierData;
        }
    }
}
