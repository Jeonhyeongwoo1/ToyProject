using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;

namespace ItemInventory
{
    public class Inventory : MonoBehaviour
    {
        [ReadOnly][SerializeField] private List<Transform> _ItemList = new List<Transform>();

        private void Start()
        {
            Transform[] itemArray = GetComponentsInChildren<Transform>(true);
            _ItemList.AddRange(itemArray);
        }
    }
}
