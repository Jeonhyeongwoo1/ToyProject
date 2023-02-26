using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class Rotator : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {
            Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            screenPoint.z = 0;

            Vector3 value = screenPoint - transform.position;
            float angle = Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg;
            Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.position = Vector3.MoveTowards(transform.position, screenPoint, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, Time.deltaTime * 5);
        }
    }
}