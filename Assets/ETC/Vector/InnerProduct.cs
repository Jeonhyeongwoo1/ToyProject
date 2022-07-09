using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerProduct : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float range = 60f;
    [SerializeField] float radius = 5;

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Transform col = colliders[i].transform;
            float dist = (col.transform.position - transform.position).magnitude;
            //Vector3.Dot은 벡터의 내적을 구할 수 있는 함수이다.
            float angle = Vector3.Dot(transform.forward, col.transform.position - transform.position)
                            / dist * transform.forward.magnitude;

            Debug.Log(angle);

            float cos_angle = Mathf.Acos(angle) * Mathf.Rad2Deg;
            Debug.Log(cos_angle);

            float v = Vector3.Angle(transform.forward, col.position - transform.position);
            Debug.Log(v);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);

        float angle = transform.eulerAngles.y + range * 0.5f;
        Vector3 right = transform.position + new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad)) * radius;
        Vector3 left = transform.position + new Vector3(Mathf.Sin(-angle * Mathf.Deg2Rad), 0, Mathf.Cos(-angle * Mathf.Deg2Rad)) * radius;
        Gizmos.DrawLine(transform.position, left);
        Gizmos.DrawLine(transform.position, right);
    }
}
