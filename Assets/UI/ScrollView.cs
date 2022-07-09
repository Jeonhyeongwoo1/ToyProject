using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScrollView : MonoBehaviour
{
    public Transform center;
    public List<RectTransform> viewList = new List<RectTransform>();
    public Button left;
    public Button right;

    [SerializeField] private float distance;
    [SerializeField] private float degree;
    [SerializeField] private int centerRotateValue = 720;
    [SerializeField] private int rotateMuiltplier = 5;
    [SerializeField] private Vector3 defaultScale;
    [SerializeField] private Vector3 upScale;

    private int index;

    void OpeningAnimation()
    {
        StartCoroutine(Opening(Opened));
    }

    void Opened()
    {
        index = 0;
        left.enabled = true;
        right.enabled = true;
    }

    void ChangeViewPanel(bool left)
    {
        if (!left && index == viewList.Count - 1)
        {
            index = 0;
        }
        else if(left && index == 0)
        {
            index = viewList.Count - 1;
        }
        else
        {
            index = left ? index - 1 : index + 1;
        }
        
        this.left.enabled = false;
        right.enabled = false;

        StartCoroutine(ChangingViewPanel(left));
    }

    void Awake()
    {
        left.onClick.AddListener(() => ChangeViewPanel(true));
        right.onClick.AddListener(() => ChangeViewPanel(false));
        left.enabled = false;
        right.enabled = false;
    }

    void Start()
    {
        OpeningAnimation();
    }

    void OnValidate()
    {
        for (int i = 0; i < viewList.Count; i++)
        {
            Transform v = viewList[i];
            float d = degree * Mathf.Deg2Rad * i;
            float x = distance * Mathf.Cos(d);
            float y = distance * Mathf.Sin(d);
            v.position = new Vector3(x, 0, y);
            v.rotation = Quaternion.Euler(new Vector3(0, -(90 + degree * i), 0));
        }
    }

    IEnumerator Opening(UnityAction done)
    {
        float elapsed = 0;
        float value = 0;
        while (value < centerRotateValue)
        {
            elapsed += Time.deltaTime;
            value = Mathf.Lerp(0, centerRotateValue, (elapsed * rotateMuiltplier) / centerRotateValue);
            center.eulerAngles = Vector3.up * value;
            yield return null;
        }

        yield return ScaleUpDown(true, done);
    }

    IEnumerator ScaleUpDown(bool isUp, UnityAction done)
    {
        Vector3 dst = isUp ? upScale : defaultScale;
        float elapsed = 0f;
        float duration = 0.3f;
        Vector3 value = Vector3.zero;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            foreach (var v in viewList)
            {
                v.localScale = Vector3.Lerp(v.localScale, dst, elapsed / duration);
            }

            yield return null;
        }

        done?.Invoke();
    }

    IEnumerator ChangingViewPanel(bool left)
    {
        yield return ScaleUpDown(false, null);

        float duration = 0.3f;
        float elapsed = 0, value = 0;
        //left - right +
        float currentY = center.eulerAngles.y;
        float d = left ? -degree : degree;
        d = center.eulerAngles.y + d;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            value = Mathf.Lerp(currentY, d, elapsed / duration);
            center.localEulerAngles = Vector3.up * value;
            yield return null;
        }

        yield return ScaleUpDown(true, null);

        this.left.enabled = true;
        right.enabled = true;
    }

}
