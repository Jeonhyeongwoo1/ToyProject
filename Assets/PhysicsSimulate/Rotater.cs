using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PhysicsSimulation
{
    public class Rotater : MonoBehaviour
    {
        public bool isReal = false;

        public void DoRotate()
        {
            //RotateAsync().Forget();
            //StartCoroutine(DoRotateCor());
            Rotate();
        }

        private void FixedUpdate() {

            if(!isReal)
            {
                return;
            }
            Rotate();
        }

        private void Rotate()
        {
            Vector3 value = Vector3.forward * 30 * Time.fixedDeltaTime;
            transform.Rotate(value);
        }

        private async UniTaskVoid RotateAsync()
        {
            while(true)
            {
                transform.Rotate(Vector3.forward * 30 * Time.deltaTime);
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            }
        }

        private IEnumerator DoRotateCor()
        {
            while(true)
            {
                transform.Rotate(Vector3.forward * 10 * Time.deltaTime);
                yield return null;
            }
        }
    }
}
