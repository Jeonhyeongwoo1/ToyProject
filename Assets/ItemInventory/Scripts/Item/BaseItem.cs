using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType
{
    Portion,
    ETC,
    Weapon,
    Armor,
    ACC
}

namespace ItemInventory
{
    [Serializable]
    public class BaseItem
    {
        public ItemData ItemData => _ItemData;

        [SerializeField] private ItemData _ItemData;

        public BaseItem(ItemData itemData) => _ItemData = itemData;
    }
}