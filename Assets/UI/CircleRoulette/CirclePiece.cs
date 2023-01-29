using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CircleRoulette
{
    public class CirclePiece : MonoBehaviour
    {
        [SerializeField] private Image _ItemImage;
        [SerializeField] private Text _DescriptionText;

        public void Setup(Sprite itemSprite, string description)
        {
            _ItemImage.sprite = itemSprite;
            _DescriptionText.text = description;
        }
    }
}