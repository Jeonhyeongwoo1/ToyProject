using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FormMover : MonoBehaviour
{

    [SerializeField] private RectTransform _TargetTransform;
    [SerializeField] private Vector2 _StartPos = new Vector2(0, 1081);
    [SerializeField] private Vector2 _EndPos = Vector2.zero;
    [SerializeField] private float _Duration = 0.3f;

    private void Awake()
    {
        if (_TargetTransform == null)
        {
            TryGetComponent<RectTransform>(out var rectTransform);
            _TargetTransform = rectTransform;
        }
    }

    public void Move()
    {
        _TargetTransform.anchoredPosition = _StartPos;

        if (!_TargetTransform.gameObject.activeSelf)
        {
            _TargetTransform.gameObject.SetActive(true);
        }

        _TargetTransform.SetAsLastSibling();
        _TargetTransform.DOAnchorPos(Vector2.zero, _Duration)
                        .SetEase(Ease.Linear);
    }
}
