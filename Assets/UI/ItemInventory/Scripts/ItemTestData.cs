using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu(fileName = "ItemTestData", menuName = "InventoryItem")]
public class ItemTestData : ScriptableObject
{
    [Serializable]
    public class TestData
    {
        public string itemName;
        public Sprite sprite;
        public int count;
        public string level;
    }

    public List<TestData> list = new List<TestData>();


}
