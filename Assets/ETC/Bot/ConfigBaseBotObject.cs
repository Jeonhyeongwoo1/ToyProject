using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Helper
{
    public static int EnumToInt(System.Enum value) => (int)(object)value;
}

public enum BotLevel
{
    Low,
    Normal,
    High
}

public enum WeightDecisionType
{
    ToWeightETC1,
    ToWeightETC2
}

public enum DecisionType
{
    ToETC1,
    ToETC2
}

[Serializable]
public class BotRawData
{
    public int level;
}

[Serializable]
public class BotWeightDecisionTypeData<TWeightDecisionType> where TWeightDecisionType : Enum
{
    public bool use;
    public TWeightDecisionType decisionType;
    public bool isOppsite;
    public float delay;
}

[Serializable]
public class BotDecisionTypeData<TWeightDecisionType, TDecisionType> where TWeightDecisionType : Enum
                                                                    where TDecisionType : Enum
{
    public float selectDecisionProcessDuration;
    public TDecisionType decisionType;
    public List<BotWeightDecisionTypeData<TWeightDecisionType>> botWeightDecisionTypeList;
}

[Serializable]
public class BotBehaviourData<TWeightDecisionType, TDecisionType> where TWeightDecisionType : Enum
                                                                        where TDecisionType : Enum
{
    public BotBehaviourData(BotBehaviourData<TWeightDecisionType, TDecisionType> botBehaviourData)
    {
        botRawData = botBehaviourData.botRawData;
        botDecisionTypeDataList = botBehaviourData.botDecisionTypeDataList;
    }

    public BotRawData botRawData;
    public List<BotDecisionTypeData<TWeightDecisionType, TDecisionType>> botDecisionTypeDataList;

    public Dictionary<int, BotDecisionTypeData<TWeightDecisionType, TDecisionType>> botDecisionTypeDataDict
        = new Dictionary<int, BotDecisionTypeData<TWeightDecisionType, TDecisionType>>();

    public void Init()
    {
        botDecisionTypeDataDict.Clear();

        foreach (BotDecisionTypeData<TWeightDecisionType, TDecisionType> botDecisionTypeData in botDecisionTypeDataList)
        {
            botDecisionTypeDataDict.Add(Helper.EnumToInt(botDecisionTypeData.decisionType), botDecisionTypeData);
        }
    }

    public BotDecisionTypeData<TWeightDecisionType, TDecisionType> GetBotDecisionTypeData(int key)
    {
        if (botDecisionTypeDataDict.TryGetValue(key, out var value))
        {
            return value;
        }

        return null;
    }
}

[Serializable]
public class CommonBotData<TWeightDecisionType, TDecisionType> where TDecisionType : Enum
                                                                where TWeightDecisionType : Enum
{
    public BotBehaviourData<TWeightDecisionType, TDecisionType> highBotData, normalBotData, lowBotData;
}

[Serializable]
public class BotData : CommonBotData<WeightDecisionType, DecisionType>
{

}

public class BaseConfigBotObject<TBotData, TWeightDecisionType, TDecisionType> : ScriptableObject
                                                                    where TBotData : CommonBotData<TWeightDecisionType, TDecisionType>
                                                                    where TWeightDecisionType : Enum
                                                                    where TDecisionType : Enum
{
    public TBotData CommonBotData
    {
        get => commonBotData;
        set => commonBotData = value;
    }

    public TBotData commonBotData;

    public void Init(TBotData botData)
    {
        commonBotData = botData;
    }
}

public class BotLevelData<TWeightDecisionType, TDecisionType> where TWeightDecisionType : Enum
                                                                        where TDecisionType : Enum
{
    public static BotBehaviourData<TWeightDecisionType, TDecisionType> High
    {
        get
        {
            if (high == null)
            {
                high = new BotBehaviourData<TWeightDecisionType, TDecisionType>(configBotObject.highBotData);
            }

            return high;
        }
    }

    public static BotBehaviourData<TWeightDecisionType, TDecisionType> Normal
    {
        get
        {
            if (normal == null)
            {
                normal = new BotBehaviourData<TWeightDecisionType, TDecisionType>(configBotObject.normalBotData);
            }

            return normal;
        }
    }

    public static BotBehaviourData<TWeightDecisionType, TDecisionType> Low
    {
        get
        {
            if (low == null)
            {
                low = new BotBehaviourData<TWeightDecisionType, TDecisionType>(configBotObject.lowBotData);
            }

            return low;
        }
    }


    private static CommonBotData<TWeightDecisionType, TDecisionType> configBotObject => Manager.botManager?.GetBotBehaviourData<TWeightDecisionType, TDecisionType>();
    private static BotBehaviourData<TWeightDecisionType, TDecisionType> high, normal, low;

    public static BotBehaviourData<TWeightDecisionType, TDecisionType> GetBotPlayData(BotLevel botLevel)
    {
        switch (botLevel)
        {
            case BotLevel.Low: return Low;
            case BotLevel.Normal: return Normal;
            case BotLevel.High: return High;
        }

        return Normal;
    }
}