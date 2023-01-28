using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

namespace ItemInventory
{
    public class ItemSlotUI : MonoBehaviour
    {
        [SerializeField] private Image _ItemImage;
        [SerializeField] private Text _CountText;
        [SerializeField] private Text _ItemLevelText;

        private InventoryUI _InventoryUI;
        private DragableItem _DragableItem;
        private Vector2 _OnBeginMousePos;
        private Sprite originSprite = null;
        private bool OnMouseEnter = false;

        public void SetIcon(Sprite sprite) => _ItemImage.sprite = sprite;
        public bool HasItem() => _ItemImage.sprite != originSprite;

        public void SetCount(int count)
        {
            if (count == 0)
            {
                _CountText.gameObject.SetActive(false);
                return;
            }

            _CountText.gameObject.SetActive(true);
            _CountText.text = count.ToString();
        }

        public void SetItemLevel(int level)
        {
            if (level == 0)
            {
                _ItemLevelText.gameObject.SetActive(false);
                return;
            }

            _ItemLevelText.gameObject.SetActive(true);
            _ItemLevelText.text = "LV. " + level.ToString();
        }

        public void Init(InventoryUI inventoryUI)
        {
            _InventoryUI = inventoryUI;
        }

        public void RemoveItem()
        {
            SetIcon(originSprite);
            SetCount(0);
            SetItemLevel(0);
        }

        public void SetDragableItem(DragableItem dragableItem)
        {
            _DragableItem = dragableItem;
        }

        private void Start()
        {
            originSprite = _ItemImage.sprite;
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
                        .Subscribe((v) => OnDropItem(v.pointerDrag));

            eventTrigger.OnPointerEnterAsObservable()
                        .Subscribe((v) => OnMouseEnter = true);

            eventTrigger.OnPointerExitAsObservable()
                        .Subscribe((v) => OnMouseEnter = false);

            Observable.EveryUpdate()
                        .Where((v) => Input.GetMouseButtonDown(1))
                        .Where((v) => OnMouseEnter)
                        .Subscribe((v) => OnUseItem());
        }

        private void OnUseItem()
        {
            if (!HasItem())
            {
                return;
            }

            _InventoryUI.UseItem(this);
        }

        private void OnDropItem(GameObject pointerDragObject)
        {
            ItemSlotUI draggedItem = pointerDragObject.GetComponent<ItemSlotUI>();
            if (draggedItem != null)
            {
                _InventoryUI.SwapItem(this, draggedItem);
            }

            _DragableItem.OnEndDrag();
        }
    }
}
