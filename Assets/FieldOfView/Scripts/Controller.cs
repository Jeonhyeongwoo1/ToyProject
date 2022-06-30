using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    float moveSpeed = 6;
    Rigidbody rd;
    Camera viewCamera;
    Vector3 velocity;

    void Start() {
        rd = GetComponent<Rigidbody>();
        viewCamera = Camera.main;
    }

    void Update() {
        Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        transform.LookAt(mousePos + Vector3.up * transform.position.y);
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    }

    void FixedUpdate() {
        rd.MovePosition(rd.position + velocity * Time.fixedDeltaTime );
    }

}
