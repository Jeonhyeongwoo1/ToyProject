using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

namespace RiggingController
{
    public class RiggingController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Transform _rig;
        [SerializeField] private Camera mainCam;
        [SerializeField] private Transform _root;
        [SerializeField] private Transform[] _ikTargetArray;
        [SerializeField] private float length;
        [SerializeField] private AnimationCurve lerpCurve;

        private Vector3 _moveDir;
        private bool _isMoving;

        private void SetInputValue()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            // float x = Input.GetAxis("Mouse X");
            // float y = Input.GetAxis("Mouse Y");

            _moveDir = new Vector3(h, 0, v);
            if (h != 0 || v != 0)
            {
                if (_isMoving)
                {
                    return;
                }

                UpdateIkTargetArrayFootStep();
               // StartCoroutine(UpdateIkTargetArrayFootStep());
            }
        }

        private void UpdateIkTargetArrayFootStep()
        {
            for (int i = 0; i < _ikTargetArray.Length; i++)
            {
                StartCoroutine(UpdateFootStep(_ikTargetArray[i]));
               //yield return null;
            }
        }

        private Vector3[] defaultPosArray;

        private void Start()
        {
            defaultPosArray = new Vector3[_ikTargetArray.Length];
            for (int i = 0; i < _ikTargetArray.Length; i++)
            {
                defaultPosArray[i] = _ikTargetArray[i].position;
            }
        }

        private void FixedUpdate()
        {
            Move();


        }

        private IEnumerator UpdateFootStep(Transform target)
        {
            _isMoving = true;
            float step = 10f;
            for (float j = 0; j < step; j++)
            {
                float delta = j / step;
                float x = Mathf.Lerp(0, -1, lerpCurve.Evaluate(delta));
                float y = Mathf.Lerp(0, 1, lerpCurve.Evaluate(delta));

                float sin = Mathf.Sin(delta * Mathf.PI);
                Vector3 newValue = new Vector3(-sin, sin, target.localPosition.z);
                Vector3 pos = Vector3.Lerp(target.localPosition, newValue, delta);
                target.localPosition = pos;
                yield return new WaitForFixedUpdate();
            }

            _isMoving = false;
        }

        private void Move()
        {
            Vector3 dir = mainCam.transform.TransformDirection(_moveDir);
            dir.y = 0;
            Vector3 value = Vector3.Lerp(transform.position, transform.position + dir, Time.deltaTime * speed);
            transform.position = value;

            Vector3 rotDir = new Vector3(dir.z, 0, -dir.x);
            Quaternion rot = Quaternion.Lerp(_root.rotation, Quaternion.LookRotation(rotDir), Time.deltaTime * speed);
            _root.rotation = rot;
        }

        private void Update()
        {
            SetInputValue();
        }
    }
}
