using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;
using DG.Tweening;

namespace Roulette
{
    public class Roulette : MonoBehaviour
    {
        [SerializeField] private ScrollRect _ScrollRect;

        [SerializeField] private bool _IsVertical;
        [SerializeField] private float _Threshold = 100;
        [SerializeField] private float _DisableMargin;
        [SerializeField] private float _Offset;
        [SerializeField] private int _CardCount;
        [SerializeField] private List<RectTransform> _ItemList = new List<RectTransform>();
        [SerializeField] private List<RollingData> _RollingData = new List<RollingData>();

        public enum DirectionType
        {
            LEFT,
            RIGHT
        }

        [Serializable]
        public struct RollingData
        {
            public DirectionType directionType;
            public int countCount;
            public float duration;
            public float delay;
            public AnimationCurve animationCurve;
        }

        private void Start()
        {
            _ScrollRect.onValueChanged.AddListener(OnScroll);

            int childCount = _ScrollRect.content.childCount;
            for (int i = 0; i < childCount; i++)
            {
                RectTransform rect = _ScrollRect.content.GetChild(i).GetComponent<RectTransform>();
                _ItemList.Add(rect);
            }

            if (_IsVertical)
            {
                float axisY = _ItemList[0].anchoredPosition.y - _ItemList[1].anchoredPosition.y;
                if (axisY < 0)
                {
                    axisY *= -1;
                }

                _Offset = axisY;
                _DisableMargin = _Offset * childCount / 2;
            }
            else
            {
                float axisX = _ItemList[0].anchoredPosition.x - _ItemList[1].anchoredPosition.x;
                if (axisX < 0)
                {
                    axisX *= -1;
                }

                _Offset = axisX;
                _DisableMargin = _Offset * childCount / 2;


            }

            _ScrollRect.content.sizeDelta = _ItemList[0].sizeDelta;

        }

        [Button]
        private void MoveTo()
        {
            StartCoroutine(MoveCor());
        }

        private IEnumerator MoveCor()
        {
            for (int i = 0; i < _RollingData.Count; i++)
            {
                bool isComplete = false;
                RollingData data = _RollingData[i];
                int dir = data.directionType == DirectionType.RIGHT ? 1 : -1;
                float movePosX = _ScrollRect.content.anchoredPosition.x + data.countCount * _Offset * dir;
                _ScrollRect.content.DOAnchorPosX(movePosX, data.duration)
                                    .SetEase(data.animationCurve)
                                    .OnComplete(() => isComplete = true);

                while (!isComplete)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(data.delay);
            }
        }

        private void OnScroll(Vector2 value)
        {
            for (int i = 0; i < _ItemList.Count; i++)
            {
                if (_IsVertical)
                {
                    RectTransform item = _ItemList[i];
                    float posY = _ScrollRect.transform.InverseTransformPoint(item.position).y;
                    if (posY < -_DisableMargin)
                    {
                        float newPosY = item.anchoredPosition.y;
                        newPosY += _Offset * _ItemList.Count;
                        item.anchoredPosition = new Vector2(item.anchoredPosition.x, newPosY);
                        _ScrollRect.content.GetChild(0).transform.SetAsLastSibling();
                    }

                    if (posY > _DisableMargin + _Threshold)
                    {
                        float newPosY = item.anchoredPosition.y;
                        newPosY -= _Offset * _ItemList.Count;
                        item.anchoredPosition = new Vector2(item.anchoredPosition.x, newPosY);
                        _ScrollRect.content.GetChild(_ItemList.Count - 1).SetAsFirstSibling();
                    }
                }
                else
                {
                    RectTransform item = _ItemList[i];
                    float posX = _ScrollRect.transform.InverseTransformPoint(item.position).x;
                    if (posX < -_DisableMargin)
                    {
                        float newPosX = item.anchoredPosition.x;
                        newPosX += _Offset * _ItemList.Count;
                        item.anchoredPosition = new Vector2(newPosX, item.anchoredPosition.y);
                        _ScrollRect.content.GetChild(0).transform.SetAsLastSibling();
                    }

                    if (posX > _DisableMargin + _Threshold)
                    {
                        float newPosX = item.anchoredPosition.x;
                        newPosX -= _Offset * _ItemList.Count;
                        item.anchoredPosition = new Vector2(newPosX, item.anchoredPosition.y);
                        _ScrollRect.content.GetChild(_ItemList.Count - 1).SetAsFirstSibling();
                    }
                }

            }
        }
    }
}
