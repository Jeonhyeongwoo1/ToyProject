using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeshAfterImageData
{
    public readonly string color = "_Color";
    public readonly string alpha = "_Alpha";
    public float alphaChangeTime;
    public float alphaChangeValue = 0.1f;
}

public class MeshAfterImage : MonoBehaviour
{
    [SerializeField] private Material _MeshAfterImageMat;
    [SerializeField] private int _AfterImageCount;
    [SerializeField] private MeshAfterImageData _MeshAfterImageData;
    [SerializeField]
    Gradient _AfterImageGradient = new Gradient()
    {
        colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(new Color(0.7f, 0.0f, 1.0f), 0.00f),
            new GradientColorKey(new Color(0.3f, 1.0f, 1.0f), 0.25f),
            new GradientColorKey(new Color(1.0f, 1.0f, 0.0f), 0.50f),
            new GradientColorKey(new Color(0.3f, 1.0f, 1.0f), 0.75f),
            new GradientColorKey(new Color(0.7f, 0.0f, 1.0f), 1.00f),
        }
    };

    [SerializeField] private float _FaderWakeUpTime = 1;
    [SerializeField] private float moveSpeed;

    private Vector3 moveDir;
    private Rigidbody rd;
    private float _CurrentElapsedTime = 0;
    private int _ColorKeyIndex;
    private Transform _MeshAfterImageContainer;
    private Queue<MeshAfterImageFader> meshAfterImageFaderQueue = new Queue<MeshAfterImageFader>();

    public void SetUp()
    {
        GameObject child = new GameObject();
        child.transform.SetParent(_MeshAfterImageContainer);
        child.AddComponent<MeshAfterImageFader>();
        MeshAfterImageFader fader = child.GetComponent<MeshAfterImageFader>();
        fader.SetUp(this, _MeshAfterImageMat, _MeshAfterImageData);
        SetMeshAfterImageFaderReadyState(fader);
    }

    public void SetMeshAfterImageFaderReadyState(MeshAfterImageFader fader)
    {
        meshAfterImageFaderQueue.Enqueue(fader);
    }

    public void SetMeshAfterImageFaderRunningState(out MeshAfterImageFader fader)
    {
        fader = meshAfterImageFaderQueue.Dequeue();
    }

    private void Start()
    {
        rd = GetComponent<Rigidbody>();
        GameObject go = new GameObject();
        go.name = "Mesh After Image Container";

        _MeshAfterImageContainer = go.transform;
    }

    private void WakeUpFader()
    {
        if (meshAfterImageFaderQueue.Count == 0)
        {
            SetUp();
        }

        _ColorKeyIndex++;
        if (_ColorKeyIndex > _AfterImageGradient.colorKeys.Length - 1)
        {
            _ColorKeyIndex = 0;
        }

        MeshAfterImageFader fader;
        SetMeshAfterImageFaderRunningState(out fader);

        if (fader)
        {
            fader.WakeUp(this, _AfterImageGradient.colorKeys[_ColorKeyIndex].color);
        }
    }

    private void Update()
    {
        _CurrentElapsedTime += Time.deltaTime;

        if (_CurrentElapsedTime > _FaderWakeUpTime)
        {
            WakeUpFader();
            _CurrentElapsedTime = 0;
        }

    }

    private void FixedUpdate()
    {
        float z = Input.GetAxisRaw("Vertical");
        float x = Input.GetAxisRaw("Horizontal");

        moveDir.x = x;
        moveDir.z = z;

        Vector3 prev = transform.position;
        Vector3 next = prev + moveDir;
        Vector3 value = Vector3.Lerp(prev, next, Time.fixedDeltaTime * moveSpeed);
        rd.MovePosition(value);
    }
}
