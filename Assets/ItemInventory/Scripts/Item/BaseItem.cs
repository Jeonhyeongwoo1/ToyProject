using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public class BaseItem
    {
        public ItemData ItemData => _ItemData;

        [SerializeField] private ItemData _ItemData;

        public BaseItem(ItemData itemData) => _ItemData = itemData;
    }
}