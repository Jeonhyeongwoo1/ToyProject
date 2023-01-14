using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ItemInventory
{
    public class Inventory : MonoBehaviour
    {
        /*
        1. 아이템은 실질적인 데이터를 가지고 있다.
        2. Add 해당 아이템을 갯수만큼 추가

        */

        [SerializeField] private BaseItem[] _ItemArray;
        [SerializeField] private InventoryUI _InventoryUI;

        private void Start()
        {
            _ItemArray = new BaseItem[InventoryData.Row * InventoryData.Column];
        }

        public void SwapItem(int originItemIndex, int swapItemIndex)
        {
            BaseItem temp = null;
            temp = _ItemArray[originItemIndex];
            _ItemArray[originItemIndex] = _ItemArray[swapItemIndex];
            _ItemArray[swapItemIndex] = temp;

            UpdateSlotUI(originItemIndex);
            UpdateSlotUI(swapItemIndex);
        }

        public void AddItemData(ItemData itemData)
        {
            if (itemData is CountableItemData countableItemData)
            {
                for (int i = 0; i < _ItemArray.Length; i++)
                {
                    if (_ItemArray[i] == null)
                    {
                        //새롭게 아이템 추가
                        int index = GetEmptyItemIndex();
                        if (index == -1)
                        {
                            //Popup
                            Debug.Log("There isn't empty item");
                            break;
                        }

                        _ItemArray[index] = new CountableItem(countableItemData);
                        UpdateSlotUI(index);
                        break;
                    }

                    CountableItem item = _ItemArray[i] as CountableItem;
                    if (item.Id == countableItemData.Id)
                    {
                        int count = item.CurAmount + countableItemData.Count;
                        if (count > item.MaxAmount)
                        {
                            continue;
                        }
                        else
                        {
                            //해당 아이템에 추가
                            item.AddAmount(countableItemData.Count);
                            UpdateSlotUI(i);
                            break;
                        }
                    }
                }
            }
            else if (itemData is EquipmentData equipmentData)
            {
                /*
                    *수량이 없기 때문에 빈 슬롯이 존재하면 넣기만 하면된다.
                */

                for (int i = 0; i < _ItemArray.Length; i++)
                {
                    if (_ItemArray[i] == null)
                    {
                        _ItemArray[i] = new EquipementItem(equipmentData);
                        UpdateSlotUI(i);
                        break;
                    }
                }
            }
        }

        private void UpdateSlotUI(int index)
        {
            BaseItem baseItem = _ItemArray[index];
            if (baseItem != null)
            {
                // if(baseItem.ItemData == null)
                // {
                //     _InventoryUI.RemoveSlotUI(index);
                //     return;
                // }

                _InventoryUI.SetItemIcon(index, baseItem.ItemData.ItemSprite);

                if (baseItem is CountableItem countableItem)
                {
                    _InventoryUI.SetCount(index, countableItem.CurAmount);
                }
                else if (baseItem is EquipementItem equipementItem)
                {
                    _InventoryUI.SetItemLevel(index, equipementItem.Level);
                }
            }
            else
            {
                //Remove   
                _InventoryUI.RemoveSlotUI(index);
            }
        }

        public void UseItem(int index, int amount = 1)
        {
            BaseItem baseItem = _ItemArray[index];
            if (baseItem is CountableItem countableItem)
            {
                bool isUse = countableItem.Use(amount);

                if (countableItem.CurAmount == 0)
                {
                    RemoveItemData(index);
                    return;
                }

                if (isUse)
                {
                    UpdateSlotUI(index);
                }
            }
        }

        public void RemoveItemData(int index, int amount = 1)
        {
            _ItemArray[index] = null;

            UpdateSlotUI(index);
        }

        private int GetEmptyItemIndex()
        {
            for (int i = 0; i < _ItemArray.Length; i++)
            {
                if (_ItemArray[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}