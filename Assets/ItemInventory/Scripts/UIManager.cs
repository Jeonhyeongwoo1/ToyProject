using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace ItemInventory
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Inventory _Inventroy;

        private void Start()
        {
            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.I))
                        .Subscribe((v) => _Inventroy.Open());

            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown((KeyCode.Escape)))
                        .Subscribe((v) => _Inventroy.Close());
        }
    }
}
