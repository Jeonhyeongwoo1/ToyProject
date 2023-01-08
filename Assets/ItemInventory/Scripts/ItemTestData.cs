using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ItemTestData", menuName = "InventoryItem")]
public class ItemTestData : ScriptableObject
{
    [Serializable]
    public class TestData
    {
        public string itemName;
        public int count;
    }

    public List<TestData> list = new List<TestData>();


}
