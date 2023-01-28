using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemInventory
{
    public class EquipementItem : BaseItem
    {
        private EquipmentData equipmentData { get; set; }

        public string Id => equipmentData.Id;
        public string ItemName => equipmentData.ItemName;
        public string Description => equipmentData.Description;
        public int Level => equipmentData.Level;
        public Sprite ItemSprite => equipmentData.ItemSprite;

        public float Durability => equipmentData.Durability;
        public float Value => equipmentData.Value;

        public EquipementItem(EquipmentData equipmentData) : base(equipmentData)
        {
            this.equipmentData = equipmentData;
        }
    }
}