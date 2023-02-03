using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IndicatorUI
{
    public class Indicator : MonoBehaviour
    {
        enum DirectionType
        {
            None,
            Left,
            Right,
            Up,
            Down
        }

        [SerializeField] private Transform _TargetTransform;
        [SerializeField] private float _DefaultAngle;
        [SerializeField] private DirectionType _DirectionType;
        [SerializeField] private RectTransform _IndicatorTransform;

        private Camera _MainCamera;

        private void Start()
        {
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            _DefaultAngle = Vector2.Angle(transform.up, screenSize);
            Debug.DrawRay(transform.position, transform.up, Color.red);

            _MainCamera = Camera.main;
        }

        private void Update()
        {
            float angle = CalculateAngleFromTarget();
            //  Debug.Log(angle);
            _DirectionType = GetDirectionType(angle);

            if (_DirectionType == DirectionType.None)
            {
                return;
            }

            Vector2 targetPosition = _MainCamera.WorldToViewportPoint(_TargetTransform.position);
            Vector2 mePosition = _MainCamera.WorldToViewportPoint(transform.position);
            Vector2 anchorMin = Vector2.zero;
            Vector2 anchorMax = Vector2.zero;

            float newX = 0;
            float newY = 0;

            switch (_DirectionType)
            {
                case DirectionType.Up:
                    newY = 0.96f;
                    newX = (newY * targetPosition.x) / targetPosition.y;

                    anchorMax.x = newX;
                    anchorMax.y = newY;

                    if (anchorMax.x <= 0.04f) { anchorMax.x = 0.04f; }
                    else if (anchorMax.x >= 0.96f) { anchorMax.x = 0.96f; }

                    break;
                case DirectionType.Right:
                    newX = 0.96f;
                    newY = (newX * targetPosition.y) / targetPosition.x;

                    anchorMax.x = newX;
                    anchorMax.y = newY;

                    if (anchorMax.y <= 0.04f) { anchorMax.y = 0.04f; }
                    else if (anchorMax.y >= 0.96f) { anchorMax.y = 0.96f; }
                    break;
                case DirectionType.Left:
                    newX = 0.04f;
                    newY = ((1 - newX) * targetPosition.y) / (1 - targetPosition.x);

                    anchorMax.x = newX;
                    anchorMax.y = newY;
                    if (anchorMax.y <= 0.04f) { anchorMax.y = 0.04f; }
                    else if (anchorMax.y >= 0.96f) { anchorMax.y = 0.96f; }
                    break;
                case DirectionType.Down:
                    newY = 0.04f;
                    newX = ((1 - newY) * targetPosition.x) / (1 - targetPosition.y);

                    anchorMax.x = newX;
                    anchorMax.y = newY;

                    if (anchorMax.x <= 0.04f) { anchorMax.x = 0.04f; }
                    else if (anchorMax.x >= 0.96f) { anchorMax.x = 0.96f; }

                    break;
            }

            _IndicatorTransform.anchorMin = anchorMax;
            _IndicatorTransform.anchorMax = anchorMax;
            _IndicatorTransform.anchoredPosition = Vector2.zero;
        }

        private float CalculateAngleFromTarget()
        {
            float signedAngle = Vector2.SignedAngle(Vector2.up, (transform.position - _TargetTransform.position));
            return signedAngle;
        }

        private DirectionType GetDirectionType(float targetAngle)
        {
            if (-_DefaultAngle < targetAngle && _DefaultAngle > targetAngle)
            {
                return DirectionType.Down;
            }
            else if (_DefaultAngle < targetAngle && 180 - _DefaultAngle > targetAngle)
            {
                return DirectionType.Right;
            }
            else if (-_DefaultAngle > targetAngle && -(180 - _DefaultAngle) < targetAngle)
            {
                return DirectionType.Left;
            }
            else if ((-180 + _DefaultAngle > targetAngle && targetAngle > -180) ||
                    (180 > targetAngle && 180 - _DefaultAngle < targetAngle))
            {
                return DirectionType.Up;
            }

            Debug.LogError("Direction,,");
            return DirectionType.None;
        }
    }
}