using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BrawlStars
{
    public enum DayGiftCardType
    {
        BrawlerSelection = 1,
        NewBrawlerSkin,
        StarPowerSelection,
        BrawlerUpgrade,
        NewPin,
        GadgetSelection
    }

    [Serializable]
    public class DayGiftData
    {
        public string title;
        public string description;
        public List<string> brawlerNameList = new List<string>();
        public List<BrawlerCardData> brawlerCardDataList = new List<BrawlerCardData>();
    }

    [Serializable]
    public class BrawlerCardData
    {
        public int day;
        public int dayGiftCardType;
        public string title;
        public BrawlerSelectionData brawlerSelectionData;
        public NewBrawlerSkinData newBrawlerSkinData;
        public StarPowerSelectionData starPowerSelectionData;
        public BrawlerUpgradeData brawlerUpgradeData;
        public NewPinData newPinData;
        public GadgetSelectionData gadgetSelectionData;
    }

    [Serializable]
    public class BrawlerCardDataBase
    {
    }

    [Serializable]
    public class NewBrawlerSkinData : BrawlerCardDataBase
    {
        public string firstBrawlerName;
        public List<string> lastBrawlerNameList;
    }

    [Serializable]
    public class BrawlerUpgradeData : BrawlerCardDataBase
    {
        public int powerNumber;
    }

    [Serializable]
    public class NewPinData : BrawlerCardDataBase
    {
        public List<string> pinName = new List<string>();
    }

    [Serializable]
    public class GadgetSelectionData : BrawlerCardDataBase
    {
        public List<string> gadgetNameList = new List<string>();
    }

    [Serializable]
    public class BrawlerSelectionData : BrawlerCardDataBase
    {
    }

    [Serializable]
    public class StarPowerSelectionData : BrawlerCardDataBase
    {
    }
}
