using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerState { None, Idle, Move, Attack, Down, Jump }

    public PlayerState state;

    [SerializeField] private LayerMask groundLayerMask;
    private Rigidbody rd;
    private Vector3 inputValue = Vector3.zero;
    private Vector3 lerpValue = Vector3.zero;
    private bool isGround = false;
    private bool isJumping = false;
    private float jumpForce = 5;
    private float groundCheckDist = 1f;
    private float groundMaxDist = 2f;
    private bool isDown = false;

    private void Awake()
    {
        rd = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        lerpValue = Vector3.Lerp(lerpValue, inputValue, Time.fixedDeltaTime * 25);
        Vector3 prevPos = transform.position;
        Vector3 nextPos = transform.position + lerpValue;

        if (isDown)
        {
            Down();
        }

        rd.MovePosition(nextPos);

        if (isJumping)
        {
            isJumping = false;
            rd.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Down()
    {
        Debug.Log("Down");
    }

    private bool IsGroundCheck()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, groundMaxDist, groundLayerMask))
        {
            return hitInfo.distance <= groundCheckDist;
        }

        return false;
    }

    private void SetInputValue()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0)
        {
            state = PlayerState.Move;
        }
        else
        {
            state = PlayerState.Idle;
        }

        if (IsGroundCheck())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                state = PlayerState.Jump;
                isJumping = true;
            }
        }

        if (Input.GetMouseButton(0))
        {
            isAttack = true;
        }

        isDown = Input.GetKey(KeyCode.Q);
        inputValue.x = x;
        inputValue.z = z;
    }
    private bool isAttack = false;

    // Update is called once per frame
    void Update()
    {

        SetInputValue();
        switch (state)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Move:
                break;
            case PlayerState.Attack:
                break;
            case PlayerState.Down:
                break;
        }
    }
}
