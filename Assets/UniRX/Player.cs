using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MVRP.Model
{
    public class Player : MonoBehaviour
    {
        public int MaxHealth => _MaxHealth;
        public IReadOnlyReactiveProperty<int> Health => _Health;

        private ReactiveProperty<int> _Health = new ReactiveProperty<int>(100);
        private int _MaxHealth = 100;
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent<Enemy>(out var enemy))
            {
                _Health.Value -= 10;
            }
        }

        private void OnDestroy()
        {
            _Health.Dispose();
        }
    }
}