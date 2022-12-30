using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UniRx.Triggers;

public class Ball : MonoBehaviour
{

    [SerializeField] private bool isForceUpdate = false;
    private Rigidbody rigidbody;

    private void Awake()
    {
        TryGetComponent<Rigidbody>(out var rigidbody);
        this.rigidbody = rigidbody;
    }


    private void Start()
    {
        // Observable.Timer(TimeSpan.FromSeconds(3.0f))
        //             .TakeUntilDisable(this)
        //             .Subscribe((v) => Destroy(gameObject));

        this.FixedUpdateAsObservable()
            .Where((_) => isForceUpdate)
            .Subscribe(_ =>
            {
                rigidbody.MovePosition(transform.position + Vector3.up);
            });

        this.OnCollisionEnterAsObservable()
            .Subscribe((v) =>
            {
                if (v.gameObject.name == "Ball")
                {
                    Debug.Log("OnCollision");
                }
            });

        Observable.Timer(TimeSpan.FromSeconds(5))
                    .Subscribe((v) => isForceUpdate = false);
    }

}
