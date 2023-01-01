using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Sirenix.OdinInspector;

namespace BrawlStars
{
    public class DayGiftCardPanel : MonoBehaviour
    {
        [SerializeField] private Button _GoToHomeButton;
        [SerializeField] private Text _TitleText;
        [SerializeField] private Text _DescriptionText;
        [SerializeField] private Transform _DayGiftCardContent;
        [SerializeField] private DayGiftCard _DayGiftCardPrefab;

        private List<DayGiftCard> cardList = new List<DayGiftCard>();

        private void Start()
        {
            _GoToHomeButton.OnClickAsObservable()
                            .Subscribe((v) => Debug.Log("Goto home"));
        }

        [Button]
        private void CreateDayGiftCard()
        {
            DayGiftData dayGiftData = UIManager.Instance.DayGiftCard;

            _TitleText.text = dayGiftData.title;
            _DescriptionText.text = dayGiftData.description;

            int count = dayGiftData.brawlerCardDataList.Count;
            for (int i = 0; i < count; i++)
            {
                DayGiftCard card = Instantiate(_DayGiftCardPrefab, _DayGiftCardContent);
                if (i == count - 1)
                {
                    card.GetComponent<RectTransform>().sizeDelta = new Vector2(650, 0);
                }
                card.Init(dayGiftData.brawlerNameList, dayGiftData.brawlerCardDataList[i]);
                cardList.Add(card);

            }
        }

        [Button]
        private void TestCode()
        {
            Observable.FromCoroutine(BrawlerChange).Subscribe().AddTo(gameObject);
        }

        private IEnumerator BrawlerChange()
        {
            while (gameObject.activeSelf)
            {

                foreach (DayGiftCard card in cardList)
                {
                    card.TestRotate();
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(2f);
            }
        }

    }
}
