using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace ItemInventory
{
    public class UIManager : Singleton<UIManager>
    {
        public InventoryConfigData InventoryConfigData => _InventoryConfigData;

        [SerializeField] private Inventory _Inventory;
        [SerializeField] private InventoryUI _InventroyUI;
        [SerializeField] private InventoryConfigData _InventoryConfigData;

        [SerializeField] private List<ItemData> _TestItemData = new List<ItemData>();

        private void Start()
        {
            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.I))
                        .Subscribe((v) => _InventroyUI.Open());

            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown((KeyCode.Escape)))
                        .Subscribe((v) => _InventroyUI.Close());


            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown((KeyCode.Alpha2)))
                        .Subscribe((v) =>
                        {
                            foreach (var data in _TestItemData)
                            {
                                _Inventory.AddItemData(data);
                            }
                        });
        }
    }
}
