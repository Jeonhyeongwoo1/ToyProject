using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace StumbleGuyz
{
    //View의 역할을 한다.
    public class MainUI : MonoBehaviour
    {
        [SerializeField] private Text _PlayerRankText;
        [SerializeField] private Text _PlayerCrownText;
        [SerializeField] private Text _PlayerNameText;
        [SerializeField] private Text _PlayerCoinText;
        [SerializeField] private Text _PlayerGemText;

        [SerializeField] private Button _StoreButton;
        [SerializeField] private Button _CustomizingButton;
        [SerializeField] private Button _RankButton;

        [SerializeField] private FormMover _StoreFormMover;
        [SerializeField] private FormMover _ProfileFormMover;

        // Start is called before the first frame update
        private void Start()
        {
            _StoreButton.OnClickAsObservable()
                        .Subscribe((v) => OpenStoreForm());

            _CustomizingButton.OnClickAsObservable()
                                .Subscribe((v) => Debug.Log("OnClick Customizing"));

            _RankButton.OnClickAsObservable()
                            .Subscribe((v) => Debug.Log("OnClick Rank"));
        }

        public void OnClickPlayerName()
        {
            _ProfileFormMover.Move();
        }

        public void Init(string rank, string crown, string coin, string gem, string playerName)
        {
            _PlayerRankText.text = rank;
            _PlayerCrownText.text = crown;
            _PlayerNameText.text = playerName;
            _PlayerCoinText.text = coin;
            _PlayerGemText.text = gem;
        }

        public void SetPlayerRank(string rank)
        {
            _PlayerRankText.text = rank;
        }

        public void SetPlayerCrown(string crown)
        {
            _PlayerCrownText.text = crown;
        }

        public void SetPlayerName(string name)
        {
            _PlayerNameText.text = name;
        }

        public void SetPlayerCoin(string coin)
        {
            _PlayerCoinText.text = coin;
        }

        public void SetPlayerGem(string gem)
        {
            _PlayerCoinText.text = gem;
        }

        private void OpenStoreForm()
        {
            _StoreFormMover.Move();
        }
    }
}