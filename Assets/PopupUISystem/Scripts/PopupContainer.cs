using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace PopupUISystem
{
    public enum PopupType
    {
        Inventory,
        Equipment,
        Map
    }

    public class PopupContainer : Singleton<PopupContainer>
    {
        private LinkedList<BasePopup> _PopupLinkedList;
        private List<BasePopup> _PopupList;

        private void Start()
        {
            BasePopup[] basePopupList = GetComponentsInChildren<BasePopup>(true);
            _PopupList = new List<BasePopup>(basePopupList.Length);
            _PopupList.AddRange(basePopupList);

            _PopupLinkedList = new LinkedList<BasePopup>();

            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.I))
                        .Subscribe((v) => OpenPopup(PopupType.Inventory));

            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.E))
                        .Subscribe((v) => OpenPopup(PopupType.Equipment));

            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.M))
                        .Subscribe((v) => OpenPopup(PopupType.Map));

            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.Escape))
                        .Subscribe((v) => ClosePopup());

            Init();
        }

        private void Init()
        {
            foreach (BasePopup popup in _PopupList)
            {
                popup.Init(() => _PopupLinkedList.Remove(popup), () => ResizePopupDepth(popup));
            }
        }

        private void OpenPopup(PopupType popupType)
        {
            foreach (BasePopup popup in _PopupList)
            {
                if (popup.PopupType == popupType)
                {
                    popup.Open();
                    ResizePopupDepth(popup);
                }
            }
        }

        private void ClosePopup(PopupType popupType)
        {
            foreach (BasePopup popup in _PopupList)
            {
                if (popup.PopupType == popupType)
                {
                    popup.Close();

                    _PopupLinkedList.Remove(popup);
                }
            }
        }

        private void OnFocus(PopupType popupType)
        {
            foreach (BasePopup popup in _PopupList)
            {
                if (popup.PopupType == popupType)
                {
                    ResizePopupDepth(popup);
                }
            }
        }

        private void ResizePopupDepth(BasePopup popup)
        {
            _PopupLinkedList.Remove(popup);
            _PopupLinkedList.AddFirst(popup);
            popup.transform.SetAsLastSibling();
        }

        private void ClosePopup()
        {
            LinkedListNode<BasePopup> popup = _PopupLinkedList.First;
            popup.Value.Close();
            _PopupLinkedList.Remove(popup);
        }

    }
}
