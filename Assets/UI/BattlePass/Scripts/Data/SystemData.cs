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
        public List<ItemData> itemDataList = new List<ItemData>();
    }

    [Serializable]
    public struct ItemData
    {
        public string id;
        public Sprite itemSprite;
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
        public string id;
        public bool isLock;
        public Sprite itemSprite;
        public int itemValue;
    }
}
