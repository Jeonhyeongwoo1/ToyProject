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
        public BattlePassItemData battlePassItemData;
        public bool isLock;
    }

    [Serializable]
    public class BattlePassItemData
    {
        public Sprite itemSprite;
        public int itemValue;
    }
}
