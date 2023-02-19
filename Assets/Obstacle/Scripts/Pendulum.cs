using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacle
{
    public class Pendulum : MonoBehaviour
    {
        [SerializeField] private Transform _Target;
        [SerializeField] private float speed;
        [SerializeField] private float angle;

        // Update is called once per frame
        void Update()
        {
            float angle = Mathf.Sin(Time.time * speed) * this.angle;
            _Target.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
