using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ItemInventory
{
    public abstract class ItemData : ScriptableObject
    {
        public string Id => _Id;
        public string ItemName => _ItemName;
        public string Description => _Description;
        public int Level => _Level;
        public Sprite ItemSprite => _ItemSprite;

        [SerializeField] private string _Id;
        [SerializeField] private string _ItemName;
        [SerializeField] private string _Description;
        [SerializeField] private int _Level;
        [SerializeField] private Sprite _ItemSprite;

        public abstract void CreateItem();
    }
}