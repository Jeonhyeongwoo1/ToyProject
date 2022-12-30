using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MVRP.Model
{
    public class SampleModel : MonoBehaviour
    {
        public int MaxCount => _MaxCount;
        public IReadOnlyReactiveProperty<int> Count => _Count;

        private ReactiveProperty<int> _Count = new ReactiveProperty<int>(100);

        private int _MaxCount = 100;

        public void SetCount(int value)
        {
            value = Mathf.Clamp(value, 0, _MaxCount);
            _Count.Value = value;
        }

        private void OnDestroy()
        {
            _Count.Dispose();
        }
    }
}
