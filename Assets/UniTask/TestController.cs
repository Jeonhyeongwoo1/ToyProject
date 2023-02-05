using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using DG.Tweening;
using System;

namespace UniTaskExample
{
    public class TestController : MonoBehaviour
    {
        [SerializeField] Transform _Target;

        private bool isStop = false;
        private Action doneCallback;
        private CancellationToken ct;

        async private void Start()
        {

            var ct = this.GetCancellationTokenOnDestroy();
        }

        private async void CancleToken()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ResizeScale(cancellationTokenSource.Token).Forget();
            await UniTask.Delay(5000);
            cancellationTokenSource.Cancel();

            if (cancellationTokenSource.IsCancellationRequested)
            {
                Debug.Log("Canceled");
            }
        }

        private async void Test()
        {
            var value = await Task.Run(() => 100);
            Debug.Log(value);

            doneCallback += UniTask.Action(async () =>
            {
                float elapsed = 0;
                float duration = 1;
                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    //Debug.Log("elapsed = " + elapsed);
                    await UniTask.Yield();
                }
            });

            var Complete = await DoMove(doneCallback);
            Debug.Log(Complete);
        }

        private async UniTask<Transform> DoMove(Action action)
        {
            float elapsed = 0;
            float duration = 1;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                Vector3 value = Vector3.Lerp(Vector3.zero, Vector3.one * 50, elapsed / duration);
                _Target.localPosition = value;
                await UniTask.Yield();
            }

            action?.Invoke();
            return _Target;
        }

        private async UniTask ResizeScale(CancellationToken cancellationToken)
        {
            int start = 0;
            int end = 1;
            while (true)
            {
                await DoScaleUp(start, end, cancellationToken);
                start = start == 0 ? 1 : 0;
                end = end == 1 ? 0 : 1;
            }
        }

        private async UniTask DoScaleUp(int start, int end,CancellationToken cancellationToken)
        {
            float elapsed = 0;
            float duration = 1;

            while (elapsed < duration)
            {
                if(cancellationToken.IsCancellationRequested)
                {
                    Debug.Log("CancleRequseted");
                }

                elapsed += Time.deltaTime;
                float value = Mathf.Lerp(start, end, elapsed / duration);
                _Target.localScale = Vector3.one * value;
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        }

        private async UniTask Sample1(CancellationToken cancellationToken)
        {
            DOVirtual.DelayedCall(3, () => isStop = true);
            Debug.Log("TEST1");
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            Debug.Log("TEST2");

            await UniTask.Yield();
            await UniTask.Delay(1000, false, PlayerLoopTiming.Update, cancellationToken);
            await UniTask.DelayFrame(0);
            await UniTask.WaitUntil(() => isStop);
            await UniTask.WaitWhile(() => !isStop);
            Debug.Log("Complete");
        }

        private Vector2[] positions = new Vector2[] {
            new Vector2(20,20),
            new Vector2(0, 0)
        };

        private async UniTaskVoid Sample2(CancellationToken cancellationToken)
        {
            int index = 0;
            while (true)
            {
                await MoveTo(_Target.position, positions[index]);
                index++;
                index = index % positions.Length;
            }
        }

        private async UniTask MoveTo(Vector3 startPosition, Vector3 endPosition)
        {
            for (float time = 0; time < 1; time += Time.deltaTime)
            {
                _Target.position = Vector3.Lerp(startPosition, endPosition, time);
                await UniTask.Yield();
            }
        }
    }
}
