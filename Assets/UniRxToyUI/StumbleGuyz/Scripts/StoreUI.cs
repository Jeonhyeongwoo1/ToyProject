using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

namespace StumbleGuyz
{
    public class StoreUI : MonoBehaviour
    {
        public enum CardType
        {
            Skin,
            Suggest,
            Gem
        }

        [System.Serializable]
        public class DummyCardData
        {
            public CardType cardType;
            public string title;
            public Vector2 sizeDelta;
        }

        [SerializeField] private Button _GoToMainButton;
        [SerializeField] private Button _SkinButton;
        [SerializeField] private Button _SuggestButton;
        [SerializeField] private Button _GemButton;

        [SerializeField] private Transform _StoreCardContentTrasnform;
        [SerializeField] private StoreCardForm _StoreCardFormPrefab;
        [SerializeField] private RectTransform _ContentTypeCardFormPrefab;
        [SerializeField] private ScrollRect scrollRect;

        [SerializeField] private Text _GemText;
        [SerializeField] private Text _CoinText;

        [SerializeField] private FormMover _MainFormMover;
        [SerializeField] private List<DummyCardData> dummyCardDataList = new List<DummyCardData>();

        private List<RectTransform> _CardTypeList = new List<RectTransform>();

        public void SetGemValue(string value)
        {
            _GemText.text = value;
        }

        public void SetCoinValue(string value)
        {
            _CoinText.text = value;
        }

        // Start is called before the first frame update
        private void Start()
        {
            _GoToMainButton.OnClickAsObservable()
                            .Subscribe((v) => OpenMainForm());

            _SkinButton.OnClickAsObservable()
                        .Subscribe((v) => OnClickSkinButton());

            _SuggestButton.OnClickAsObservable()
                            .Subscribe((v) => OnClickSuggestButton());

            _GemButton.OnClickAsObservable()
                        .Subscribe((v) => OnClickGemButton());

            SetCardList();
        }

        private void SetCardList()
        {
            List<DummyCardData> skinCardList = dummyCardDataList.FindAll((v) => v.cardType == CardType.Skin);

            RectTransform skinTypeCardForm = Instantiate<RectTransform>(_ContentTypeCardFormPrefab, _StoreCardContentTrasnform);
            skinTypeCardForm.name = CardType.Skin.ToString();

            foreach (DummyCardData skinCard in skinCardList)
            {
                StoreCardForm card = Instantiate<StoreCardForm>(_StoreCardFormPrefab, skinTypeCardForm);
                card.Init(skinCard.sizeDelta, skinCard.title);
                skinTypeCardForm.sizeDelta += new Vector2(skinCard.sizeDelta.x, 0);
            }

            List<DummyCardData> suggestCardList = dummyCardDataList.FindAll((v) => v.cardType == CardType.Suggest);

            RectTransform suggestTypeCardForm = Instantiate<RectTransform>(_ContentTypeCardFormPrefab, _StoreCardContentTrasnform);
            suggestTypeCardForm.name = CardType.Suggest.ToString();
            foreach (DummyCardData suggestCard in suggestCardList)
            {
                StoreCardForm card = Instantiate<StoreCardForm>(_StoreCardFormPrefab, suggestTypeCardForm);
                card.Init(suggestCard.sizeDelta, suggestCard.title);
                suggestTypeCardForm.sizeDelta += new Vector2(suggestCard.sizeDelta.x, 0);
            }

            List<DummyCardData> gemCardList = dummyCardDataList.FindAll((v) => v.cardType == CardType.Gem);

            RectTransform gemTypeCardForm = Instantiate<RectTransform>(_ContentTypeCardFormPrefab, _StoreCardContentTrasnform);
            gemTypeCardForm.name = CardType.Gem.ToString();
            foreach (DummyCardData gemCard in gemCardList)
            {
                StoreCardForm card = Instantiate<StoreCardForm>(_StoreCardFormPrefab, gemTypeCardForm);
                card.Init(gemCard.sizeDelta, gemCard.title);
                gemTypeCardForm.sizeDelta += new Vector2(gemCard.sizeDelta.x, 0);
            }

            _CardTypeList.Add(skinTypeCardForm);
            _CardTypeList.Add(suggestTypeCardForm);
            _CardTypeList.Add(gemTypeCardForm);
        }

        private void OnClickGemButton()
        {
            RectTransform gem = _CardTypeList.Find((v) => v.name == CardType.Gem.ToString());
            StartCoroutine(ScrollViewFocusFunctions.FocusOnItemCoroutine(scrollRect, gem, 5));

            _GemButton.transform.GetChild(0).gameObject.SetActive(true);
            _SuggestButton.transform.GetChild(0).gameObject.SetActive(false);
            _SkinButton.transform.GetChild(0).gameObject.SetActive(false);
        }

        private void OnClickSuggestButton()
        {
            RectTransform gem = _CardTypeList.Find((v) => v.name == CardType.Suggest.ToString());
            StartCoroutine(ScrollViewFocusFunctions.FocusOnItemCoroutine(scrollRect, gem, 5));

            _GemButton.transform.GetChild(0).gameObject.SetActive(false);
            _SuggestButton.transform.GetChild(0).gameObject.SetActive(true);
            _SkinButton.transform.GetChild(0).gameObject.SetActive(false);
        }

        private void OnClickSkinButton()
        {
            RectTransform gem = _CardTypeList.Find((v) => v.name == CardType.Skin.ToString());
            StartCoroutine(ScrollViewFocusFunctions.FocusOnItemCoroutine(scrollRect, gem, 5));

            _GemButton.transform.GetChild(0).gameObject.SetActive(false);
            _SuggestButton.transform.GetChild(0).gameObject.SetActive(false);
            _SkinButton.transform.GetChild(0).gameObject.SetActive(true);
        }

        private void MoveTo(Vector2 endPos)
        {
            _StoreCardContentTrasnform.TryGetComponent<RectTransform>(out var rectTransform);
            rectTransform.DOAnchorPos(endPos, duration: 0.3f);
        }

        private void OpenMainForm()
        {
            _MainFormMover.Move();
        }
    }
}