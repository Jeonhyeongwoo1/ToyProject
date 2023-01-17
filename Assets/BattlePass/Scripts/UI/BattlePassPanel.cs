using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UniRx;

namespace BattlePass
{
    public class BattlePassPanel : MonoBehaviour
    {
        [SerializeField] private BattlePassElement _BattlePassElement;
        [SerializeField] private Transform _BattlePassContentTransform;

        [Header("[GoodsValueList]")]
        [SerializeField] private Text _StarText;
        [SerializeField] private Text _PearlText;
        [SerializeField] private Text _GoldText;
        [SerializeField] private Text _DiamondText;

        [SerializeField] private Button _InventoryButton;
        [SerializeField] private Button _AddDiamondButton;

        private List<BattlePassElement> _BattlePassElementList = new List<BattlePassElement>();

        public void UpdateBattlePassTier(int index, string tier)
        {
            _BattlePassElementList[index]!.UpdateTier(tier);
        }

        public void UpdateBattlePassElementUI(int index, bool isLock, Sprite itemSprite, int itemValue)
        {
            _BattlePassElementList[index]!.UpdateBattlePassElementUI(isLock, itemSprite, itemValue);
        }

        public void UpdateFreePassElementUI(int index, bool isLock, Sprite itemSprite, int itemValue)
        {
            _BattlePassElementList[index]!.UpdateFreePassElementUI(isLock, itemSprite, itemValue);
        }

        [Button]
        public void CreateBattlePassElement(int count)
        {
            for (int i = 0; i < count; i++)
            {
                BattlePassElement element = Instantiate(_BattlePassElement, _BattlePassContentTransform);
                _BattlePassElementList.Add(element);
            }
        }

        [Button]
        private void ClearBattlePassElement()
        {
            for (int i = 0; i < _BattlePassElementList.Count; i++)
            {
                Destroy(_BattlePassElementList[i].gameObject);
            }

            _BattlePassElementList.Clear();
        }

        private void OnClickInventoryButton()
        {

        }

        private void OnClickAddDiamondButton()
        {

        }

        private void Start()
        {
            _InventoryButton.OnClickAsObservable()
                            .Subscribe((v) => OnClickInventoryButton());

            _AddDiamondButton.OnClickAsObservable()
                            .Subscribe((v) => OnClickAddDiamondButton());
        }
    }

}