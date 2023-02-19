using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacle
{
    public class Bounds : MonoBehaviour
    {
        [SerializeField] private float force = 10;

        private void OnCollisionEnter(Collision other)
        {
            foreach(ContactPoint contactPoint in other.contacts)
            {
                if(other.gameObject.tag == "Player")
                {
                    Vector3 hit = contactPoint.normal;
                    other.transform.GetComponent<CharacterControls>().HitPlayer(-hit * force, 3);
                }
            }
        }
    }
}