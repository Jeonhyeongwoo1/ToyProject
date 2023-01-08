using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragableItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private RectTransform rectTransform;

    private Vector2 _OriginPos;

    public void Init(Vector2 slotSize)
    {
        rectTransform.sizeDelta = slotSize;
    }

    public void OnBeginDrag(Image image, Vector2 startPos)
    {
        itemImage.sprite = image.sprite;
        rectTransform.anchoredPosition = startPos;
        _OriginPos = rectTransform.anchoredPosition;
        gameObject.SetActive(true);
    }

    public void OnDrag(Vector2 pos)
    {
        transform.position = _OriginPos + pos;
    }

    public void OnEndDrag()
    {
        DisableDragableItem();
    }

    public void OnDrop()
    {
        DisableDragableItem();
    }

    private void DisableDragableItem()
    {
        itemImage.sprite = null;
        rectTransform.anchoredPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
