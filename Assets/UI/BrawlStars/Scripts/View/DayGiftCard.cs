using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace BrawlStars
{
    public class DayGiftCard : MonoBehaviour
    {
        [SerializeField] private Text _DayText;
        [SerializeField] private Text _TitleText;
        [SerializeField] private Transform _ContentTransform;
        [SerializeField] private List<DayGiftCardContentPanel> _DayGiftCardContentPanelPrefabList;

        private DayGiftCardContentPanel _SelectedDayGiftCardContentPanel;

        public void Init(List<string> bralwerNameList, BrawlerCardData brawlerCardData)
        {
            _DayText.text = brawlerCardData.day.ToString() + "일 차";
            _TitleText.text = brawlerCardData.title;
            DayGiftCardType type = (DayGiftCardType)brawlerCardData.dayGiftCardType;

            foreach (DayGiftCardContentPanel panel in _DayGiftCardContentPanelPrefabList)
            {
                if (panel.DayGiftCardType == type)
                {
                    DayGiftCardContentPanel contentPanel = Instantiate(panel, _ContentTransform);
                    contentPanel.Init(bralwerNameList, brawlerCardData);
                    _SelectedDayGiftCardContentPanel = contentPanel;
                }
            }
            
            try
            {

            }
            catch (System.Exception e)
            {
                Debug.LogError("Daygift card parse error" + e);
                return;
            }
        }

        public void TestRotate()
        {
            _SelectedDayGiftCardContentPanel.TestCode();
        }
    }
}