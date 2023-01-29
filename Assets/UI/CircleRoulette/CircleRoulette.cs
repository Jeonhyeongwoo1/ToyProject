using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CircleRoulette
{

    [System.Serializable]
    public class RouletteData
    {
        public string id;
        public Sprite rouletteSprite;
        public string description;
        public float chance;
        public int index;
        public float weight;
    }

    public class CircleRoulette : MonoBehaviour
    {
        [SerializeField] private Transform _LinePrefab;
        [SerializeField] private CirclePiece _CirclePiecePrefab;
        [SerializeField] private Transform _LineParent;
        [SerializeField] private Transform _CirclePieceParent;
        [SerializeField] private Transform _SpinningPanel;

        [SerializeField] private RouletteData[] _RouletteData;

        [SerializeField] private int _RotateCount;
        [SerializeField] private float _RotateDuration;
        [SerializeField] private float _TargetAngle;
        [SerializeField] private AnimationCurve _RotateAnimationCurve;
        [SerializeField] private RouletteData _SelectedRouletteData;

        [SerializeField] private float _TotalWeight = 0;
        private CirclePiece _SelectedPiece;

        private void Start()
        {
            CalculateTotalWeight();
            SetRouletteData();


            CreateCircleLine();
            CreateCirclePiece();

            _TargetAngle = _SelectedPiece.transform.localEulerAngles.z;
        }

        private void CalculateTotalWeight()
        {
            float totalWeight = 0;
            for (int i = 0; i < _RouletteData.Length; i++)
            {
                if (_RouletteData[i].chance == 0)
                {
                    _RouletteData[i].chance = 1;
                }

                totalWeight += _RouletteData[i].chance;
                _RouletteData[i].weight = totalWeight;
            }

            _TotalWeight = totalWeight;
        }

        private void SetRouletteData()
        {
            float random = Random.Range(0, _TotalWeight);
            for (int i = 0; i < _RouletteData.Length; i++)
            {
                if (_RouletteData[i].weight > random)
                {
                    _SelectedRouletteData = _RouletteData[i];
                    break;
                }
            }
        }

        [Button]
        private void DoRotation()
        {
            StartCoroutine(DoRotationCor());
        }

        private IEnumerator DoRotationCor()
        {
            float elapsed = 0;
            float startAngle = _SpinningPanel.localEulerAngles.z;
            float destAngle = _RotateCount * 360 + _TargetAngle;
            Vector3 rot = _SpinningPanel.localEulerAngles;

            while (elapsed < _RotateDuration)
            {
                elapsed += Time.deltaTime;
                float value = Mathf.LerpUnclamped(startAngle, -destAngle, _RotateAnimationCurve.Evaluate(elapsed / _RotateDuration));
                rot.z = value;
                _SpinningPanel.rotation = Quaternion.Euler(rot);
                yield return null;
            }
        }

        [Button]
        private void CreateCircleLine()
        {
            int dataCount = _RouletteData.Length;
            float angle = 360 / dataCount;
            float startAngle = angle * 0.5f;

            for (int i = 0; i < dataCount; i++)
            {
                Transform line = Instantiate(_LinePrefab, _LineParent);
                line.localEulerAngles = new Vector3(0, 0, angle * i + startAngle);
            }
        }

        [Button]
        private void CreateCirclePiece()
        {
            int dataCount = _RouletteData.Length;
            float angle = 360 / dataCount;

            for (int i = 0; i < dataCount; i++)
            {
                CirclePiece piece = Instantiate(_CirclePiecePrefab, _CirclePieceParent);
                piece.transform.localEulerAngles = new Vector3(0, 0, angle * i);
                piece.Setup(_RouletteData[i].rouletteSprite, _RouletteData[i].description);

                if (_RouletteData[i].id == _SelectedRouletteData.id)
                {
                    _SelectedPiece = piece;
                }
            }
        }

    }
}
