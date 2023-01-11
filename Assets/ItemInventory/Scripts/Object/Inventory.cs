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

        public void AddItemData(ItemData itemData)
        {
            if (itemData is CountableItemData countableItemData)
            {
                /*
                1.해당 아이템이 존재하는 슬롯이 있는지 확인
                    -존재한다면
                        해당 아이템의 갯수와 새롭게 아이템이 들어가면 MAX인지 확인
                            -MAX라면 
                                새롭게 아이템을 추가
                            -MAX가 아니라면
                                해당 아이템에 추가
                    -존재하지 않는다면
                        비어있는 슬롯을 찾는다.
                            -비어있는 슬롯이 존재한다면
                                -해당 슬롯에 새롭게 아이템 추가
                            -비어있는 슬롯이 없다면
                                -아이템 추가 불가능(팝업창 띄우기?)
                */

                for (int i = 0; i < _ItemArray.Length; i++)
                {
                    CountableItem item = _ItemArray[i] as CountableItem;
                    if (item == null)
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
                    EquipementItem item = _ItemArray[i] as EquipementItem;
                    if (item == null)
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
            }
        }

        private void RemoveItemData(ItemData itemData, int amount = 1)
        {
            if (itemData is CountableItemData countableItemData)
            {
                for (int i = 0; i < _ItemArray.Length; i++)
                {

                }
            }
        }

        private int GetEmptyItemIndex()
        {
            for (int i = 0; i < _ItemArray.Length; i++)
            {
                if (_ItemArray[i].ItemData == null)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}