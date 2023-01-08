using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ItemInventory
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private Image _ItemImage;
        [SerializeField] private Text _CountText;
        [SerializeField] private Text _ItemLevelText;
        [SerializeField] private RectTransform _RectTransform;

        public void Init(Vector2 size)
        {
            _RectTransform.sizeDelta = size;
        }
    }
}
