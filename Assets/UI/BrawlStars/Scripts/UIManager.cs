using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlStars
{
    public class UIManager : Singleton<UIManager>
    {
        public DayGiftData DayGiftCard => dayGiftData;

        [SerializeField] private DayGiftData dayGiftData;

    }
}
