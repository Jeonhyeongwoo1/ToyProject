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
        [SerializeField] private List<RectTransform> _ItemList = new List<RectTransform>();
        [SerializeField] private List<RollingData> _RollingData = new List<RollingData>();

        public enum DirectionType
        {
            LEFT,
            RIGHT
        }

        [Serializable]
        public class RollingData
        {
            public DirectionType directionType;
            public int count;
            public float duration;
            public float delay;
            public AnimationCurve animationCurve;

            public bool useHook;
            public float hookRatio; //Hook은 맨마지막 단계에서 발생
            [HideInInspector] public float hookCorrection;
        }

        [SerializeField] private RectTransform _SelectedItem;

        //미리 선정된 아이템을 뽑아내자.
        private void GetSelectedItem()
        {
            /*
                1.좌우 이동에 의해서 최종적 이동되는 값을 찾아낸다.
                2.total에서 몇번째 인덱스인지 확인한다
            */

            bool isRight = false;
            int rightCount = 0;
            int leftCount = 0;

            for (int i = 0; i < _RollingData.Count; i++)
            {
                if (_RollingData[i].directionType == DirectionType.RIGHT)
                {
                    rightCount += _RollingData[i].count;
                }
                else if (_RollingData[i].directionType == DirectionType.LEFT)
                {
                    leftCount += _RollingData[i].count;
                }
            }

            isRight = rightCount > leftCount;
            int totalCount = Mathf.Abs(rightCount - leftCount);
            int index = (totalCount) % (_ItemList.Count);
            int selectIndex = _ItemList.Count - index;

            _SelectedItem = _ItemList[selectIndex];
        }

        private void Start()
        {
            _ScrollRect.onValueChanged.AddListener(OnScroll);

            int childCount = _ScrollRect.content.childCount;
            for (int i = 0; i < childCount; i++)
            {
                RectTransform rect = _ScrollRect.content.GetChild(i).GetComponent<RectTransform>();
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.anchorMin = new Vector2(0.5f, 0.5f);

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

            SetupItemPosition();
            GetSelectedItem();
        }

        private void SetupItemPosition()
        {
            for (int i = 0; i < _ItemList.Count; i++)
            {
                RectTransform item = _ItemList[i];

                int half = _ItemList.Count / 2;
                if (i <= half)
                {
                    item.anchoredPosition = new Vector2(_Offset * i, 0);
                }
                else if (i > half)
                {
                    int value = (_ItemList.Count - i) * -1;
                    item.anchoredPosition = new Vector2(_Offset * value, 0);
                }
                else
                {
                    item.anchoredPosition = Vector2.zero;
                }
            }
        }

        private void OnEnable()
        {
            _ScrollRect.content.anchoredPosition = Vector2.zero;
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
                RollingData data = _RollingData[i];
                int dir = data.directionType == DirectionType.RIGHT ? -1 : 1;
                float endPosX = _ScrollRect.content.anchoredPosition.x + data.count * _Offset * dir;
                float startPosX = _ScrollRect.content.anchoredPosition.x;

                if (data.useHook)
                {
                    float delta = _Offset * data.hookRatio;
                    endPosX += dir == -1 ? delta : -delta;

                    if (i != _RollingData.Count - 1)
                    {
                        _RollingData[i + 1].hookCorrection = _Offset * (1 - data.hookRatio);
                    }
                }

                if (data.hookCorrection != 0)
                {
                    endPosX += data.hookCorrection;
                }

                float elapsed = 0;
                float duration = data.duration;
                AnimationCurve animationCurve = data.animationCurve;
                Vector2 pos = Vector2.zero;
                pos.y = _ScrollRect.content.anchoredPosition.y;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    float value = Mathf.LerpUnclamped(startPosX, endPosX, animationCurve.Evaluate(elapsed / duration));
                    pos.x = value;
                    _ScrollRect.content.anchoredPosition = pos;
                    yield return null;
                }

                if (!data.useHook)
                {
                    yield return new WaitForSeconds(data.delay);
                }
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
