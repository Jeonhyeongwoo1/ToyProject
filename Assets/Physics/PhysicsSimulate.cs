using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace PhysicsSimulation
{
    public class PhysicsSimulate : MonoBehaviour
    {
        [SerializeField] private int _simulateCount;
        [SerializeField] private Rigidbody _sphere;

        [SerializeField] private List<Vector3> _simulatePosition;

        [Button]
        public void Simulate()
        {
            Physics.autoSimulation = false;

            _simulatePosition = new List<Vector3>(_simulateCount);
            _sphere.AddForce(Vector3.left * 50, ForceMode.Impulse);

            for (int i = 0; i < _simulateCount; i++)
            {
                Physics.Simulate(Time.fixedDeltaTime);   
                _simulatePosition.Add(_sphere.transform.position);
            }

           Physics.autoSimulation = true;
        }
    }
}