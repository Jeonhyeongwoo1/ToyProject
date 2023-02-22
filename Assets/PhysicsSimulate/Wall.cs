using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsSimulation
{
    public class Wall : MonoBehaviour
    {
        public bool isOnSimulate;

        // Start is called before the first frame update
        void Start()
        {

        }

        private float time = 0;
        private void FixedUpdate()
        {
            if(!isOnSimulate)
            {
                return;
            }

            time += Time.fixedDeltaTime * 3;
            float value = Mathf.Sin(time);
            transform.position += new Vector3(0, value, 0);
        }
    }
}