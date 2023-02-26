using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class Tentacle : MonoBehaviour
    {
        [SerializeField] private int _length;
        [SerializeField] private Transform _headTransform;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _speed = 5;
        [SerializeField] private float targetDist = 0.02f;
        [SerializeField] private float _tailSpeed;
        [SerializeField] private Transform _wiggleTrasnform;
        [SerializeField] private float _wiggleSpeed;
        [SerializeField] private float _wiggleMagnitube;

        private Vector3[] _linePositionArray;
        private Vector3[] _linePositionV;

        private void Start()
        {
            _linePositionArray = new Vector3[_length];
            _linePositionV = new Vector3[_length];
            _lineRenderer.positionCount = _length;
        }

        private void Update()
        {
            _wiggleTrasnform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * _wiggleSpeed) * _wiggleMagnitube);

            _linePositionArray[0] = _headTransform.position;

            for (int i = 1; i < _linePositionArray.Length; i++)
            {
                _linePositionArray[i] = Vector3.SmoothDamp(_linePositionArray[i], _linePositionArray[i - 1] + _headTransform.right * targetDist * -1, ref _linePositionV[i], _speed + i / _tailSpeed);

            }

            _lineRenderer.SetPositions(_linePositionArray);
        }
    }
}
