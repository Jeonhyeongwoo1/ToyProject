using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScrollViewPanel : MonoBehaviour
{
    [SerializeField] private Vector2 _MaxElementSize;
    [SerializeField] private Vector2 _MinElementSize;
    [SerializeField] private Vector2 _ElementSpace;

    [SerializeField] private List<RectTransform> _TargetList;
    [SerializeField] private float[] _LookupXTable; //X 좌표값
    [SerializeField] private Vector2[] _LookupSizeTable; //Rect size

    [SerializeField] private int _TargetCount;
    [SerializeField] private int _CurrentIndex; //첫 번째는 중앙에 있는것
    [SerializeField] private int _CenterIndex;
    [SerializeField] private float _TransitionTime;

    private const int LEFT = 1;
    private const int RIGHT = -1;

    private void Start()
    {
        int childCount = transform.childCount;
        _TargetList = new List<RectTransform>(childCount);

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.TryGetComponent<RectTransform>(out var rectTransform);
            _TargetList.Add(rectTransform);
        }

        _TargetCount = childCount + 2;

        _LookupXTable = new float[_TargetCount];
        _LookupSizeTable = new Vector2[_TargetCount];
        _CenterIndex = _TargetCount / 2;

        SetLookupXTable();
        SetLookupSizeTable();
        InitRectTransform();
    }

    private float _Prograss;
    private int _Direction;
    private bool _IsTransition;

    private void Update()
    {
        if (!_IsTransition)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _Direction = LEFT;
                _IsTransition = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _Direction = RIGHT;
                _IsTransition = true;
            }
        }
        else
        {
            _Prograss += Time.deltaTime / _TransitionTime;

            OnMove();

            if(_Prograss >= 1)
            {
                _Prograss = 0;
                _IsTransition = false;
                _CurrentIndex = (_CurrentIndex - _Direction) % _TargetList.Count;
                if(_CurrentIndex <= 0)
                {
                    _CurrentIndex += _TargetList.Count;
                }

                //End
                OnEndMove();
            }

        }

    }

    private void OnEndMove()
    {
        for (int i = 0; i < _TargetList.Count; i++)
        {
            int index = GetLookupIndex(i);
            Vector2 size = _LookupSizeTable[index];
            float pos = _LookupXTable[index];
            SetSizeAndPosition(_TargetList[i], size, pos);
        }
    }

    private void InitRectTransform()
    {
        for (int i = 0; i < _TargetList.Count; i++)
        {
            int index = GetLookupIndex(i);
            Debug.Log(index);
            SetSizeAndPosition(_TargetList[i], _LookupSizeTable[index], _LookupXTable[index]);
        }
    }

    private int GetLookupIndex(int index)
    {
        //Center 기준으로 설정한다
        index = index - _CurrentIndex + _CenterIndex;
        int targetCount = _TargetList.Count;
        if (index > targetCount)
        {
            index -= targetCount;
        }
        else if (index <= 0)
        {
            index += targetCount;
        }

        return index;
    }

    private void SetSizeAndPosition(RectTransform target, Vector2 size, float pos)
    {
        target.sizeDelta = size;
        target.anchoredPosition = new Vector2(pos, target.anchoredPosition.y);
    }

    private void SetLookupSizeTable()
    {
        for (int i = 0; i < _TargetCount; i++)
        {
            float absGap = GetDiffFromCenterIndex(i);
            Vector2 value = Vector2.Lerp(_MaxElementSize, _MinElementSize, Mathf.Abs(i - _CenterIndex) / (float)_CenterIndex);
            _LookupSizeTable[i] = value;
        }
    }

    private void SetLookupXTable()
    {
        /*
            0 : 맨 마지막
            1 : 맨 마지막 - 1
        */
        for (int i = 0; i < _TargetCount; i++)
        {
            float absGap = GetDiffFromCenterIndex(i);

            float pos = 0;
            float space = _ElementSpace.x * absGap;

            pos += space; // 여백의 합

            //사이즈의 합

            if (i == _CenterIndex)
            {
                _LookupXTable[i] = 0;
                continue;
            }

            for (int j = 0; j <= absGap; j++)
            {
                float w = Vector2.Lerp(_MaxElementSize, _MinElementSize, j / absGap).x;

                if (j > 0 && j <= absGap)
                {
                    pos += w;
                }
                else
                {
                    pos += w * 0.5f;
                }
            }

            if (i < _CenterIndex)
            {
                pos *= -1;
            }

            _LookupXTable[i] = pos;
        }
    }

    private int GetDiffFromCenterIndex(int index)
    {
        return Mathf.Abs(index - _CenterIndex);
    }

    private void OnMove()
    {
        for (int i = 0; i < _TargetList.Count; i++)
        {
            int index = GetLookupIndex(i);
            Vector2 curSize = _LookupSizeTable[index];
            Vector2 nextSize = _LookupSizeTable[index + _Direction];
            Vector2 size = Vector2.Lerp(curSize, nextSize, _Prograss);

            float curXPos = _LookupXTable[index];
            float nextXPos = _LookupXTable[index + _Direction];
            float pos = Mathf.Lerp(curXPos, nextXPos, _Prograss);

            SetSizeAndPosition(_TargetList[i], size, pos);
        }
    }
}
