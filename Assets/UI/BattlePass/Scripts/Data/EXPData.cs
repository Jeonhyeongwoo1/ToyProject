using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattlePass
{
    [CreateAssetMenu(menuName = "BattlePass/Create EXPData", fileName = "EXPData")]
    public class EXPData : ScriptableObject
    {
        [Serializable]
        public class EXP
        {
            public int tier;
            public int maxExp;
        }

        public int maxTier;
        public List<EXP> expList;
    }
}
