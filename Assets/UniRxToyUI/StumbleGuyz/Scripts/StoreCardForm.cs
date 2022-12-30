using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace StumbleGuyz
{
    public class StoreCardForm : MonoBehaviour
    {
        [SerializeField] private RectTransform _TargetTransform;
        [SerializeField] private Text _TitleText;

        [OnValueChanged(nameof(OnSizeDeltaChanged))]
        [SerializeField] private Vector2 _SizeDelta;

        public void Init(Vector2 sizeDelta, string title)
        {
            _TitleText.text = title;
            _SizeDelta = sizeDelta;
            OnSizeDeltaChanged();
        }

        private void OnSizeDeltaChanged()
        {
            _TargetTransform.sizeDelta = _SizeDelta;
        }
    }
}