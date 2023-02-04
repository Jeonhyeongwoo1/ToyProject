using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using DG.Tweening;

namespace UniTaskExample
{
    public class TestController : MonoBehaviour
    {
        [SerializeField] Transform _Target;

        private bool isStop = false;

        async private void Start()
        {
            //await Sample1(this.GetCancellationTokenOnDestroy());
            Sample2(this.GetCancellationTokenOnDestroy()).Forget();
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
