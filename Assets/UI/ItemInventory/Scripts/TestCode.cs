using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestCode
{
    [Serializable]
    public class TestData
    {
        public string itemName;
        public int count;
    }

    public static List<TestData> testDataList = new List<TestData>()
    {
        new TestData() { itemName = "HPPortion", count = 3},
        new TestData() { itemName = "Armor", count = 1},
        new TestData() { itemName = "Sword", count = 1},
        new TestData() { itemName = "Earrings", count = 1},
    };
}
