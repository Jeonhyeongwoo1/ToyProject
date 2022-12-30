using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using MVRP.Model;
using UnityEngine.UI;

namespace MVRP.Presenter
{
    public class CountPresenter : MonoBehaviour
    {
        [SerializeField] private SampleModel _SampleModel;
        [SerializeField] private Slider _Slider;
        [SerializeField] private Text _Text;

        private void Start()
        {
            _Slider.OnValueChangedAsObservable()
                    .Subscribe((v) =>
                    {
                        int value =(int) (v * 100);
                        _SampleModel.SetCount(value);
                    });

            _SampleModel.Count.Subscribe((v) =>
            {
                float value = (float)v / _SampleModel.MaxCount;
                _Slider.value = value;
                _Text.text = $"{v}%";
            });
        }
    }
}
