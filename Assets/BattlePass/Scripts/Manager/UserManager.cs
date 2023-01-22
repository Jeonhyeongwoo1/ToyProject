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

        public bool IsOpenedBattlePass() => _User.IsOpenedBattlePass;
        public int GetUserTier() => _User.Tier;

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

        public void AddUserDiamond(int count)
        {
            _User.Diamond += count;

            UIManager.Instance.UpdateUserDiamondUI(_User.Diamond);
        }
    }
}