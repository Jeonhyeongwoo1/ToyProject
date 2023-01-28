using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemInventory
{
    [CreateAssetMenu(fileName = "Portion_Data", menuName = "Config/Create portionData")]
    public class PortionData : CountableItemData
    {
        public float Value => _Value;

        [SerializeField] private float _Value;

        public override BaseItem CreateItem()
        {
            return new PortionItem(this);
        }
    }
}