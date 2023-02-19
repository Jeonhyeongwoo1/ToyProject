using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Obstacle
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 _RotateDir;
        [SerializeField] private float _Speed;

        private CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();

        private void OnEnable()
        {
            RotateAsync().Forget();
        }

        private async UniTaskVoid RotateAsync()
        {
            try
            {
                while (true)
                {
                    transform.Rotate(_RotateDir * _Speed, Space.Self);
                    await UniTask.Yield(_CancellationTokenSource.Token);
                }
            }
            catch (Exception e)
            {
                Debug.Log("Cancel Rotate async" + e);
            }
        }

        private void OnDisable()
        {
            if (_CancellationTokenSource == null)
            {
                return;
            }

            _CancellationTokenSource.Cancel();
            _CancellationTokenSource.Dispose();
        }
    }
}