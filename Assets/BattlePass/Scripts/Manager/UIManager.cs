using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace BattlePass
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private SystemData _SystemData;
        [SerializeField] private BattlePassPresenter _BattlePassPresenter;

        public void SetUserGoods(int star, int pearl, int gold, int diamond)
        {
            _BattlePassPresenter.SetUserGoodsUI(star, pearl, gold, diamond);
        }

        public void UpdateTier(int tier, bool isOpenedBattlePass)
        {
            _BattlePassPresenter.UpdateTier(tier, isOpenedBattlePass);
        }

        public void SetUserEXP(int curExp, int maxExp)
        {
            _BattlePassPresenter.SetUserEXP(curExp, maxExp);
        }

        public void UpdateUserEXP(int exp)
        {
            UserManager.Instance.UpdateUserEXP(exp);
        }

        public void UpdateUserDiamondUI(int count)
        {
            _BattlePassPresenter.UpdateUserDiamondUI(count);
        }

        public List<Sprite> GetUserItemSpriteList()
        {
            List<string> userItemList = UserManager.Instance.GetUserItemData();
            if (userItemList == null)
            {
                return null;
            }

            List<Sprite> userItemSpriteList = new List<Sprite>();
            for (int i = 0; i < userItemList.Count; i++)
            {
                for (int j = 0; j < _SystemData.itemDataList.Count; j++)
                {
                    if (_SystemData.itemDataList[j].id == userItemList[i])
                    {
                        userItemSpriteList.Add(_SystemData.itemDataList[j].itemSprite);
                    }
                }
            }

            return userItemSpriteList;
        }

        public List<string> GetUserItemList() => UserManager.Instance.GetUserItemData();
        public void GainItem(string id) => UserManager.Instance.GainItem(id);
        public void UpdateUserStar(int count) => UserManager.Instance.UpdateUserStar(count);
        public void UpdateUserDiamond(int count) => UserManager.Instance.UpdateUserDiamond(count);
        public void UpdateUserPearl(int count) => UserManager.Instance.UpdateUserPearl(count);
        public void UpdateUserGold(int count) => UserManager.Instance.UpdateUserGold(count);

        private void Start()
        {
            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.Alpha1))
                        .Subscribe((v) => _BattlePassPresenter.CreateBattlePassElement(_SystemData.battlePassDataList));

            // Observable.EveryUpdate()
            //             .Where((v) => Input.GetKeyDown(KeyCode.Alpha3))
            //             .Subscribe((v) => _BattlePassPresenter.star.Value += 1);

            // Observable.EveryUpdate()
            //             .Where((v) => Input.GetKeyDown(KeyCode.Alpha4))
            //             .Subscribe((v) => _BattlePassPresenter.pearl.Value += 1);

            // Observable.EveryUpdate()
            //             .Where((v) => Input.GetKeyDown(KeyCode.Alpha5))
            //             .Subscribe((v) => _BattlePassPresenter.gold.Value += 1);

            // Observable.EveryUpdate()
            //             .Where((v) => Input.GetKeyDown(KeyCode.Alpha6))
            //             .Subscribe((v) => _BattlePassPresenter.diamond.Value += 1);

        }
    }
}
