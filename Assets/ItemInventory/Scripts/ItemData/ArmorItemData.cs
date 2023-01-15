using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemInventory
{
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "Config/Create ArmorData")]
    public class ArmorItemData : EquipmentData
    {

        public override BaseItem CreateItem()
        {
            return new ArmorItem(this);
        }
    }
}

