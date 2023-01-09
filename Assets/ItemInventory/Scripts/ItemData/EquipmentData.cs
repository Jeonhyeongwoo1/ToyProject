using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemInventory
{
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "Config/Create equipmentData")]
    public class EquipmentData : ItemData
    {
        public float Durability => _Durability;
        public float Value => _Value;

        [SerializeField] private float _Durability;
        [SerializeField] private float _Value;

        public override void CreateItem()
        {
        }
    }
}