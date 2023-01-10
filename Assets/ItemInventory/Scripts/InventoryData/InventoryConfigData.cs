using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemInventory
{
    [CreateAssetMenu(fileName = "InventoryConfigData", menuName = "Config/Create inventoryConfigData")]
    public class InventoryConfigData : ScriptableObject
    {
        public float contentPadding;
        public float contentSpace;
        public int row;
        public int column;
    }

    public static class InventoryData
    {
        public static InventoryConfigData inventoryConfigData;
        public static InventoryConfigData InventoryConfigData
        {
            get
            {
                if (inventoryConfigData == null)
                {
                    inventoryConfigData = UIManager.Instance.InventoryConfigData;
                }

                return inventoryConfigData;
            }
        }

        public static float ContentPadding => InventoryConfigData.contentPadding;
        public static float ContentSpace => InventoryConfigData.contentSpace;
        public static int Row => InventoryConfigData.row;
        public static int Column => InventoryConfigData.column;
    }
}