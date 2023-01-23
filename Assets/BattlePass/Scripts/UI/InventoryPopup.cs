using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace BattlePass
{
    public class InventoryPopup : BasePopup
    {
        [SerializeField] private Button _CloseButton;
        [SerializeField] private RectTransform _ItemContentTransform;
        [SerializeField] private Transform _InventoryItemPrefab;
        [SerializeField] private Transform _InventoryPopupTransform;

        private List<Transform> _ItemTransformList = new List<Transform>();

        public override void OpenPopup(object data)
        {
            try
            {
                List<Sprite> itemSpriteList = (List<Sprite>)data;
                for (int i = 0; i < itemSpriteList.Count; i++)
                {
                    Transform item = Instantiate(_InventoryItemPrefab, _ItemContentTransform);
                    item.GetChild(0).TryGetComponent<Image>(out var image);
                    image.sprite = itemSpriteList[i];
                    _ItemTransformList.Add(item);
                }

            }
            catch (Exception e)
            {

            }

            base.OpenPopup(data);
            ResizeScale(_InventoryPopupTransform, true);
        }

        public override void ClosePopup()
        {
            DestroyItemList();
            ResizeScale(_InventoryPopupTransform, false, () => gameObject.SetActive(false));
        }

        private void DestroyItemList()
        {
            Transform[] childs = _ItemContentTransform.GetComponentsInChildren<Transform>();
            for (int i = 0; i < childs.Length; i++)
            {
                if (childs[i] == _ItemContentTransform)
                {
                    continue;
                }
                
                Destroy(childs[i].gameObject);
            }
        }

        private void Start()
        {
            _CloseButton.OnClickAsObservable()
                        .Subscribe((v) => ClosePopup());
        }
    }
}
