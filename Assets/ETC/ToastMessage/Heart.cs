using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Heart : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private AnimationCurve customAnimationCurve;

    private bool useScale;
    public void Init(float startAlpha, float scale, bool useScale)
    {
        canvasGroup.alpha = startAlpha;
        this.useScale = useScale;
        transform.localScale = new Vector3(scale, scale, 1);
    }

    public void Play(Vector3 startPos
                                    , Vector3 endPos
                                     , float duration)
    {
        transform.position = startPos;
        // transform.DOMove(endPos, duration)
        //             .SetEase(Ease.Linear);

        StartCoroutine(SinMoving(startPos, endPos, duration));
        DOVirtual.DelayedCall(2, () =>
        {
            canvasGroup.DOFade(0, 1)
                               .SetEase(Ease.Linear)
                               .OnComplete(() =>
                               {
                                   gameObject.SetActive(false);
                               });
        });

        if (!useScale)
        {
            return;
        }

        Vector3 scale = transform.localScale + new Vector3(0.2f, 0.2f, 1);
        transform.DOScale(scale, duration)
                    .SetEase(customAnimationCurve);
    }

    [SerializeField] private float angle;
    [SerializeField] private float radius;
    private IEnumerator SinMoving(Vector3 startPos
                                    , Vector3 endPos
                                     , float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float cosMoveX = Mathf.Cos(elapsed * angle) * radius;
            Vector3 nextPos = Vector3.Lerp(startPos, endPos, elapsed / duration);
            nextPos.x += cosMoveX;
            transform.position = nextPos;
            yield return null;
        }
    }
}
