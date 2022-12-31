using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;

namespace BrawlStars
{
    public class DayGiftCardContentPanel : MonoBehaviour
    {
        [Serializable]
        internal class BrawlerSelectionPanel
        {
            public GameObject selectionImageObject;
            public Image[] characterImageArray;
        }

        [Serializable]
        internal class BrawlerUpgradePanel
        {
            public Text powerText;
            public Image characterImage;
        }

        [Serializable]
        internal class GadgetSelectionPanel
        {
            public Image[] characterImageArray;
            public Image[] gadgetImageArray;
        }

        [Serializable]
        internal class NewPinPanel
        {
            public Image characterImage;
            public Text characterText;
        }

        [Serializable]
        internal class NewSkinPanel
        {
            public GameObject model;
        }

        [Serializable]
        internal class StarPowerSelectionPanel
        {
            public Image[] characterImageArray;
            public Image[] characterStarImageArray;
        }

        public DayGiftCardType DayGiftCardType => dayGiftCardType;

        [EnumPaging]
        [SerializeField] private DayGiftCardType dayGiftCardType;

        [ShowIf(nameof(dayGiftCardType), DayGiftCardType.BrawlerSelection)]
        [SerializeField] private BrawlerSelectionPanel brawlerSelectionPanel;

        [ShowIf(nameof(dayGiftCardType), DayGiftCardType.BrawlerUpgrade)]
        [SerializeField] private BrawlerUpgradePanel brawlerUpgradePanel;

        [ShowIf(nameof(dayGiftCardType), DayGiftCardType.GadgetSelection)]
        [SerializeField] private GadgetSelectionPanel gadgetSelectionPanel;

        [ShowIf(nameof(dayGiftCardType), DayGiftCardType.NewPin)]
        [SerializeField] private NewPinPanel newPinPanel;

        [ShowIf(nameof(dayGiftCardType), DayGiftCardType.NewBrawlerSkin)]
        [SerializeField] private NewSkinPanel newSkinPanel;

        [ShowIf(nameof(dayGiftCardType), DayGiftCardType.StarPowerSelection)]
        [SerializeField] private StarPowerSelectionPanel starPowerSelectionPanel;

        private readonly string imagePath = "Image/";
        private readonly string modelPath = "Model/";

        public void Init(List<string> bralwerNameList, BrawlerCardData brawlerCardData)
        {

            switch (dayGiftCardType)
            {
                case DayGiftCardType.BrawlerSelection:
                    for (int i = 0; i < bralwerNameList.Count; i++)
                    {
                        brawlerSelectionPanel.characterImageArray[i].sprite = Resources.Load<Sprite>(imagePath + bralwerNameList[i]);
                    }


                    break;
                case DayGiftCardType.NewBrawlerSkin:

                    if (!string.IsNullOrEmpty(brawlerCardData.newBrawlerSkinData.firstBrawlerName))
                    {
                        GameObject go = Resources.Load<GameObject>(modelPath + brawlerCardData.newBrawlerSkinData.firstBrawlerName);
                        GameObject character = Instantiate(go, transform);
                        character.transform.localScale = Vector3.one * 200;
                        character.transform.localPosition = new Vector3(0, -200, -70);
                        character.transform.localEulerAngles = new Vector3(0, 180, 0);
                    }

                    if (brawlerCardData.newBrawlerSkinData.lastBrawlerNameList != null)
                    {
                        int count = brawlerCardData.newBrawlerSkinData.lastBrawlerNameList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            GameObject go = Resources.Load<GameObject>(modelPath + brawlerCardData.newBrawlerSkinData.lastBrawlerNameList[i]);
                            GameObject character = Instantiate(go, transform);
                            character.transform.localScale = Vector3.one * 200;
                            character.transform.localPosition = new Vector3(0, -200, -70);
                            character.transform.localEulerAngles = new Vector3(0, 180, 0);
                        }
                    }
                    break;
                case DayGiftCardType.StarPowerSelection:
                    break;
                case DayGiftCardType.BrawlerUpgrade:
                    break;
                case DayGiftCardType.NewPin:
                    break;
                case DayGiftCardType.GadgetSelection:
                    break;
            }
        }
    }
}
