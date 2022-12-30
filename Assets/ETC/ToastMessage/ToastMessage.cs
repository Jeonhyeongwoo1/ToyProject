using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

interface IVisible
{
    void Show(UnityAction done = null);
    void Hide(UnityAction done = null);
}

public class ToastMessage : MonoBehaviour, IVisible
{
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private Text messageText;
    [SerializeField] private float moveIntervalY;

    public void Init(float alpha, string message)
    {
        panelCanvasGroup.alpha = alpha;
        messageText.text = message;
    }

    public void MoveTo()
    {
        RectTransform rect = GetComponent<RectTransform>();
        float value = rect.sizeDelta.y + rect.anchoredPosition.y + moveIntervalY;
        rect.DOKill();
        rect.DOAnchorPosY(value, 0.2f);
    }

    public void Show(UnityAction done = null)
    {
        DoFade(1.5f, done);
    }

    public void Hide(UnityAction done = null)
    {
        Destroy(gameObject);
        // gameObject.SetActive(false);
        // transform.position = Vector3.zero;
    }

    private void DoFade(float duration, UnityAction done)
    {
        panelCanvasGroup.DOKill();
        panelCanvasGroup.DOFade(1, duration)
                        .OnComplete(() =>
                        {
                            panelCanvasGroup.DOFade(0, duration)
                                            .OnComplete(() => done?.Invoke());
                        });
    }
}
