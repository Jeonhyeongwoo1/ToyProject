using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
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
            public Image[] characterImageArray;

            public Image currentImage;

            public void ChangeBrawler()
            {
                Image image = null;
                for (int i = 0; i < characterImageArray.Length; i++)
                {
                    if (characterImageArray[i] != currentImage)
                    {
                        image = characterImageArray[i];
                        break;
                    }
                }

                if (image == null)
                {
                    return;
                }

                currentImage.DOFade(0, 0.5f);
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                image.gameObject.SetActive(true);
                image.DOFade(1, 0.5f)
                        .OnComplete(() => currentImage = image);
            }
        }

        [Serializable]
        internal class GadgetSelectionPanel
        {
            public Image[] characterImageArray;
            public Image[] gadgetImageArray;
            public List<Image> currentImageList;
            public int currentImageIndex;

            public void ChangeBrawler()
            {
                List<Image> imageList = new List<Image>(currentImageList.Count);

                int index = currentImageIndex == 0 ? 1 : 0;
                imageList.Add(characterImageArray[index * 2]);
                imageList.Add(characterImageArray[index * 2 + 1]);

                for (int i = 0; i < imageList.Count; i++)
                {
                    currentImageList[i].DOFade(0, 0.5f);
                    imageList[i].color = new Color(imageList[i].color.r, imageList[i].color.g, imageList[i].color.b, 0);
                    imageList[i].gameObject.SetActive(true);
                    imageList[i].DOFade(1, 0.5f);
                    currentImageList[i] = imageList[i];
                }

                currentImageIndex = index;
            }
        }

        [Serializable]
        internal class NewPinPanel
        {
            public Image[] characterImageArray;
            public Text[] characterTextArray;

            public Image currentImage;

            public void ChangeBrawler()
            {
                Image image = null;
                for (int i = 0; i < characterImageArray.Length; i++)
                {
                    if (characterImageArray[i] != currentImage)
                    {
                        image = characterImageArray[i];
                        break;
                    }
                }

                if (image == null)
                {
                    return;
                }

                currentImage.DOFade(0, 0.5f);
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                image.gameObject.SetActive(true);
                image.DOFade(1, 0.5f)
                        .OnComplete(() => currentImage = image);
            }
        }

        [Serializable]
        internal class NewSkinPanel
        {
            public List<GameObject> brawlerModelList;
            public GameObject currentModel;
            public bool isChangeBralwer;

            public void ChangeBrawler()
            {
                if (!isChangeBralwer)
                {
                    return;
                }

                GameObject model = brawlerModelList.Find((v) => currentModel != v);
                model.gameObject.SetActive(true);
                currentModel.SetActive(false);

                currentModel = model;
            }
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

        private GameObject CreateBrawlerModel(string brawlerName)
        {
            GameObject go = Resources.Load<GameObject>(modelPath + brawlerName);
            GameObject character = Instantiate(go, transform);
            character.transform.localScale = Vector3.one * 200;
            character.transform.localPosition = new Vector3(0, -200, -70);
            character.transform.localEulerAngles = new Vector3(0, 180, 0);
            character.SetActive(false);
            return character;
        }

        public void TestCode()
        {
            switch (dayGiftCardType)
            {
                case DayGiftCardType.BrawlerSelection:
                    break;
                case DayGiftCardType.NewBrawlerSkin:
                    newSkinPanel.ChangeBrawler();
                    break;
                case DayGiftCardType.StarPowerSelection:
                    break;
                case DayGiftCardType.BrawlerUpgrade:
                    brawlerUpgradePanel.ChangeBrawler();
                    break;
                case DayGiftCardType.NewPin:
                    newPinPanel.ChangeBrawler();
                    break;
                case DayGiftCardType.GadgetSelection:
                    gadgetSelectionPanel.ChangeBrawler();
                    break;
            }
        }

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
                        GameObject model = CreateBrawlerModel(brawlerCardData.newBrawlerSkinData.firstBrawlerName);
                        newSkinPanel.currentModel = model;
                        newSkinPanel.isChangeBralwer = false;
                        newSkinPanel.currentModel.SetActive(true);
                        break;
                    }

                    if (brawlerCardData.newBrawlerSkinData.lastBrawlerNameList.Count > 0)
                    {
                        int count = brawlerCardData.newBrawlerSkinData.lastBrawlerNameList.Count;
                        newSkinPanel.brawlerModelList = new List<GameObject>(count);
                        for (int i = 0; i < count; i++)
                        {
                            GameObject model = CreateBrawlerModel(brawlerCardData.newBrawlerSkinData.lastBrawlerNameList[i]);
                            newSkinPanel.brawlerModelList.Add(model);
                            model.SetActive(true);
                            model.transform.localPosition += new Vector3(i == 0 ? -150 : 150, 0);
                        }

                        newSkinPanel.isChangeBralwer = false;
                        break;
                    }

                    newSkinPanel.brawlerModelList = new List<GameObject>(bralwerNameList.Count);
                    for (int i = 0; i < bralwerNameList.Count; i++)
                    {
                        GameObject model = CreateBrawlerModel(bralwerNameList[i]);
                        newSkinPanel.brawlerModelList.Add(model);
                    }

                    newSkinPanel.currentModel = newSkinPanel.brawlerModelList[0];
                    newSkinPanel.isChangeBralwer = true;
                    newSkinPanel.currentModel.SetActive(true);
                    break;
                case DayGiftCardType.StarPowerSelection:
                    break;
                case DayGiftCardType.BrawlerUpgrade:

                    for (int i = 0; i < bralwerNameList.Count; i++)
                    {
                        Sprite brawler = Resources.Load<Sprite>(imagePath + bralwerNameList[i]);
                        brawlerUpgradePanel.characterImageArray[i].sprite = brawler;
                        brawlerUpgradePanel.characterImageArray[i].gameObject.SetActive(false);
                    }

                    brawlerUpgradePanel.currentImage = brawlerUpgradePanel.characterImageArray[0];
                    brawlerUpgradePanel.currentImage.gameObject.SetActive(true);
                    brawlerUpgradePanel.powerText.text = brawlerCardData.brawlerUpgradeData.powerNumber.ToString();
                    break;
                case DayGiftCardType.NewPin:

                    for (int i = 0; i < newPinPanel.characterImageArray.Length; i++)
                    {
                        newPinPanel.characterImageArray[i].sprite = Resources.Load<Sprite>(imagePath + bralwerNameList[i]);
                        newPinPanel.characterImageArray[i].gameObject.SetActive(false);
                    }

                    for (int i = 0; i < brawlerCardData.newPinData.pinName.Count; i++)
                    {
                        newPinPanel.characterTextArray[i].text = brawlerCardData.newPinData.pinName[i];
                        //    newPinPanel.characterTextArray[i].gameObject.SetActive(false);
                    }

                    newPinPanel.currentImage = newPinPanel.characterImageArray[0];
                    newPinPanel.currentImage.gameObject.SetActive(true);
                    break;
                case DayGiftCardType.GadgetSelection:

                    gadgetSelectionPanel.currentImageList = new List<Image>(bralwerNameList.Count);
                    for (int i = 0; i < bralwerNameList.Count; i++)
                    {
                        Sprite sprite = Resources.Load<Sprite>(imagePath + bralwerNameList[i]);
                        gadgetSelectionPanel.characterImageArray[i * 2].sprite = sprite;
                        gadgetSelectionPanel.characterImageArray[i * 2 + 1].sprite = sprite;
                        gadgetSelectionPanel.characterImageArray[i * 2].gameObject.SetActive(false);
                        gadgetSelectionPanel.characterImageArray[i * 2 + 1].gameObject.SetActive(false);
                    }

                    gadgetSelectionPanel.characterImageArray[0].gameObject.SetActive(true);
                    gadgetSelectionPanel.characterImageArray[1].gameObject.SetActive(true);
                    gadgetSelectionPanel.currentImageList.Add(gadgetSelectionPanel.characterImageArray[0]);
                    gadgetSelectionPanel.currentImageList.Add(gadgetSelectionPanel.characterImageArray[1]);
                    gadgetSelectionPanel.currentImageIndex = 0;
                    break;
            }
        }
    }
}
