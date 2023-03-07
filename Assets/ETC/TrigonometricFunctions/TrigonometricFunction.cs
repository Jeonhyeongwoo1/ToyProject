using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace TrigonometricFunction
{
    public class TrigonometricFunction : MonoBehaviour
    {
        public float ViewAngle => _viewAngle;

        [SerializeField] private Transform _target;
        [SerializeField] private float _length;
        [SerializeField] private float _speed;
        [SerializeField] private float _viewAngle = 60;
        [SerializeField] private float _Interval = 5;
        [SerializeField] private Transform _circlePrefab;


        private Vector3 _targetPos;

        private void Awake()
        {
            _targetPos = _target.position;
        }

        public Vector3 DistToFromAngle(float angle, bool isGlobal)
        {
            float value = angle;
            if (!isGlobal)
            {
                value += transform.localEulerAngles.y;
            }

            //Degree를 호도법으로 변경한다.
            return new Vector3(Mathf.Sin(value * Mathf.Deg2Rad), 0, Mathf.Cos(value * Mathf.Deg2Rad));
        }

        private void Start()
        {
            Observable.EveryUpdate()
                        .Subscribe((v) =>
                        {
                            //Atan2();
                            //CircleMove();
                            //SinMover();
                        });


            //Observable.Interval(TimeSpan.FromSeconds(0.02f)).Subscribe((v) => CircleMover());
        }

        //나선형
        private void CircleMover()
        {
            Transform circle = Instantiate(_circlePrefab);
            
            float theta = Time.time;
            float angle = Mathf.PI * 2 * theta;
            circle.position = new Vector3(Mathf.Cos(angle) * theta, Mathf.Sin(angle) * theta);
           // circle.position
           // circle.GetComponent<Rigidbody2D>().velocity = transform.right *10;
        }

        private void Atan2()
        {
            var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 screenPoint = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 dir = screenPoint - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void SinMover()
        {
            float axisX = Mathf.Sin(Time.time * _speed) * _length;
            _targetPos.x = axisX;
            transform.position = _targetPos;
        }

        private void CircleMove()
        {
            float axisX = Mathf.Cos(Time.time * _speed) * _length;
            float axisY = Mathf.Sin(Time.time * _speed) * _length;

            transform.position = new Vector3(axisX, axisY);
        }

    }
}
