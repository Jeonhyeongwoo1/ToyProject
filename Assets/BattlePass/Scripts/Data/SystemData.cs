using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattlePass
{
    [CreateAssetMenu(menuName = "BattlePass/Create SystemData", fileName = "SystemData")]
    public class SystemData : ScriptableObject
    {
        public List<BattlePassTierData> battlePassDataList = new List<BattlePassTierData>();
    }

    [Serializable]
    public class BattlePassTierData
    {
        public int tier;
        public BattlePassTierItemData freePassItemData;
        public BattlePassTierItemData battlePassItemData;
    }

    [Serializable]
    public class BattlePassTierItemData
    {
        public bool isLock;
        public Sprite itemSprite;
        public int itemValue;
    }
}
