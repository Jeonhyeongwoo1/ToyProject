using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace ProceduralAnimation
{
    public class IKResolver : MonoBehaviour
    {
        [SerializeField] private float length = 5;
        [SerializeField] private Transform _target;

        private Vector3 oldPosition;
        private Vector3 newPosition;

        // private void CheckDistFromGround()
        // {
        //     Ray ray = new Ray(transform.position, Vector3.down);
        //     if (Physics.Raycast(ray, out var hitInfo, rayDist, LayerMask.GetMask("Ground")))
        //     {
        //         float posY = transform.position.y - hitInfo.transform.position.y;
        //         if (posY > threshold)
        //         {
        //             //꽤 높은 곳에 위치한다면 더 내려오도록 한다.
        //             //transform.position = 



        //         }
        //     }
        // }

        private bool isMoving = false;

        private async UniTaskVoid PerfromStepAsync()
        {
            try
            {
                if (isMoving)
                {
                    return;
                }

                isMoving = true;
                for (float i = 0; i < 0.5f; i += Time.deltaTime)
                {
                    Vector3 newPos = transform.up * Mathf.Sin(i * length);
                    newPosition += newPos;
                    _target.localPosition = newPosition;
                    await UniTask.Yield(this.GetCancellationTokenOnDestroy());
                }

                isMoving = false;
                _target.localPosition = Vector3.zero;
            }
            catch (System.Exception e)
            {

            }
        }

        private void Start()
        {
            oldPosition = transform.position;
            newPosition = transform.position;
        }

        private void FixedUpdate()
        {
            newPosition = transform.position;
            if (newPosition != oldPosition)
            {
                oldPosition = newPosition;
               //PerfromStepAsync().Forget();
            }
        }
    }
}
