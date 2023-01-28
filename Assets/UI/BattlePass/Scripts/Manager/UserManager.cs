using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace BattlePass
{
    public class UserManager : Singleton<UserManager>
    {
        [SerializeField] private UserData _UserData;
        [SerializeField] private EXPData _EXPData;

        private User _User;

        private void Start()
        {
            _User = new User(_UserData);
            _User.CurMaxExp = _EXPData.expList.Find((v) => v.tier == _User.Tier).maxExp;
            UIManager.Instance.SetUserEXP(_User.CurExp, _User.CurMaxExp);
            UIManager.Instance.SetUserGoods(_User.Star, _User.Pearl, _User.Gold, _User.Diamond);
        }

        public void GainItem(string id)
        {
            if (_User.ItemList == null)
            {
                _User.ItemList = new List<string>();
                _User.ItemList.Add(id);
            }

            if (!_User.ItemList.Contains(id))
            {
                _User.ItemList.Add(id);
            }
        }

        public List<string> GetUserItemData() => _User.ItemList;
        public void UpdateUserStar(int count) => _User.Star += count;
        public void UpdateUserDiamond(int count) => _User.Diamond += count;
        public void UpdateUserPearl(int count) => _User.Pearl += count;
        public void UpdateUserGold(int count) => _User.Gold += count;
        public int GetUserTier() => _User.Tier;
        public int GetUserStar() => _User.Star;
        public int GetUserDiamond() => _User.Diamond;
        public int GetUserPearl() => _User.Pearl;
        public int GetUserGold() => _User.Gold;
        public bool IsOpenedBattlePass() => _User.IsOpenedBattlePass;

        public void UpdateUserEXP(int exp)
        {
            _User.CurExp += exp;

            if (_User.CurExp >= _User.CurMaxExp)
            {
                Debug.Log("Tier Update");
                int curTier = _User.Tier;
                if (curTier >= _EXPData.maxTier)
                {
                    Debug.Log("maxTier");
                    _User.CurExp = _User.CurMaxExp;
                    UIManager.Instance.SetUserEXP(_User.CurExp, _User.CurMaxExp);
                    return;
                }

                int curExp = _User.CurExp;
                EXPData.EXP eXP = _EXPData.expList.Find((v) => v.tier == curTier + 1);
                _User.CurExp = curExp - _User.CurMaxExp;
                _User.Tier = eXP.tier;
                _User.CurMaxExp = eXP.maxExp;
            }

            UIManager.Instance.UpdateTier(_User.Tier, _User.IsOpenedBattlePass);
            UIManager.Instance.SetUserEXP(_User.CurExp, _User.CurMaxExp);
        }

        public bool UseDiamond(int diamond)
        {
            if(diamond > _User.Diamond)
            {
                Debug.Log("need more a diamond");
                return false;
            }

            _User.IsOpenedBattlePass = true;
            AddUserDiamond(-diamond);
            return true;
        }

        public void AddUserDiamond(int count)
        {
            _User.Diamond += count;

            UIManager.Instance.UpdateUserDiamondUI(_User.Diamond);
        }
    }
}