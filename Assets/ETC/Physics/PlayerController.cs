using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform tpRig;
    [SerializeField] CapsuleCollider col;
    [SerializeField] Rigidbody rd;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float jumpForce = 5f;

    private Vector3 _MoveDir;
    private Vector3 _Rot;
    private float _AxisY;
    private float _JumpCoolTime = 2f;
    private float _CurJumpTime = 0;
    private bool _IsJump;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    void LateUpdate()
    {

        Vector3 dir = _MoveDir.normalized * Time.fixedDeltaTime * moveSpeed;
        rd.MovePosition(rd.position + dir);

        Vector3 rotDir = _Rot.normalized * rotationSpeed;
        // tpRig.rotation = Quaternion.AngleAxis(_AxisY, Vector3.up) * tpRig.rotation;
        tpRig.rotation = Quaternion.Euler(tpRig.eulerAngles + rotDir);

        if (_IsJump)
        {
            Vector3 force = Vector3.up * jumpForce;
            rd.AddForce(force, ForceMode.VelocityChange);
            _CurJumpTime = _JumpCoolTime;
            _IsJump = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        float h, v;
        float x, y;

        x = Input.GetAxisRaw("Mouse X");
        y = Input.GetAxisRaw("Mouse Y");

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(_CurJumpTime <= 0)
            {
                _IsJump = true;
            }
        }

        if (_CurJumpTime > 0)
        {
            _CurJumpTime -= Time.deltaTime;
        }

        _MoveDir.x = h;
        _MoveDir.z = v;
        _Rot.y = x;
        _Rot.x = -y;
        _AxisY = x;

    }
}
