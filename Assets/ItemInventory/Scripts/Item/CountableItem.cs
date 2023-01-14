using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemInventory
{
    public class CountableItem : BaseItem, Useable
    {
        private CountableItemData countableItemData { get; set; }

        public CountableItem(CountableItemData countableItemData) : base(countableItemData)
        {
            this.countableItemData = countableItemData;
            curAmount = countableItemData.Count;
        }

        public string Id => countableItemData.Id;
        public string ItemName => countableItemData.ItemName;
        public string Description => countableItemData.Description;
        public int Level => countableItemData.Level;
        public Sprite ItemSprite => countableItemData.ItemSprite;

        public int MaxAmount => maxAmount;
        public int CurAmount => curAmount;

        private int maxAmount = 100;
        private int minAmount = 1;
        private int curAmount;

        public void AddItem(CountableItemData countableItemData)
        {
            this.countableItemData = countableItemData;
        }

        public void AddAmount(int amount)
        {
            if (amount <= 0 || curAmount >= maxAmount)
            {
                return;
            }

            curAmount += amount;
        }

        public bool Use(int amount = 1)
        {
            if (curAmount == 0)
            {
                return false;
            }
            
            curAmount -= amount;
            return true;
        }
    }
}