using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemInventory
{
    public abstract class CountableItemData : ItemData
    {
        public int Count => _Count;

        [SerializeField] private int _Count;
    }
}
