using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Sirenix.OdinInspector;
using System;
using UnityEngine.UI;

namespace ItemInventory
{
    public class InventoryUI : MonoBehaviour
    {

        [OnValueChanged(nameof(OnItemSlotSizeChanged))]
        [Range(0, 1)]
        [SerializeField] private float _ItemSlotRatio = 1;
        [SerializeField] private int _ItemListCount;

        [SerializeField] private RectTransform _InventoryHeaderTransform;
        [SerializeField] private RectTransform _ItemListParent;
        [SerializeField] private RectTransform _ItemListPanel;
        [SerializeField] private ItemSlotUI _ItemObjectPrefab;
        [SerializeField] private Text _CurrentIndexText;
        [SerializeField] private Button _ItemListMoveUpButton;
        [SerializeField] private Button _ItemListMoveDownButton;
        [SerializeField] private Inventory _Inventory;
        [SerializeField] private DragableItem _DragableItem;


        private Vector3 _BeginMovePos = Vector3.zero;
        private Vector2 _OriginInventoryRectPos = Vector2.zero;
        private List<ItemSlotUI> _ItemSlotUIList = new List<ItemSlotUI>();
        private ReactiveProperty<int> _CurItemListIndex = new ReactiveProperty<int>();

        public void Open()
        {
            Observable.FromCoroutine<Vector3>((observer) => TransformScaleCor(observer, true))
                        .Subscribe((v) => transform.localScale = v)
                        .AddTo(gameObject);
        }

        public void Close()
        {
            Observable.FromCoroutine<Vector3>((observer) => TransformScaleCor(observer, false))
                        .Subscribe((v) => transform.localScale = v)
                        .AddTo(gameObject);
        }

        public void RemoveItem(int index)
        {
            _Inventory.RemoveItemData(index);
        }

        public void RemoveSlotUI(int index)
        {
            ItemSlotUI slotUI = _ItemSlotUIList[index];
            if (slotUI == null)
            {
                Debug.Log("There isn't slot UI");
                return;
            }

            slotUI.RemoveItem();
        }

        ///ItemData와 직접적인 접촉은 없도록 한다.
        public void SetItemIcon(int index, Sprite sprite)
        {
            ItemSlotUI slotUI = _ItemSlotUIList[index];
            if (slotUI == null)
            {
                Debug.Log("There isn't slot UI");
                return;
            }

            slotUI.SetIcon(sprite);
        }

        public void SetCount(int index, int count)
        {
            ItemSlotUI slotUI = _ItemSlotUIList[index];
            if (slotUI == null)
            {
                Debug.Log("There isn't slot UI");
                return;
            }

            slotUI.SetCount(count);
        }

        public void SetItemLevel(int index, int level)
        {
            ItemSlotUI slotUI = _ItemSlotUIList[index];
            if (slotUI == null)
            {
                Debug.Log("There isn't slot UI");
                return;
            }

            slotUI.SetItemLevel(level);
        }

        private IEnumerator TransformScaleCor(IObserver<Vector3> observer, bool isUp)
        {
            float elapsed = 0;
            float duration = 0.3f;
            Vector3 start = isUp ? Vector3.zero : Vector3.one;
            Vector3 end = isUp ? Vector3.one : Vector3.zero;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                Vector3 value = Vector3.Lerp(start, end, elapsed / duration);
                observer.OnNext(value);
                yield return null;
            }
        }

        private void SetCurItemListIndexText(int curIndex)
        {
            _CurrentIndexText.text = curIndex.ToString() + "/" + _ItemListCount.ToString();
        }

        private void OnMoveUpItemSlotList()
        {
            ChangeItemSlotList(false);
        }

        private void OnMoveDownItemSlotList()
        {
            ChangeItemSlotList(true);
        }

        [Button]
        private void ChangeItemSlotList(bool isDown)
        {
            if (_ItemSlotUIList.Count == 0)
            {
                return;
            }

            if ((!isDown && _CurItemListIndex.Value == 1) || (isDown && _CurItemListIndex.Value == _ItemListCount))
            {
                return;
            }

            int row = InventoryData.Row;
            float contentPadding = InventoryData.ContentPadding;
            float height = _ItemListPanel.sizeDelta.y - contentPadding * (row / 2);
            bool isChangeNextItemList = height < _ItemListParent.sizeDelta.y;

            if (!isChangeNextItemList)
            {
                return;
            }

            _CurItemListIndex.Value = isDown ? _CurItemListIndex.Value + 1 : _CurItemListIndex.Value - 1;

            float posY = _ItemListParent.anchoredPosition.y;
            float anchoredPosY = isDown ? height + posY : posY - height;

            _ItemListParent.anchoredPosition = new Vector2(_ItemListParent.anchoredPosition.x, anchoredPosY);
        }

        private void OnItemSlotSizeChanged()
        {
            if (_ItemSlotUIList.Count == 0)
            {
                return;
            }

            Vector2 sizeDelta = GetItemSlotSize();
            Vector2 newSizeDelta = Vector2.zero;

            newSizeDelta.x = sizeDelta.x * _ItemSlotRatio;
            newSizeDelta.y = sizeDelta.y * _ItemSlotRatio;

            DrawGrid(newSizeDelta);
        }

        private Vector2 GetItemSlotSize()
        {
            Vector2 rectSize = _ItemListPanel.rect.size;
            Vector2 itemContentSize = Vector2.zero;
            int column = InventoryData.Column;
            float contentPadding = InventoryData.ContentPadding;
            float contentSpace = InventoryData.ContentSpace;
            float size = rectSize.x / column - contentPadding - contentSpace;
            itemContentSize.x = size;
            itemContentSize.y = size;

            //itemSlotSize.y = rectSize.y / row - _ContentPadding - _ContentSpace; //스크롤이 추가되면서 x사이즈와 동일한 정사각형 사이즈로 변경
            return itemContentSize;
        }

