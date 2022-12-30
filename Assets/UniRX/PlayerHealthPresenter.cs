using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using MVRP.Model;
using MVRP.View;

namespace MVRP.Presenter
{
    public class PlayerHealthPresenter : MonoBehaviour
    {
        [SerializeField] private MVRP.Model.Player _Player;
        [SerializeField] private AnimationSlider _animaionSlider;

        private void Start()
        {
            _Player.Health.Subscribe((v) =>
            {
                _animaionSlider.SetValue((float)v / _Player.MaxHealth);
            });
        }
    }
}
