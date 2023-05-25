using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy.StageBosses
{
    [Serializable]
    public class BossAbilityStage
    {
        [SerializeField]
        private List<BossAbilitySequence> bossAbilitySequences;

        [SerializeField]
        private int healthStageActivation;

        public List<BossAbilitySequence> GetBossAbilitySequences()
        {
            return bossAbilitySequences;
        }

        public int GetHealhStageActivation()
        {
            return healthStageActivation;
        }
    }
}
