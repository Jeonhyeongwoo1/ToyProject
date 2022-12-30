using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Enemy : MonoBehaviour
{
    private Rigidbody rigidbody;

    [SerializeField] private float _JumpForce;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Rigidbody>(out var rigidbody);
        this.rigidbody = rigidbody;
        Observable.EveryUpdate()
                    .Where((v)=> Input.GetKeyDown(KeyCode.Space))
                    .Subscribe((v)=> rigidbody.AddForce(Vector3.up * _JumpForce, ForceMode.VelocityChange));
    }

}
