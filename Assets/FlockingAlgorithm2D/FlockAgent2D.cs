using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent2D : MonoBehaviour
{
    Collider2D col;

    public Flock2D flock2D;
    public Collider2D collider => col;

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3) velocity * Time.deltaTime;
    }

    public void Initialize(Flock2D flock2D)
    {
        this.flock2D = flock2D; 
    }

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
