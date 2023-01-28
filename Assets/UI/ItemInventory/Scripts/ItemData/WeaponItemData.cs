using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemInventory
{
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "Config/Create WeaponData")]
    public class WeaponItemData : EquipmentData
    {

        public override BaseItem CreateItem()
        {
            return new WeapornItem(this);
        }
    }
}