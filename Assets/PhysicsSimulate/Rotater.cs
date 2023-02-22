using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsSimulation
{
    public class Rotater : MonoBehaviour
    {
        public void DoRotate()
        {
            StartCoroutine(DoRotateCor());
        }

        private IEnumerator DoRotateCor()
        {
            while(true)
            {
                transform.Rotate(Vector3.forward * 10 * Time.deltaTime);
                yield return null;
            }
        }
    }
}
