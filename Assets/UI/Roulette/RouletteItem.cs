using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteItem : MonoBehaviour
{
    [SerializeField] private Text _ItemTitleText;
    [SerializeField] private Image _ItemImage;

    public void SetItemTitle(string title)
    {
        _ItemTitleText.text = title;
    }

    public void SetItemImageColor(Color itemColor)
    {
        _ItemImage.color = itemColor;
    }
}
