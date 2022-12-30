using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;

namespace StumbleGuyz
{
    public class MainUIPresenter : MonoBehaviour
    {
        [SerializeField] private ProfileUI _ProfileUI;
        [SerializeField] private StoreUI _StoreUI;
        [SerializeField] private MainUI _MainUI;
        [SerializeField] private MainUIModel _MainUIModel;

        public void Init(int playerRank, int crown, int coin, int gem, string playerName)
        {
            _MainUIModel.Init(playerRank, crown, coin, gem, playerName);
            _MainUI.Init(playerRank.ToString(), crown.ToString(), coin.ToString(), gem.ToString(), playerName);
        }

        [Button]
        public void TestCode()
        {
            Init(playerRank: 3, crown: 10, coin: 100000, gem: 400, playerName: "Jeonhyeongwoo");
        }

        private void Start()
        {
            _MainUIModel.Gem.AsObservable()
                            .Subscribe((v) =>
                            {
                                _StoreUI.SetGemValue(v.ToString());
                                _MainUI.SetPlayerGem(v.ToString());
                            });

            _MainUIModel.Coin.AsObservable()
                            .Subscribe((v) =>
                            {
                                _StoreUI.SetCoinValue(v.ToString());
                                _MainUI.SetPlayerCoin(v.ToString());
                            });

            _MainUIModel.PlayerName.AsObservable()
                                    .Subscribe((v) =>
                                    {
                                        _ProfileUI.SetPlayerName(v);
                                        _MainUI.SetPlayerName(v);
                                    });

            _MainUIModel.Crown.AsObservable()
                                .Subscribe((v) =>
                                {
                                    _ProfileUI.SetCrownCount(v.ToString());
                                });

            _MainUIModel.PlayerRank.AsObservable()
                                .Subscribe((v) =>
                                {
                                    _ProfileUI.SetRankCount(v.ToString());
                                });

        }
    }
}