using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlePass
{
    public class User
    {
        public UserData userData { get; private set; }

        public User(UserData userData)
        {
            this.userData = userData;

            _Star = userData.userGoodsData.star;
            _Pearl = userData.userGoodsData.pearl;
            _Gold = userData.userGoodsData.gold;
            _Diamond = userData.userGoodsData.diamond;
        }

        public int Star
        {
            get => _Star;
            set => _Star = value;
        }

        public int Pearl
        {
            get => _Pearl;
            set => _Pearl = value;
        }

        public int Gold
        {
            get => _Gold;
            set => _Gold = value;
        }

        public int Diamond
        {
            get => _Diamond;
            set => _Diamond = value;
        }

        private int _Star;
        private int _Pearl;
        private int _Gold;
        private int _Diamond;
    }
}