using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InfiniteUIController : MonoBehaviour
{
    [SerializeField] private Button testBtn;

    [SerializeField] private RectTransform cardPrefab;
    [SerializeField] private RectTransform cardParent;

    [SerializeField] private float firstCardAxisX;
    [SerializeField] private float cardPositionInterval = 100;
    [SerializeField] private int cardCreateCount = 30;
    [SerializeField] private float rollingDuration = 10;
    [SerializeField] private AnimationCurve rollingCurve;

    [SerializeField] private List<RectTransform> cardList = new List<RectTransform>();

    void Awake()
    {

        for (int i = 0; i < cardCreateCount; i++)
        {
            RectTransform card = Instantiate<RectTransform>(cardPrefab, Vector3.one, Quaternion.identity, cardParent);
            float x = card.rect.width * i + firstCardAxisX + cardPositionInterval * i;
            card.anchoredPosition = new Vector2(x, 0);
            cardList.Add(card);
        }

        testBtn.onClick.AddListener(OnStartRollingCard);
    }

    private void OnStartRollingCard()
    {
        // axisX + interval
        StartCoroutine(RollingCard());
    }

    private IEnumerator RollingCard()
    {
        float elapsed = 0;
        float start = cardParent.anchoredPosition.x;
        float end = -cardList[cardList.Count - 2].anchoredPosition.x;
        Vector2 value = Vector2.zero;

        while (elapsed < rollingDuration)
        {
            elapsed += Time.deltaTime;
            float x = Mathf.LerpUnclamped(start, end, rollingCurve.Evaluate(elapsed / rollingDuration));
            value.x = x;
            cardParent.anchoredPosition = value;
            yield return null;
        }
    }





}
