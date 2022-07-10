using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidbody2D;
    public float velocity = 5f;

    void Start() {
      //  rigidbody2D.velocity = velocity * transform.right;
        
    }
}
