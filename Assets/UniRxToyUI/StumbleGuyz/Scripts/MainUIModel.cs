using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace StumbleGuyz
{
    public class MainUIModel : MonoBehaviour
    {
        public IReactiveProperty<int> PlayerRank => _PlayerRank;
        public IReactiveProperty<int> Crown => _Crown;
        public IReactiveProperty<int> Coin => _Coin;
        public IReactiveProperty<int> Gem => _Gem;
        public IReactiveProperty<string> PlayerName => _PlayerName;

        private IntReactiveProperty _PlayerRank = new IntReactiveProperty();
        private IntReactiveProperty _Crown = new IntReactiveProperty();
        private IntReactiveProperty _Coin = new IntReactiveProperty();
        private IntReactiveProperty _Gem = new IntReactiveProperty();
        private StringReactiveProperty _PlayerName = new StringReactiveProperty();

        public void Init(int playerRank, int crown, int coin, int gem, string playerName)
        {
            _PlayerRank.Value = playerRank;
            _Crown.Value = crown;
            _Coin.Value = coin;
            _Gem.Value = gem;
            _PlayerName.Value = playerName;
        }
    }
}
