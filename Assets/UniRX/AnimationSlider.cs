using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MVRP.View
{
    public class AnimationSlider : MonoBehaviour
    {
        [SerializeField] private Slider _Slider;

        private void Start()
        {
            _Slider.value = 1;
        }

        public void SetValue(float value)
        {
            Debug.Log(value);
            DOTween.To(() => _Slider.value
                            , (v) => _Slider.value = v
                            , value
                            , duration: 1);
        }
    }
}
