using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ItemInventory
{
    public class Inventory : MonoBehaviour
    {
        //원래의 아이템 데이터를 가지고 있는 배열
        [SerializeField] private BaseItem[] _ItemArray;
        //Invnetory Type별 보여줄 View 배열
        [SerializeField] private BaseItem[] _ViewItemArray;

        [SerializeField] private InventoryUI _InventoryUI;

        /*
        1.View, Origin을 나눠서 저장한다.
        2.View는 SlotUI에 전달한 타입별 데이터를 가지고있따
        3.Oriign은 전체 데이터를 보관하고있다.

        1.아이템이 추가될 경우
            Origin에 데이터를 업데이트를 한다.
            Origin업데이트가 완료되고난 후 같은 타입이라면 View에 아이템을 추가한다.
            View에 있는 데이터를 순차적으로 SlotUI에 전달한다.


        2.아이템 삭제
            삭제할 아이템의 인덱스를 받아온다.
            View[인덱스] 값을 넣어서 해당 BaseItem의 아이디 값을 찾아온다.
            Origin과 비교하여 Origin에서 데이터를 찾아온다.
            Origin 해당 아이템을 삭제한다
            View에 있는 데이터를 업데이트한다.
            View를 순차적으로 업데이트한다. or 데이터를 업데이트한다.

        3.아이템 스왑
            변경할 인덱스와 Origin인덱스를 받아온다
            View에서 Swap을 해준다
            View에 있는 데이터를 업데이트한다.
        */

        private void Start()
        {
            _ItemArray = new BaseItem[InventoryData.Row * InventoryData.Column];
            _ViewItemArray = new BaseItem[InventoryData.Row * InventoryData.Column];
        }

        public void SwapItem(int originItemIndex, int swapItemIndex)
        {
            Swap(_ItemArray, originItemIndex, swapItemIndex);
            // UpdateSlotUI(originItemIndex);
            // UpdateSlotUI(swapItemIndex);
        }

        private void Swap(BaseItem[] baseItemArray, int originItemIndex, int swapItemIndex)
        {
            BaseItem temp = null;
            temp = baseItemArray[originItemIndex];
            baseItemArray[originItemIndex] = baseItemArray[swapItemIndex];
            baseItemArray[swapItemIndex] = temp;
        }

        public void AddItemData(ItemData itemData)
        {
            BaseItem[] itemArray = _ItemArray;
            BaseItem changedItem = null;
            if (itemData is CountableItemData countableItemData)
            {
                for (int i = 0; i < itemArray.Length; i++)
                {
                    if (itemArray[i] == null)
                    {
                        //새롭게 아이템 추가
                        int index = GetEmptyItemIndex();
                        if (index == -1)
                        {
                            //Popup
                            Debug.Log("There isn't empty item");
                            break;
                        }

                        CountableItem cItem = countableItemData.CreateItem() as CountableItem;
                        itemArray[index] = cItem;
                        changedItem = cItem;
                        break;
                    }

                    CountableItem item = itemArray[i] as CountableItem;
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
                            changedItem = item;
                            break;
                        }
                    }
                }
            }
            else if (itemData is EquipmentData equipmentData)
            {
                for (int i = 0; i < itemArray.Length; i++)
                {
                    if (itemArray[i] == null)
                    {
                        itemArray[i] = equipmentData.CreateItem() as EquipementItem;
                        changedItem = itemArray[i];
                        break;
                    }
                }
            }

            if (changedItem == null)
            {
                return;
            }

            UpdateViewItemArray();

            for (int i = 0; i < _ViewItemArray.Length; i++)
            {
                if(_ViewItemArray[i] == null)
                {
                    continue;
                }

                UpdateSlotUI(i);
            }

            //ChangeInventoryType();
        }

        public void ChangeInventoryType()
        {
            UpdateViewItemArray();
            SetItemSlotData();
        }

        private void SetItemSlotData()
        {
            for (int i = 0; i < _ViewItemArray.Length; i++)
            {
                UpdateSlotUI(i);
            }
        }

        private void UpdateViewItemArray()
        {
            //View에 있는 데이터를 삭제한다.
            //삭제한 다음 현재 인벤토리 타입에 맞게 데이터를 새롭게 추가한다.

            Array.Clear(_ViewItemArray, 0, _ViewItemArray.Length);

            switch (_InventoryUI.InventoryType)
            {
                case InventoryType.All:
                    _ItemArray.CopyTo(_ViewItemArray, 0);
                    break;
                case InventoryType.Weapon:
                    AddViewItemArray<WeapornItem>();
                    break;
                case InventoryType.Armor:
                    AddViewItemArray<ArmorItem>();
                    break;
                case InventoryType.Portion:
                    AddViewItemArray<PortionItem>();
                    break;
                case InventoryType.ACC:
                    AddViewItemArray<ACCItem>();
                    break;
            }
        }

        private void AddViewItemArray<T>() where T : BaseItem
        {
            for (int i = 0; i < _ItemArray.Length; i++)
            {
                T item = _ItemArray[i] as T;
                if (item == null)
                {
                    continue;
                }

                for (int j = 0; j < _ViewItemArray.Length; j++)
                {
                    if (_ViewItemArray[j] == null)
                    {
                        _ViewItemArray[j] = item;
                        break;
                    }
                }
            }
        }

        private void UpdateSlotUI(int index)
        {
            Debug.Log(index);
            BaseItem baseItem = _ViewItemArray[index];
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
                    _InventoryUI.SetItemLevel(index, 0);
                }
                else if (baseItem is EquipementItem equipementItem)
                {
                    Debug.Log(equipementItem.Level);
                    _InventoryUI.SetItemLevel(index, equipementItem.Level);
                    _InventoryUI.SetCount(index, 0);
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
            BaseItem baseItem = _ViewItemArray[index];
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
                    for(int i = 0; i < _ItemArray.Length; i++)
                    {
                        if(_ItemArray[i] == null)
                        {
                            continue;
                        }

                        if (_ItemArray[i].ItemData.Id == baseItem.ItemData.Id)
                        {
                            _ItemArray[i] = baseItem;
                        }
                    }
                    
                    UpdateSlotUI(index);
                }
            }
        }

        public void RemoveItemData(int index, int amount = 1)
        {
            BaseItem item = _ViewItemArray[index];

            for (int i = 0; i < _ItemArray.Length; i++)
            {
                if (_ItemArray[i] == item)
                {
                    _ItemArray[i] = null;
                }
            }

            UpdateViewItemArray();
            SetItemSlotData();
        //    UpdateSlotUI(index);
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