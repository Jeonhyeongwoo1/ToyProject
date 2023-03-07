using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dot
{
    public class Dot : MonoBehaviour
    {
        public float Angle => angle;
        public float Radius => radius;
        //

        [SerializeField] private float radius;
        [SerializeField] private float angle;
        [SerializeField] private Transform target;

        public Vector3 DistToFromAngle(bool isGlobal, float angle)
        {
            if (!isGlobal)
            {
                angle += transform.localEulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
        }

        private void Update()
        {

            float dot = Vector3.Dot((target.transform.position - transform.position), transform.forward);
            float magnitude = (target.transform.position - transform.position).magnitude;
            float forward = transform.forward.magnitude;
            float cosAngle = dot / magnitude * forward;
            float angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;
            float a = Vector3.Angle(target.transform.position - transform.position, transform.forward);
            Debug.Log(angle);
            //내적은 앞에 있으면 양수를 의미하고 뒤에 있으면 -값을 배포한다.
       
        
        }

    }
}