        [Button]
        private void OnItemSlotChanged()
        {
            if (_ItemSlotUIList.Count != 0)
            {
                DestoryItemList();
            }

            Vector2 size = GetItemSlotSize();
            CreateItemSlot(size);
            DrawGrid(size);
            //SetItemData();
            DragableItemInit(size);
            _CurItemListIndex.Value = 1;
        }

        [Button]
        private void ClearItem()
        {
            DestoryItemList();
            _ItemListParent.anchoredPosition = new Vector2(0, 396);
        }

        public void SetDragableItem(ItemSlotUI itemSlotUI)
        {
            itemSlotUI.SetDragableItem(_DragableItem);
        }

        public void DragableItemInit(Vector2 slotSize)
        {
            _DragableItem.Init(slotSize);
        }

        public void SwapItem(ItemSlotUI swapItemSlotUI, ItemSlotUI originItemSlotUI)
        {
            int swapItemSlotUIIndex = _ItemSlotUIList.FindIndex((v) => v == swapItemSlotUI);
            int originItemSlotUIIndex = _ItemSlotUIList.FindIndex((v) => v == originItemSlotUI);

            _Inventory.SwapItem(originItemSlotUIIndex, swapItemSlotUIIndex);
        }

        public void UseItem(ItemSlotUI itemSlotUI)
        {
            int index = _ItemSlotUIList.FindIndex((v) => v == itemSlotUI);
            _Inventory.UseItem(index);
        }

        private void CreateItemSlot(Vector2 slotSize)
        {
            int row = InventoryData.Row;
            int column = InventoryData.Column;
            float contentSpace = InventoryData.ContentSpace;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    ItemSlotUI item = Instantiate(_ItemObjectPrefab, Vector3.zero, Quaternion.identity, _ItemListParent);
                    item.name = "Item : " + i + " : " + j;
                    item.Init(this);
                    _ItemSlotUIList.Add(item);
                    SetDragableItem(item);
                }
            }

            float height = (slotSize.y + contentSpace) * row + contentSpace;
            Vector2 value = new Vector2(_ItemListParent.sizeDelta.x, height);
            _ItemListParent.sizeDelta = value;
            _ItemListCount = Mathf.CeilToInt(_ItemListParent.sizeDelta.y / _ItemListPanel.sizeDelta.y);
        }

        private void DrawGrid(Vector2 slotSize)
        {
            float contentPadding = InventoryData.ContentPadding;
            float contentSpace = InventoryData.ContentSpace;
            Vector2 startPos = new Vector2(slotSize.x * 0.5f + contentPadding, -slotSize.y * 0.5f - contentPadding);
            int row = 0;
            int column = 0;
            float spaceX = 0;
            float spaceY = 0;

            foreach (ItemSlotUI item in _ItemSlotUIList)
            {
                if (column == InventoryData.Column)
                {
                    spaceX = 0;
                    spaceY -= contentSpace;
                    column = 0;
                    row++;
                }

                float sizeX = slotSize.x * column + contentPadding + spaceX;
                float sizeY = slotSize.y * row * -1 - contentPadding + spaceY;
                RectTransform rect = item.GetComponent<RectTransform>();
                rect.sizeDelta = slotSize;
                rect.anchoredPosition = startPos + new Vector2(sizeX, sizeY);

                column++;
                spaceX += contentSpace;
            }
        }

        private void DestoryItemList()
        {
            foreach (ItemSlotUI item in _ItemSlotUIList)
            {
                Destroy(item.gameObject);
            }

            _ItemSlotUIList.Clear();
        }

        private void Start()
        {
            TryGetComponent<RectTransform>(out var rectTransform);
            _InventoryHeaderTransform = rectTransform;

            ObservableEventTrigger eventTrigger = _InventoryHeaderTransform.gameObject.AddComponent<ObservableEventTrigger>();
            eventTrigger.OnBeginDragAsObservable()
                        .Select((v) => Input.GetMouseButtonDown(0))
                        .Select((v) => Input.mousePosition)
                        .Subscribe((beginPos) =>
                        {
                            _BeginMovePos = beginPos;
                            _OriginInventoryRectPos = _InventoryHeaderTransform.anchoredPosition;
                        })
                        .AddTo(gameObject);

            eventTrigger.OnDragAsObservable()
                        .Select((v) => Input.GetMouseButtonDown(0))
                        .Select((v) => Input.mousePosition)
                        .Subscribe((v) =>
                        {
                            Vector2 movePos = v - _BeginMovePos;
                            _InventoryHeaderTransform.anchoredPosition = _OriginInventoryRectPos + movePos;
                        })
                        .AddTo(gameObject);

            _CurItemListIndex.AsObservable()
                            .Subscribe((v) => SetCurItemListIndexText(v))
                            .AddTo(gameObject);

            _ItemListMoveUpButton.OnClickAsObservable()
                                    .Subscribe((v) => OnMoveUpItemSlotList())
                                    .AddTo(gameObject);

            _ItemListMoveDownButton.OnClickAsObservable()
                                    .Subscribe((v) => OnMoveDownItemSlotList())
                                    .AddTo(gameObject);

            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.Alpha1))
                        .Subscribe((v) =>
                        {
                            OnItemSlotChanged();
                        })
                        .AddTo(gameObject);
        }
    }
}
