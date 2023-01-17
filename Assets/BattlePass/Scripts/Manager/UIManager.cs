using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace BattlePass
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UserData _UserData;
        [SerializeField] private SystemData _SystemData;
        [SerializeField] private BattlePassPresenter _BattlePassPresenter;

        private void Start()
        {
            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.Alpha1))
                        .Subscribe((v) => _BattlePassPresenter.CreateBattlePassElement(_SystemData.battlePassDataList));

            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.Alpha2))
                        .Subscribe((v) => _BattlePassPresenter.InitUserData(_UserData));

            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.Alpha3))
                        .Subscribe((v) => _BattlePassPresenter.star.Value += 1);

            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.Alpha4))
                        .Subscribe((v) => _BattlePassPresenter.pearl.Value += 1);

            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.Alpha5))
                        .Subscribe((v) => _BattlePassPresenter.gold.Value += 1);

            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.Alpha6))
                        .Subscribe((v) => _BattlePassPresenter.diamond.Value += 1);

        }
    }
}
