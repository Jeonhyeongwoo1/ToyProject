using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemInventory
{
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "Config/Create AccData")]
    public class ACCItemData : EquipmentData
    {

        public override BaseItem CreateItem()
        {
            return new ACCItem(this);
        }
    }
}
