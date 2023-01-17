using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace BattlePass
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private SystemData _SystemData;
        [SerializeField] private BattlePassPresenter _BattlePassPresenter;

        private void Start()
        {
            Observable.EveryUpdate()
                        .Where((v) => Input.GetKeyDown(KeyCode.Alpha1))
                        .Subscribe((v) => _BattlePassPresenter.CreateBattlePassElement(_SystemData.battlePassDataList));
        }

    }
}
