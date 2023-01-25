using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Roulette
{
    public class Roulette : MonoBehaviour
    {
        [SerializeField] private ScrollRect _ScrollRect;

        [SerializeField] private float _Threshold = 100;
        [SerializeField] private float _DisableMargin;
        [SerializeField] private float _OffsetY;
        [SerializeField] private List<RectTransform> _ItemList = new List<RectTransform>();

        /*
            1. 
        */

        private void Start()
        {
            _ScrollRect.onValueChanged.AddListener(OnScroll);

            int childCount = _ScrollRect.content.childCount;
            for (int i = 0; i < childCount; i++)
            {
                RectTransform rect = _ScrollRect.content.GetChild(i).GetComponent<RectTransform>();
                _ItemList.Add(rect);
            }

            float axisY = _ItemList[0].anchoredPosition.y - _ItemList[1].anchoredPosition.y;
            if (axisY < 0)
            {
                axisY *= -1;
            }

            _OffsetY = axisY;
            _DisableMargin = _OffsetY * childCount / 2;

        }

        private Vector2 itemPos;
        private void OnScroll(Vector2 value)
        {
            for (int i = 0; i < _ItemList.Count; i++)
            {
                RectTransform item = _ItemList[i];
                //맨 밑의 경우 child 0으로 변경한다
                if (i == 0)
                {
                    Debug.Log(_ScrollRect.transform.InverseTransformPoint(item.gameObject.transform.position).y);
                }

                float posY = _ScrollRect.transform.InverseTransformPoint(item.position).y;
                if (posY < -_DisableMargin)
                {
                    float newPosY = item.anchoredPosition.y;
                    newPosY += _OffsetY * _ItemList.Count;
                    item.anchoredPosition = new Vector2(item.anchoredPosition.x, newPosY);
                    _ScrollRect.content.GetChild(0).transform.SetAsLastSibling();
                }

                if (posY > _DisableMargin + _Threshold)
                {
                    float newPosY = item.anchoredPosition.y;
                    newPosY -= _OffsetY * _ItemList.Count;
                    item.anchoredPosition = new Vector2(item.anchoredPosition.x, newPosY);
                    _ScrollRect.content.GetChild(_ItemList.Count - 1).SetAsFirstSibling();
                }

            }
        }

    }
}
