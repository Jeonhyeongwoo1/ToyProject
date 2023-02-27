using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsController
{
    public class ConveyorBelt : MonoBehaviour
    {
        [SerializeField] private List<PlayerController> playerList = new List<PlayerController>();
        [SerializeField] private Vector3 _direction;
        [SerializeField] private float _speed;
        [SerializeField] private float _playerSpeed;

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent<PlayerController>(out var player))
            {
                if (!playerList.Contains(player))
                {
                    player.SetSpeed(_playerSpeed);
                    playerList.Add(player);
                }
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.transform.TryGetComponent<PlayerController>(out var player))
            {
                if (playerList.Contains(player))
                {
                    player.SetDefaultSpeed();
                    playerList.Remove(player);
                }
            }
        }

        private Rigidbody rigidbody;
        private void Start()
        {
            this.rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            Quaternion quaternion = Quaternion.Euler(new Vector3(0, 60, 0) * Time.fixedDeltaTime);
            rigidbody.MoveRotation(quaternion * rigidbody.rotation);

            // for (int i = 0; i < playerList.Count; i++)
            // {
            //     playerList[i].Rd.velocity = _speed * _direction * Time.deltaTime;
            // }
        }
    }
}
