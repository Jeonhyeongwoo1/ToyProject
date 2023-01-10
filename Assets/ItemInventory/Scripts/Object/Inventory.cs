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

                bool isAddItem = false;
                for (int i = 0; i < _ItemArray.Length; i++)
                {
                    CountableItem item = _ItemArray[i] as CountableItem;
                    if (item == null)
                    {
                        continue;
                    }

                    if (item.countableItemData.Id == countableItemData.Id)
                    {
                        int count = item.CurAmount + countableItemData.Count;
                        if (count > item.MaxAmount)
                        {
                            //새롭게 아이템 추가
                            int index = GetEmptyItemIndex();
                            if (index == -1)
                            {
                                Debug.Log("There isn't empty item");
                                break;
                            }

                            BaseItem baseItem = new BaseItem(countableItemData);
                            _ItemArray[index] = baseItem;
                            isAddItem = true;
                            break;
                        }
                        else
                        {
                            //해당 아이템에 추가
                            item.AddAmount(countableItemData.Count);
                            isAddItem = true;
                            break;
                        }
                    }
                }

                if (!isAddItem)
                {
                    //새롭게 아이템 추가
                    int index = GetEmptyItemIndex();
                    if (index == -1)
                    {
                        Debug.Log("There isn't empty item");
                        return;
                    }

                    BaseItem item = new BaseItem(countableItemData);
                    _ItemArray[index] = item;
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