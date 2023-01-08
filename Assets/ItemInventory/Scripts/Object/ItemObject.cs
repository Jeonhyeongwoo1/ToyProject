using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

namespace ItemInventory
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private Image _ItemImage;
        [SerializeField] private Text _CountText;
        [SerializeField] private Text _ItemLevelText;

        private DragableItem _DragableItem;
        private Vector2 _OnBeginMousePos;
        private ItemTestData.TestData _ItemData;

        public bool HasItem() => _ItemData != null;
        public ItemTestData.TestData GetItemData() => _ItemData;

        public void SetItemData(ItemTestData.TestData itemData)
        {
            if (itemData == null)
            {
                _CountText.gameObject.SetActive(false);
                _ItemLevelText.gameObject.SetActive(false);
                _ItemImage.sprite = null;
                _ItemData = null;
                return;
            }

            if (itemData.count == 0)
            {
                _CountText.gameObject.SetActive(false);
            }
            else
            {
                _CountText.text = itemData.count.ToString();
                _CountText.gameObject.SetActive(true);
            }

            if (string.IsNullOrEmpty(itemData.level))
            {
                _ItemLevelText.gameObject.SetActive(false);
            }
            else
            {
                _ItemLevelText.text = "LV." + itemData.level;
                _ItemLevelText.gameObject.SetActive(true);
            }

            _ItemImage.sprite = itemData.sprite;
            _ItemData = itemData;
        }

        public void SetDragableItem(DragableItem dragableItem)
        {
            _DragableItem = dragableItem;
        }

        public void Init(ItemTestData.TestData itemData)
        {
            SetItemData(itemData);
        }

        private void Start()
        {
            TryGetComponent<RectTransform>(out var rectTransform);

            ObservableEventTrigger eventTrigger = gameObject.AddComponent<ObservableEventTrigger>();
            eventTrigger.OnBeginDragAsObservable()
                        .Select((v) => Input.GetMouseButtonDown(0))
                        .Select((v) => Input.mousePosition)
                        .Subscribe((v) =>
                        {
                            if (!HasItem())
                            {
                                return;
                            }

                            _OnBeginMousePos = v;
                            _DragableItem.OnBeginDrag(_ItemImage, _OnBeginMousePos);
                        });

            eventTrigger.OnDragAsObservable()
                        .Select((v) => Input.GetMouseButtonDown(0))
                        .Select((v) => Input.mousePosition)
                        .Subscribe((v) =>
                        {
                            if (!HasItem())
                            {
                                return;
                            }

                            Vector2 value = (Vector2)v - _OnBeginMousePos;
                            _DragableItem.OnDrag(value);
                        });

            eventTrigger.OnEndDragAsObservable()
                        .Select((v) => Input.GetMouseButtonDown(0))
                        .Select((v) => Input.mousePosition)
                        .Subscribe((v) =>
                        {
                            if (!HasItem())
                            {
                                return;
                            }

                            _DragableItem.OnEndDrag();
                        });

            eventTrigger.OnDropAsObservable()
                        .Subscribe((v) => ChangeItemSlotData(v.pointerDrag));
        }

        private void ChangeItemSlotData(GameObject pointerDragObject)
        {
            ItemObject draggedItem = pointerDragObject.GetComponent<ItemObject>();
            var itemData = draggedItem.GetItemData();
            if (itemData == null)
            {
                return;
            }

            draggedItem.SetItemData(_ItemData);
            SetItemData(itemData);
            _DragableItem.OnEndDrag();
        }
    }
}
