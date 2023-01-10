using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemInventory
{
    public class EquipementItem : BaseItem
    {
        public EquipmentData equipmentData { get; private set; }

        public EquipementItem(EquipmentData equipmentData) : base(equipmentData)
        {
            this.equipmentData = equipmentData;
        }
    }
}