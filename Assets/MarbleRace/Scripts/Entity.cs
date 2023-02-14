using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[Serializable]
public class EntityData
{
    [Serializable]
    public class BaseEntity
    {
        public bool use;
        public AnimationCurve curve;
        public float duration;
        public float multiplier;
    }

    [Serializable]
    public class ItemFromTo<T> : BaseEntity
    {
        public T from;
        public T to;
    }

    [Serializable]
    public class ItemScale : BaseEntity
    {
        public Vector2 originPos;
        public Vector2 moveToPos;
        public Vector2 originScale;
        public Vector2 scale;
    }

    public ItemFromTo<Transform> moveInfoByTrans;
    public ItemFromTo<Vector2> moveInfoByDist;
    public ItemScale itemScaleInfo;
    public BaseEntity rotateInfo;
}

public class Entity : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private EntityData entityData;

    private void Start()
    {
        if (entityData.moveInfoByTrans.use)
        {
            StartCoroutine(MoveCor(entityData.moveInfoByTrans.curve,
                                    entityData.moveInfoByTrans.duration,
                                    entityData.moveInfoByTrans.from.position,
                                    entityData.moveInfoByTrans.to.position));
        }

        if (entityData.moveInfoByDist.use)
        {
            StartCoroutine(MoveCor(entityData.moveInfoByDist.curve,
                                    entityData.moveInfoByDist.duration,
                                    entityData.moveInfoByDist.from,
                                    entityData.moveInfoByDist.to));
        }

        if (entityData.rotateInfo.use)
        {
            StartCoroutine(RotateCor(entityData.rotateInfo.multiplier));
        }

        if (entityData.itemScaleInfo.use)
        {
            StartCoroutine(ResizeCor());
        }
    }

    private IEnumerator ResizeCor()
    {
        bool isOrigin = true;
        while (true)
        {
            Vector2 endPos = isOrigin ? entityData.itemScaleInfo.moveToPos : entityData.itemScaleInfo.originPos;
            Vector2 endScale = isOrigin ? entityData.itemScaleInfo.scale : entityData.itemScaleInfo.originScale;
            bool isEndPosition = false;
            target.DOMove(endPos, entityData.itemScaleInfo.duration)
                    .OnComplete(() => isEndPosition = true)
                    .SetEase(entityData.itemScaleInfo.curve);

            target.DOScale(endScale, entityData.itemScaleInfo.duration)
                    .SetEase(entityData.itemScaleInfo.curve);
            yield return new WaitUntil(() => isEndPosition);

            isOrigin = !isOrigin;
        }
    }

    private IEnumerator MoveCor(AnimationCurve curve, float duration, Vector2 leftLimitPos, Vector2 rigtrLimitPos)
    {
        bool isLeft = false;
        while (true)
        {
            Vector2 endPos = isLeft ? leftLimitPos : rigtrLimitPos;
            bool isEndPosition = false;
            target.DOMove(endPos, duration)
                    .OnComplete(() => isEndPosition = true)
                    .SetEase(curve);
            yield return new WaitUntil(() => isEndPosition);

            isLeft = !isLeft;
        }
    }

    private IEnumerator RotateCor(float speed)
    {
        while (true)
        {
            target.Rotate(Vector3.forward * Time.deltaTime * speed);
            yield return null;
        }
    }
}
