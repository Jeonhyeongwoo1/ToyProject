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
    public interface Useable
    {
        bool Use(int amount = 1);
    }

    [Serializable]
    public class BaseItem
    {
        public ItemData ItemData
        {
            get => _ItemData;
            set => _ItemData = value;
        }

        [SerializeField] private ItemData _ItemData;

        public BaseItem(ItemData itemData) => _ItemData = itemData;
    }
}