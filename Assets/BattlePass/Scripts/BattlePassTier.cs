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

        private bool _HaveBattlePassItem;

        public BattlePassTier(BattlePassTierData battlePassTierData)
        {
            this.battlePassTierData = battlePassTierData;
        }
    }
}
