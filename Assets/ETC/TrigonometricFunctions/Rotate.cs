using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 value = Vector3.right * Time.deltaTime * 50;
        transform.Rotate(value, Space.Self);
    }
}
