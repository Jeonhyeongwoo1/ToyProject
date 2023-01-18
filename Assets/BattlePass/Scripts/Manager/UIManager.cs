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

        public void SetUserEXP(int tier, int curExp, int maxExp)
        {
            _BattlePassPresenter.SetUserEXP(tier, curExp, maxExp);
        }

        public void UpdateUserEXP(int exp)
        {
            UserManager.Instance.UpdateUserEXP(exp);
        }

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
