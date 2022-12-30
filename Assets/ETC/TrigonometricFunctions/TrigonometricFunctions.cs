using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigonometricFunctions : MonoBehaviour
{

    public GameObject bullet;
    public Transform bulletSpawner;
    public float bulletSpeed = 5;
    public int degree;
    public float radius = 1f;
    public float degreeInterval = 30f;


    [Tooltip("[기본 공격]")]
    public bool defualtShoot = true;
    [Tooltip("[캐릭터 주위를 원 운동을 하면서 공격")]
    public bool sequenceCircleShoot = true;
    [Tooltip("[캐릭터 주위에 원 형태 공격]")]
    public bool circleShoot = true;
    [Tooltip("[캐릭터 앞쪽 반원 형태의 공격")]
    public bool semiCircelShoot = true;

    [Tooltip("[캐릭터 주위 원 형태 공격")]
    public bool circelShoot2 = true;


    private float currentDegree = 0;
    private Vector3 movedir;
    private Vector3 rot;

    void Move()
    {
        float h, v;
        h = Input.GetAxisRaw("Vertical");
        v = Input.GetAxisRaw("Horizontal");

        movedir.x = v;
        movedir.z = h;

        transform.position += movedir * Time.deltaTime * 10f;
    }

    void Rotate()
    {
        float y;
        y = Input.GetAxisRaw("Mouse X");

        rot.y = y;

        transform.eulerAngles += Vector3.up * rot.y * Time.deltaTime * 50f;
    }

    private void Shoot()
    {

        if (defualtShoot)
        {
            GameObject go = Instantiate(bullet, transform.position, Quaternion.identity);
            go.transform.position = bulletSpawner.position;
            go.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        }

        if(sequenceCircleShoot)
        {
            GameObject go = Instantiate(bullet, transform.position, Quaternion.identity);
            go.transform.position = bulletSpawner.position;

            float x, y;
            x = Mathf.Cos(Mathf.Deg2Rad * currentDegree) * radius;
            y = Mathf.Sin(Mathf.Deg2Rad * currentDegree) * radius;

            Vector3 value = new Vector3(x, 0, y);
            go.transform.right = value;
            go.GetComponent<Rigidbody>().velocity = go.transform.right * bulletSpeed;
            
            currentDegree += degreeInterval;
        }

        if(circleShoot)
        {
            for (int i = 0; i <= 360; i += (int)degreeInterval)
            {
                GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                b.transform.position = bulletSpawner.position;
                float x, y;
                x = Mathf.Cos(Mathf.Deg2Rad * i) * radius;
                y = Mathf.Sin(Mathf.Deg2Rad * i) * radius;

                Vector3 value = new Vector3(x, 0, y);
                b.transform.right = value;
                b.GetComponent<Collider>().enabled = false;
                b.GetComponent<Rigidbody>().velocity = b.transform.right * bulletSpeed;
            }
        }

        if(semiCircelShoot)
        {
            for (int i = 0; i <= 90; i += (int)degreeInterval)
            {
                GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                b.transform.position = bulletSpawner.position;
                float x, y;
                x = Mathf.Cos(Mathf.Deg2Rad * i) * radius;
                y = Mathf.Sin(Mathf.Deg2Rad * i) * radius;

                Vector3 value = new Vector3(x, 0, y);
                b.transform.forward = value;
                b.GetComponent<Collider>().enabled = false;
                b.GetComponent<Rigidbody>().velocity = b.transform.forward * bulletSpeed;
            }
        }

        if(circelShoot2)
        {
            GameObject go = Instantiate(bullet, transform.position, Quaternion.identity);
            go.transform.position = bulletSpawner.position;
            StartCoroutine(CircleShooting(go));
        }

    }

    IEnumerator CircleShooting(GameObject go)
    {
        float x = 1, y;
        float theta = 0;
        float circleRadius = 1;
        float angle = 0;

        while(true)
        {
            theta += Time.deltaTime;
            circleRadius += Time.deltaTime;
            angle = 2 * Mathf.PI * theta;
            x = radius * Mathf.Cos(angle) * theta;
            y = radius * Mathf.Sin(angle) * theta;

            Vector3 value = new Vector3(x, 0, y);
            go.transform.position = value;
            go.GetComponent<Collider>().enabled = false;
            yield return null;
        }
    }


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        Move();
        Rotate();
    }
}
