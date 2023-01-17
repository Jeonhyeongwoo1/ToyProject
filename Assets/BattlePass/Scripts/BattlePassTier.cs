using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlePass
{
    public class BattlePassTier
    {
        public BattlePassTierData battlePassTierData { get; private set; }

        public BattlePassTier(BattlePassTierData battlePassTierData)
        {
            this.battlePassTierData = battlePassTierData;
        }
    }
}
