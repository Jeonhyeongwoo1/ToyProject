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

            _Tier = userData.userEXPData.tier;
            _CurExp = userData.userEXPData.curExp;
            _ItemList = userData.inventoryItemData.itemIdList;
        }

        public List<string> ItemList
        {
            get => _ItemList;
            set => _ItemList = value;
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

        public int Tier
        {
            get => _Tier;
            set => _Tier = value;
        }

        public int CurExp
        {
            get => _CurExp;
            set => _CurExp = value;
        }

        public int CurMaxExp
        {
            get => _CurMaxExp;
            set => _CurMaxExp = value;
        }

        public bool IsOpenedBattlePass
        {
            get => _IsOpenedBattlePass;
            set => _IsOpenedBattlePass = value;
        }

        private int _Star;
        private int _Pearl;
        private int _Gold;
        private int _Diamond;

        private int _Tier;
        private int _CurExp;
        private int _CurMaxExp;
        private bool _IsOpenedBattlePass;
        private List<string> _ItemList;
    }
}