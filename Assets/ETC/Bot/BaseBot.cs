using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class BotDecisionCoroutineContainer
{
    public int decisionType;
    public Coroutine coroutine;
}

public class BaseBot<TWeightDecisionType, TDecisionType> : MonoBehaviour
                                                        where TWeightDecisionType : Enum
                                                        where TDecisionType : Enum
{
    public BotBehaviourData<TWeightDecisionType, TDecisionType> BotBehaviourData
    {
        get => _BotBehaviourData;
        private set => _BotBehaviourData = value;
    }

    [SerializeField] protected BotLevel _BotLevel;
    [SerializeField] private BotBehaviourData<TWeightDecisionType, TDecisionType> _BotBehaviourData;
    [SerializeField] private Rigidbody rd;

    private List<BotDecisionCoroutineContainer> botDecisionCoroutineContainerList
                                                = new List<BotDecisionCoroutineContainer>();
    private int selectDecisionType;
    private int newDecisionType;
    private Vector3 moveDir = Vector3.zero;

    private Coroutine selectDecisionProcessCor = null;

    private void Awake()
    {
        TryGetComponent<Rigidbody>(out var rigidbody);
        rd = rigidbody;
    }

    private IEnumerator Start()
    {

        yield return new WaitForSeconds(2f);

        BotBehaviourData = BotLevelData<TWeightDecisionType, TDecisionType>.GetBotPlayData(_BotLevel);
        BotBehaviourData.Init();
        selectDecisionType = 0;
        OnBeginProcess();
    }

    /*
        1. Bot data에 저장된 데이터를 불러온다.
    // */

    // private void OnEnable()
    // {
    //     BotBehaviourData = BotLevelData<TWeightDecisionType, TDecisionType>.GetBotPlayData(_BotLevel);
    //     BotBehaviourData.Init();
    //     selectDecisionType = 0;
    // }

    private void FixedUpdate()
    {
        float z = Input.GetAxisRaw("Vertical");
        float x = Input.GetAxisRaw("Horizontal");

        moveDir.x = x;
        moveDir.z = z;

        Vector3 prev = transform.position;
        Vector3 next = prev + moveDir;
        Vector3 value = Vector3.Lerp(prev, next, Time.fixedDeltaTime * 20);
        rd.MovePosition(value);
    }

    private void OnBeginProcess()
    {
        selectDecisionProcessCor = StartCoroutine(SelectDecisionProcessCor());

    }

    private void UpdateDecision(int newDecisionType)
    {
        if (selectDecisionType == newDecisionType)
        {
            return;
        }

        selectDecisionType = newDecisionType;
    }

    private IEnumerator SelectDecisionProcessCor()
    {
        var botDecisionTypeData = BotBehaviourData.GetBotDecisionTypeData(selectDecisionType);

        if (botDecisionTypeData != null)
        {
            foreach (var typeData in botDecisionTypeData.botWeightDecisionTypeList)
            {
                if (!typeData.use)
                {
                    continue;
                }

                BotDecisionCoroutineContainer container = new BotDecisionCoroutineContainer();
                container.decisionType = Helper.EnumToInt(typeData.decisionType);
                container.coroutine = StartCoroutine(AddDecisionProcessCor(typeData));
            }

            while (selectDecisionType == newDecisionType)
            {
                yield return null;
            }
        }
    }

    private IEnumerator AddDecisionProcessCor(BotWeightDecisionTypeData<TWeightDecisionType> data)
    {
        //Bot에서 설정한 행위를 처리해주는 코루틴을 설계한다.
        yield return null;

    }

}
