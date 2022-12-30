using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Manager : MonoBehaviour
{
    public static BotManager botManager => BotManager.Instance;
}

public class BotManager : Singleton<BotManager>
{
    public ConfigBotObject ConfigBotObject => configBotObject;
    [SerializeField] private ConfigBotObject configBotObject;

    private void Start()
    {
        GetBotBehaviourData<WeightDecisionType, DecisionType>();
    }

    public CommonBotData<TWeightDecisionType, TDecisionType> GetBotBehaviourData<TWeightDecisionType, TDecisionType>()
                                                                                                    where TDecisionType : Enum
                                                                                                    where TWeightDecisionType : Enum
    {
        return ConfigBotObject.CommonBotData as CommonBotData<TWeightDecisionType, TDecisionType>;
    }
}
