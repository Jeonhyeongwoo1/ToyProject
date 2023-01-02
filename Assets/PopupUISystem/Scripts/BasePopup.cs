using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Events;

namespace PopupUISystem
{
    public interface IPopup
    {
        void Open();
        void Close();
    }

    public class BasePopup : MonoBehaviour, IPopup
    {
        public PopupType PopupType => _PopupType;

        [SerializeField] private Text _PopupTitleText;
        [SerializeField] private Button _CloseButton;
        [SerializeField] private Transform _HeaderTransform;
        [SerializeField] private PopupType _PopupType;

        private RectTransform _PopupRectTrasnform;
        private Vector2 _RectBegin;
        private Vector2 _MoveBegin;
        private Vector2 _MoveOffset;
        private UnityAction _OnCloseAction;
        private UnityAction _OnFocusAction;

        public void Init(UnityAction onCloseAction, UnityAction onFocusAction)
        {
            _OnCloseAction = onCloseAction;
            _OnFocusAction = onFocusAction;
        }

        // Start is called before the first frame update
        private void Start()
        {
            TryGetComponent<RectTransform>(out var rectTransform);
            _PopupRectTrasnform = rectTransform;
            _PopupTitleText.text = _PopupType.ToString();
            _CloseButton.OnClickAsObservable()
                        .Subscribe((v) =>
                        {
                            _OnCloseAction?.Invoke();
                            Close();
                        });

            ObservableEventTrigger headerTrigger = _HeaderTransform.gameObject.AddComponent<ObservableEventTrigger>();
            headerTrigger.OnBeginDragAsObservable()
                            .Select(_ => Input.GetMouseButton(0))
                            .Select(_ => Input.mousePosition)
                            .Subscribe(beginPos =>
                            {
                                _MoveBegin = beginPos;
                                _RectBegin = _PopupRectTrasnform.anchoredPosition;
                                _OnFocusAction?.Invoke();
                            })
                            .AddTo(_HeaderTransform);

            headerTrigger.OnDragAsObservable()
                            .Select(_ => Input.GetMouseButton(0))
                            .Select(_ => Input.mousePosition)
                            .Subscribe(position =>
                            {
                                _MoveOffset = (Vector2)position - _MoveBegin;
                                _PopupRectTrasnform.anchoredPosition = _RectBegin + _MoveOffset;
                            })
                            .AddTo(_HeaderTransform);

            ObservableEventTrigger trigger = _PopupRectTrasnform.gameObject.AddComponent<ObservableEventTrigger>();
            trigger.OnPointerClickAsObservable()
                    .Select((v) => Input.GetMouseButton(0))
                    .Subscribe((v) =>
                    {
                        _OnFocusAction?.Invoke();
                    })
                    .AddTo(this);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
