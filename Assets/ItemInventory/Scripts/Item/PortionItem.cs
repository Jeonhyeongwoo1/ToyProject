using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemInventory
{
    public class PortionItem : CountableItem
    {
        private PortionData portionData { get; set; }

        public PortionItem(PortionData portionData) : base(portionData)
        {
            this.portionData = portionData;
        }
    }
}