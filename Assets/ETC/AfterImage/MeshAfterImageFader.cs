using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class MeshAfterImageFader : MonoBehaviour
{
    private MeshAfterImage _Controller;

    private MeshFilter[] _MeshFilterArray { get; set; }
    private Transform[] _ChildTranformArray { get; set; }
    private MeshRenderer[] _MeshRendererArray { get; set; }

    private MeshAfterImageData _MeshAfterIamgeData;

    private float _CurrentAlphaValue = 1;
    private float _CurrentElapsedTime = 0;

    public void SetUp(MeshAfterImage targetMeshAfterImage, Material meshAfterImageMat, MeshAfterImageData meshAfterIamgeData)
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = targetMeshAfterImage.GetComponent<MeshFilter>().mesh;
        GetComponent<MeshRenderer>().material = meshAfterImageMat;

        _Controller = targetMeshAfterImage;
        _MeshAfterIamgeData = meshAfterIamgeData;

        _MeshFilterArray = GetComponentsInChildren<MeshFilter>(true);
        _ChildTranformArray = GetComponentsInChildren<Transform>(true);
        _MeshRendererArray = GetComponentsInChildren<MeshRenderer>(true);

        SetChildPosAndRot(targetMeshAfterImage.transform.position, targetMeshAfterImage.transform.rotation);
    }

    private void SetChildPosAndRot(Vector3 pos, Quaternion rot)
    {
        if (_ChildTranformArray == null)
        {
            return;
        }

        foreach (Transform child in _ChildTranformArray)
        {
            child.SetPositionAndRotation(pos, rot);
        }
    }

    private void SetChildMeshColor(Color color)
    {
        if (_MeshRendererArray == null)
        {
            return;
        }

        for (int i = 0; i < _MeshRendererArray.Length; i++)
        {
            _MeshRendererArray[i].material.SetColor(_MeshAfterIamgeData.color, color);
        }
    }

    public void WakeUp(MeshAfterImage targetMeshAfterImage, Color color)
    {
        SetChildMeshColor(color);
        SetChildPosAndRot(targetMeshAfterImage.transform.position, targetMeshAfterImage.transform.rotation);
        gameObject.SetActive(true);
    }

    public void Sleep()
    {
        _Controller.SetMeshAfterImageFaderReadyState(this);
        gameObject.SetActive(false);
        SetChildMeshAlpha(1);
    }

    public void SetChildMeshAlpha(float alpha)
    {
        if (_MeshRendererArray == null)
        {
            return;
        }

        for (int i = 0; i < _MeshRendererArray.Length; i++)
        {
            _MeshRendererArray[i].material.SetFloat(_MeshAfterIamgeData.alpha, alpha);
        }
    }

    private void Update()
    {
        if (_CurrentElapsedTime > _MeshAfterIamgeData.alphaChangeTime)
        {
            _CurrentAlphaValue -= _MeshAfterIamgeData.alphaChangeValue;
            SetChildMeshAlpha(_CurrentAlphaValue);
            _CurrentElapsedTime = 0;
        }

        _CurrentElapsedTime += Time.deltaTime;

        if (_CurrentAlphaValue <= 0)
        {
            Sleep();
            _CurrentAlphaValue = 1;
        }
    }
}
