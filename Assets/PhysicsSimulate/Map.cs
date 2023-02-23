using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsSimulation
{
    public class Map : MonoBehaviour
    {
        public Rotater Rotater => rotater;

        [SerializeField] private Rotater rotater;
    }
}