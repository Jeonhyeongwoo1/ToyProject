using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace Roulette
{
    public class RouletteTestManager : MonoBehaviour
    {

        public enum TitleType
        {
            Test1,
            Test2,
            Test3,
            Test4,
            Test5,
            Test6,
            Test7,
            Test8
        }

        [Serializable]
        public class RouletteData
        {
            public TitleType titleType;
            public Color color;
        }

        [SerializeField] private Roulette _Roulette;
        [SerializeField] private RouletteData[] _RoullettData;
        [SerializeField] private TitleType _SelectionItemTitleType;

        private int currentItemIndex;

        public (string, Color) GetRouletteData()
        {
            currentItemIndex++;
            currentItemIndex = currentItemIndex % _RoullettData.Length;

            RouletteData data = _RoullettData[currentItemIndex];
            if (data.titleType == _SelectionItemTitleType)
            {
                currentItemIndex++;
                currentItemIndex = currentItemIndex % _RoullettData.Length;
            }

            return (_RoullettData[currentItemIndex].titleType.ToString(), _RoullettData[currentItemIndex].color);
        }

        private void Start()
        {
            for (int i = 0; i < _RoullettData.Length; i++)
            {
                _RoullettData[i].color = UnityEngine.Random.ColorHSV(0, 1);
            }
        }

        private RouletteData GetRouletteData(TitleType titleType)
        {
            for (int i = 0; i < _RoullettData.Length; i++)
            {
                if (_RoullettData[i].titleType == titleType)
                {
                    return _RoullettData[i];
                }
            }

            return null;
        }

        [Button]
        public void InitRoulletteData()
        {
            int count = _Roulette.RollingItemCount;
            for (int i = 0; i < count; i++)
            {
                if (i == _Roulette.SelectionItemIndex)
                {
                    RouletteData data = GetRouletteData(_SelectionItemTitleType);
                    if (data == null)
                    {
                        Debug.LogError("There isn't roulletteData");
                        return;
                    }

                    _Roulette.UpdateRoulletteItem(i, data.titleType.ToString(), data.color);
                    continue;
                }
                Debug.Log(_RoullettData[i].titleType.ToString());
                _Roulette.UpdateRoulletteItem(i, _RoullettData[i].titleType.ToString(), _RoullettData[i].color);
            }
        }
    }
}