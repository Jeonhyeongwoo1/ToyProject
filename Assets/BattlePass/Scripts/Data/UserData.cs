using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattlePass
{
    [CreateAssetMenu(menuName = "BattlePass/Create UserData", fileName = "UserData")]
    public class UserData : ScriptableObject
    {
        public UserGoodsData userGoodsData;
        public UserEXPData userEXPData;
        public InventoryItemData inventoryItemData;
    }

    [Serializable]
    public class UserGoodsData
    {
        public int star;
        public int pearl;
        public int gold;
        public int diamond;
    }

    [Serializable]
    public class InventoryItemData
    {
        public List<string> itemIdList;
    }

    [Serializable]
    public class UserEXPData
    {
        public int tier;
        public int curExp;
    }
}