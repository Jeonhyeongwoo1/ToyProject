using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ItemInventory
{
    public class Inventory : MonoBehaviour
    {
        public List<ItemObject> ItemList => _ItemList;

        [ReadOnly]
        [SerializeField] private List<ItemObject> _ItemList = new List<ItemObject>();
        [SerializeField] private DragableItem _DragableItem;


        public void DestoryItemList()
        {
            foreach (ItemObject item in _ItemList)
            {
                Destroy(item.gameObject);
            }

            _ItemList.Clear();
        }

        public void SetDragableItem(ItemObject item)
        {
            item.SetDragableItem(_DragableItem);
        }

        public void DragableItemInit(Vector2 slotSize)
        {
            _DragableItem.Init(slotSize);
        }



    }
}