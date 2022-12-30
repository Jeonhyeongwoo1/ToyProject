using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public enum CameraType { FP, TP }

    [Serializable]
    public class CameraOption
    {
        public Transform rig;
        public Camera cam;

        public int upDegree;
        public int downDegree;
        public int speed = 10;
    }

    [Serializable]
    public class MovingOption
    {
        public LayerMask groundLayer;
        public float movingSpeed;
        public float jumpForce;
        public float jumpCoolTime = 1f;
        public float zoomIn = 2;
        public float zoomOut = 6;
        public float zoomForce = 2f;
    }

    [SerializeField] Animator _Animator;
    [SerializeField] CapsuleCollider _Collider;
    [SerializeField] Rigidbody _Rigidbody;
    [SerializeField] CameraType _CameraType;
    [SerializeField] CameraOption _FPCam;
    [SerializeField] CameraOption _TPCam;
    [SerializeField] Transform _FPRoot;
    [SerializeField] MovingOption _MOption;

    private Vector3 _Rotation;
    private Vector3 _CurRot;
    private Vector3 _CurMove;
    private Vector3 _MoveDir;
    private float _Radius;
    private float _JumpCoolTime;
    private float _ZoomInputValue;
    private float _CurWheel;
    private float _CurDistTPCam;

    void FPRotate()
    {
        float prevX = _FPCam.cam.transform.localEulerAngles.x;
        float nextX = prevX + _CurRot.x * _FPCam.speed;

        if (nextX > 180)
        {
            nextX -= 360;
        }

        float prevY = _FPRoot.localEulerAngles.y;
        float nextY = prevY + _CurRot.y * _FPCam.speed;

        bool isRotable = nextX > _FPCam.upDegree && nextX < _FPCam.downDegree;
        _FPCam.cam.transform.localEulerAngles = isRotable ? Vector3.right * nextX : Vector3.right * prevX;
        _FPRoot.localEulerAngles = Vector3.up * nextY;
    }

    void TPRotate()
    {
        float prevX = _TPCam.rig.localEulerAngles.x;
        float nextX = prevX + _CurRot.x * _TPCam.speed;

        if (nextX > 180)
        {
            nextX -= 360;
        }

        float prevY = _TPCam.rig.localEulerAngles.y;
        float nextY = prevY + _CurRot.y * _TPCam.speed;

        bool isRotable = nextX > _TPCam.upDegree && nextX < _TPCam.downDegree;
        _TPCam.rig.localEulerAngles = Vector3.up * nextY + (isRotable ? Vector3.right * nextX : Vector3.right * prevX);
    }

    void FPRootRotate()
    {
        if (_CameraType == CameraType.FP) { return; }

        Vector3 dir = _TPCam.rig.TransformDirection(_MoveDir);
        float prevY = _FPRoot.localEulerAngles.y;
        float nextY = Quaternion.LookRotation(dir, Vector3.up).eulerAngles.y;

        if (nextY - prevY > 180)
        {
            nextY -= 360;
        }
        else if (prevY - nextY > 180)
        {
            nextY += 360;
        }

        _FPRoot.localEulerAngles = Vector3.up * Mathf.Lerp(prevY, nextY, 0.1f);
    }
    
    void Move()
    {
        if (_MoveDir == Vector3.zero)
        {
            _Rigidbody.velocity = new Vector3(0, _Rigidbody.velocity.y, 0);
            return;
        }

        Vector3 dir = Vector3.zero;
        if (_CameraType == CameraType.FP)
        {
            dir = _FPRoot.TransformDirection(_MoveDir);
        }
        else
        {
            dir = _TPCam.rig.TransformDirection(_MoveDir);
        }


        Vector3 move = dir * _MOption.movingSpeed;
        _Rigidbody.velocity = new Vector3(move.x, _Rigidbody.velocity.y, move.z);

    }

    void SetInputValue()
    {
        float x, y;
        float h, v;

        y = Input.GetAxisRaw("Mouse X");
        x = Input.GetAxisRaw("Mouse Y");

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        _ZoomInputValue = Input.GetAxisRaw("Mouse ScrollWheel");
        _Rotation.y = y;
        _Rotation.x = -x;

        _MoveDir.z = v;
        _MoveDir.x = h;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _MoveDir.z *= 2f;
            _MoveDir.x *= 2f;
        }

        _CurRot = Vector3.Lerp(_CurRot, _Rotation.normalized, Time.deltaTime * 10f);
        _CurWheel = Mathf.Lerp(_CurWheel, _ZoomInputValue, 0.1f);
    }

    void TransitionCameraType()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _TPCam.cam.gameObject.SetActive(_CameraType == CameraType.FP);
            _FPCam.cam.gameObject.SetActive(_CameraType == CameraType.TP);
            _CameraType = _CameraType == CameraType.FP ? CameraType.TP : CameraType.FP;


            if (_CameraType == CameraType.FP)
            {
                _FPCam.cam.transform.localEulerAngles = _TPCam.rig.localEulerAngles.x * Vector3.right;
                _FPCam.rig.localEulerAngles = _TPCam.rig.localEulerAngles.y * Vector3.up;
            }
            else
            {
                Vector3 rot = Vector3.zero;
                rot.x = _FPCam.cam.transform.localEulerAngles.x;
                rot.y = _FPCam.rig.localEulerAngles.y;
                _TPCam.rig.localEulerAngles = rot;
            }
        }
    }

    void Jump()
    {
        if (!CheckDistFromGround())
        {
            return;
        }

        if (_JumpCoolTime > 0)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _JumpCoolTime = _MOption.jumpCoolTime;
            _Rigidbody.AddForce(Vector3.up * _MOption.jumpForce, ForceMode.VelocityChange);
            if(_Animator != null)
            {
                _Animator.SetTrigger("Jump");
            }
           
        }
    }

    bool CheckDistFromGround()
    {
        const float maxDist = 100f;
        const float threshold = 0.01f;

        float distFromGround = 0;
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        bool cast = Physics.SphereCast(ray, _Radius, out RaycastHit hit, maxDist, _MOption.groundLayer);
        distFromGround = cast ? hit.distance - 1 + _Radius : float.MaxValue;
        return distFromGround <= _Radius + threshold;
    }

    void UpdateJumpCoolTime()
    {
        if (_JumpCoolTime > 0)
        {
            _JumpCoolTime -= Time.deltaTime;
        }
    }

    void Rotate()
    {
        if (_CameraType == CameraType.FP)
        {
            FPRotate();
        }
        else
        {
            TPRotate();
            FPRootRotate();
        }
    }

    void UpdateAnimationParam()
    {
        if (_Animator == null)
        {
            return;
        }
        _Animator.SetFloat("moveY", _MoveDir.z);
        _Animator.SetFloat("moveX", _MoveDir.x);
    }

    void ZoomInOut()
    {
        if (_CameraType == CameraType.FP) { return; }
        _CurDistTPCam = Vector3.Distance(_FPRoot.transform.position, _TPCam.cam.transform.position);

        if (_CurWheel > 0.01f)
        {
            Vector3 move = _CurWheel * Vector3.forward * _MOption.zoomForce;
            if(_CurDistTPCam > _MOption.zoomIn)
            {
                _TPCam.cam.transform.Translate(move, Space.Self);
            }
        }
        else if (_CurWheel < -0.01f)
        {
            Vector3 move = _CurWheel * Vector3.forward * _MOption.zoomForce;
            if (_CurDistTPCam < _MOption.zoomOut)
            {
                _TPCam.cam.transform.Translate(move, Space.Self);
            }

        }
    }

    void Start()
    {
        _Radius = _Collider.radius;
    }

    void Update()
    {

        SetInputValue();
        Move();
        Rotate();
        TransitionCameraType();
        Jump();
        UpdateAnimationParam();
        UpdateJumpCoolTime();
        ZoomInOut();
    }


}
