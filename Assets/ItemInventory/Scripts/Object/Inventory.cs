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
    public class Inventory : MonoBehaviour
    {
        [ReadOnly][SerializeField] private List<ItemObject> _ItemList = new List<ItemObject>();

        [OnValueChanged(nameof(OnItemSlotSizeChanged))]
        [Range(0, 1)]
        [SerializeField] private float _ItemSlotRatio = 1;
        [SerializeField] private float _ContentPadding;
        [SerializeField] private float _ContentSpace;
        [SerializeField] private int row;
        [SerializeField] private int column;
        [SerializeField] private int _ItemListCount;

        [SerializeField] private ItemTestData _ItemTestData;
        [SerializeField] private RectTransform _InventoryHeaderTransform;
        [SerializeField] private RectTransform _ItemListParent;
        [SerializeField] private RectTransform _ItemListPanel;
        [SerializeField] private ItemObject _ItemObjectPrefab;
        [SerializeField] private Text _CurrentIndexText;
        [SerializeField] private Button _ItemListMoveUpButton;
        [SerializeField] private Button _ItemListMoveDownButton;

        private Vector3 _BeginMovePos = Vector3.zero;
        private Vector2 _OriginInventoryRectPos = Vector2.zero;
        private float _ItemListParentHeight;

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
            if (_ItemList.Count == 0)
            {
                return;
            }

            if ((!isDown && _CurItemListIndex.Value == 1) || (isDown && _CurItemListIndex.Value == _ItemListCount))
            {
                return;
            }

            float height = _ItemListPanel.sizeDelta.y - _ContentPadding * (row / 2);
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
            if (_ItemList.Count == 0)
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
            float size = rectSize.x / column - _ContentPadding - _ContentSpace;
            itemContentSize.x = size;
            itemContentSize.y = size;

            //itemSlotSize.y = rectSize.y / row - _ContentPadding - _ContentSpace; //스크롤이 추가되면서 x사이즈와 동일한 정사각형 사이즈로 변경
            return itemContentSize;
        }

        [Button]
        private void OnItemCountChanged()
        {
            if (_ItemList.Count != 0)
            {
                DestoryItemList();
            }

            Vector2 size = GetItemSlotSize();
            CreateItemSlot(size);
            DrawGrid(size);
            _CurItemListIndex.Value = 1;
        }

        [Button]
        private void ClearItem()
        {
            DestoryItemList();
            _ItemListParent.anchoredPosition = new Vector2(0, 396);
        }

        private void CreateItemSlot(Vector2 slotSize)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    ItemObject item = Instantiate(_ItemObjectPrefab, Vector3.zero, Quaternion.identity, _ItemListParent);
                    item.Init(slotSize);
                    _ItemList.Add(item);
                }
            }

            float height = (slotSize.y + _ContentSpace) * row + _ContentSpace;
            Vector2 value = new Vector2(_ItemListParent.sizeDelta.x, height);
            _ItemListParent.sizeDelta = value;
            _ItemListParentHeight = height;
            _ItemListCount = Mathf.CeilToInt(_ItemListParent.sizeDelta.y / _ItemListPanel.sizeDelta.y);
        }

        private void DrawGrid(Vector2 slotSize)
        {
            Vector2 startPos = new Vector2(slotSize.x * 0.5f + _ContentPadding, -slotSize.y * 0.5f - _ContentPadding);
            int row = 0;
            int column = 0;
            float spaceX = 0;
            float spaceY = 0;

            foreach (ItemObject item in _ItemList)
            {
                if (column == this.column)
                {
                    spaceX = 0;
                    spaceY -= _ContentSpace;
                    column = 0;
                    row++;
                }

                float sizeX = slotSize.x * column + _ContentPadding + spaceX;
                float sizeY = slotSize.y * row * -1 - _ContentPadding + spaceY;
                RectTransform rect = item.GetComponent<RectTransform>();
                rect.sizeDelta = slotSize;
                rect.anchoredPosition = startPos + new Vector2(sizeX, sizeY);

                column++;
                spaceX += _ContentSpace;
            }
        }

        private void DestoryItemList()
        {
            foreach (ItemObject item in _ItemList)
            {
                Destroy(item.gameObject);
            }

            _ItemList.Clear();
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
        }
    }
}
