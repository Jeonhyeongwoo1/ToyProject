using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class Menu : MonoBehaviour, IPointerExitHandler
{

    [Serializable]
    public class MenuItem
    {
        public Transform item;
        public float axisZ;
    }

    public enum State { None, Dimming, Dim, Opening, Opened, Closing, Closed }

    [SerializeField] private List<MenuItem> items = new List<MenuItem>();
    [SerializeField] private Transform behaivor;
    [SerializeField] private CanvasGroup logo;
    [SerializeField] private float backgroundOpenCloseDuration;
    [SerializeField] private float itemOpenCloseScaleDuration;
    [SerializeField] private float itemOpenCloseRotateDuration;
    [SerializeField] private float itemOpenCloseInterval;
    [SerializeField] private float dimingDuration;
    [SerializeField] private float dimingAlpha;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private State state;

    public void OnPointerEnter()
    {
        if (state == State.None || state == State.Opening || state == State.Opened || state == State.Closing) { return; }

        if (state == State.Dimming || state == State.Dim)
        {
            StopAllCoroutines();
            SetDim(false);
        }
        
        StartCoroutine(OpeningAnimation(OpendMenu));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null) { return; }
        if (state != State.Opened) { return; }

        StartCoroutine(CloseAnimation(ClosedMenu));
    }

    public void OnPointerExit()
    {  
        if (state != State.Opened) { return; }

        StartCoroutine(CloseAnimation(ClosedMenu));
    }

    private void ClosedMenu()
    {
        state = State.Closed;
        StartCoroutine(Dimming(true, Dim));
    }

    private void OpendMenu()
    {
        state = State.Opened;
        SetDim(false);
    }

    private void Dim()
    {
        state = State.Dim;
    }

    private void SetDim(bool isDimming)
    {
        logo.alpha = isDimming ? 0 : 1;
    }

    void Start()
    {
        state = State.Dim;
    }

    void Awake()
    {
        behaivor.localScale = Vector3.zero;

        foreach (MenuItem v in items)
        {
            v.item.GetChild(0).localScale = Vector3.zero;
            v.item.localEulerAngles = Vector3.zero;
            v.item.GetChild(0).localEulerAngles = Vector3.zero;
        }
    }

    void OnValidate()
    {
        foreach (MenuItem v in items)
        {
            v.item.rotation = Quaternion.Euler(new Vector3(0, 0, v.axisZ));
            v.item.GetChild(0).localEulerAngles = new Vector3(0, 0, -v.axisZ);
        }
    }

    IEnumerator Dimming(bool isDimming, UnityAction done)
    {
        state = State.Dimming;
        float elapsed = 0;
        float start = logo.alpha;
        float alpha = isDimming ? dimingAlpha : 1;
        while (elapsed < dimingDuration)
        {
            elapsed += Time.deltaTime;
            logo.alpha = Mathf.Lerp(start, alpha, curve.Evaluate(elapsed / dimingDuration));
            yield return null;
        }

        done?.Invoke();
    }

    IEnumerator CloseAnimation(UnityAction done = null)
    {
        int count = 0;
        int lastIdx = items.Count - 1;
        WaitForSeconds waitForSeconds = new WaitForSeconds(itemOpenCloseInterval);
        for (int i = lastIdx; i >= 0; --i)
        {
            MenuItem menu = items[i];
            StartCoroutine(RotateItem(menu, 0, () =>
                             StartCoroutine(TransformScale(menu.item.GetChild(0), Vector3.one, Vector3.zero, itemOpenCloseScaleDuration, () => count++))));
            yield return waitForSeconds;
        }

        while (count != items.Count)
        {
            yield return null;
        }

        yield return TransformScale(behaivor, Vector3.one, Vector3.zero, backgroundOpenCloseDuration);

        done?.Invoke();
    }

    IEnumerator TransformScale(Transform target, Vector3 from, Vector3 to, float duration, UnityAction done = null)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.localScale = Vector3.LerpUnclamped(from, to, curve.Evaluate(elapsed / duration));
            yield return null;
        }

        done?.Invoke();
    }

    IEnumerator OpeningAnimation(UnityAction done)
    {
        state = State.Opening;
        int count = 0;
        WaitForSeconds waitForSeconds = new WaitForSeconds(itemOpenCloseInterval);
        yield return TransformScale(behaivor, Vector3.zero, Vector3.one, backgroundOpenCloseDuration);

        foreach (MenuItem h in items)
        {
            yield return TransformScale(h.item.GetChild(0), Vector3.zero, Vector3.one, itemOpenCloseScaleDuration,
                                        () => StartCoroutine(RotateItem(h, h.axisZ, () => count++)));
            yield return waitForSeconds;
        }


        while (count != items.Count - 1)
        {
            yield return null;
        }

        done?.Invoke();
    }

    IEnumerator RotateItem(MenuItem menuItem, float rotateValue, UnityAction done)
    {

        Transform item = menuItem.item;

        float elapsed = 0;
        Quaternion quaternion = item.rotation;
        while (elapsed < itemOpenCloseRotateDuration)
        {
            elapsed += Time.deltaTime;
            item.rotation = Quaternion.Slerp(quaternion, Quaternion.Euler(new Vector3(0, 0, rotateValue)), elapsed / itemOpenCloseRotateDuration);
            item.GetChild(0).localRotation = Quaternion.Slerp(quaternion, Quaternion.Euler(new Vector3(0, 0, -rotateValue)), elapsed / itemOpenCloseRotateDuration);
            yield return null;
        }

        done?.Invoke();
    }

}
