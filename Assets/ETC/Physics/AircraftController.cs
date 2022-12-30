using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftController : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float torqueForce;

    private Rigidbody rd;
    private Collider col;
    private Vector3 inputValue = Vector3.zero;
    private Quaternion curRot;
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Rigidbody>(out var rigidbody);
        rd = rigidbody;

        TryGetComponent<Collider>(out var collider);
        col = collider;    
    }

    private void FixedUpdate()
    {

        Vector3 nextDir = inputValue.normalized * force * Time.fixedDeltaTime;
        rd.AddForce(nextDir);
        // rd.MovePosition(rd.position + nextDir);
        
        // if(dir.normalized.x < 0 && dir.normalized.x > -1)
        // {
        //     rd.AddTorque(-Vector3.up * torqueForce, ForceMode.VelocityChange);
        // }
        // else if(dir.normalized.x < 1 && dir.normalized.x > 0)
        // {
        //     rd.AddTorque(Vector3.up * torqueForce, ForceMode.VelocityChange);
        // }
        // else
        // {
        //     rd.angularVelocity = Vector3.zero;
        // }


        if(isMoving)
        {
            // Quaternion nextRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
            // transform.rotation = Quaternion.Lerp(transform.rotation, nextRot, Time.fixedDeltaTime * 5);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float z = Input.GetAxisRaw("Vertical");
        float x = Input.GetAxisRaw("Horizontal");

        isMoving = x != 0 && z != 0;
        inputValue.x = x;
        inputValue.z = z;
    }
}
