using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace ItemInventory
{
    public class InventoryHeaderUI : MonoBehaviour
    {
        [SerializeField] private Button _AllTypeButton;
        [SerializeField] private Button _WeaponTypeButton;
        [SerializeField] private Button _ArmorTypeButton;
        [SerializeField] private Button _ACCTypeButton;
        [SerializeField] private Button _PortionTypeButton;
        [SerializeField] private InventoryUI _InventoryUI;

        private void Start()
        {
            _AllTypeButton.OnClickAsObservable()
                            .Subscribe((v) => _InventoryUI.ChangeInventoryType(InventoryType.All));

            _ArmorTypeButton.OnClickAsObservable()
                            .Subscribe((v) => _InventoryUI.ChangeInventoryType(InventoryType.Armor));

            _WeaponTypeButton.OnClickAsObservable()
                            .Subscribe((v) => _InventoryUI.ChangeInventoryType(InventoryType.Weapon));

            _ACCTypeButton.OnClickAsObservable()
                            .Subscribe((v) => _InventoryUI.ChangeInventoryType(InventoryType.ACC));

            _PortionTypeButton.OnClickAsObservable()
                                .Subscribe((v) => _InventoryUI.ChangeInventoryType(InventoryType.Portion));

        }
    }
}