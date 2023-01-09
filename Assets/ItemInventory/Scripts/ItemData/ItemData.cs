using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemInventory
{
    public abstract class ItemData : ScriptableObject
    {
        public string Id => _Id;
        public string ItemName => _ItemName;
        public string Description => _Description;

        [SerializeField] private string _Id;
        [SerializeField] private string _ItemName;
        [SerializeField] private string _Description;

        public abstract void CreateItem();
    }
}