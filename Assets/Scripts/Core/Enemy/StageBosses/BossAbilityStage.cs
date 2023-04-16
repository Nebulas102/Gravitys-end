using System;
using UnityEngine;

namespace Core.Enemy.StageBosses
{
    [Serializable]
    public class BossAbilityStage
    {
        [SerializeField]
        private BossAbility bossAbility;

        [SerializeField]
        private int amountOfTimes;

        private int _amountOfTimesUsed;

        public BossAbility GetBossAbility()
        {
            return bossAbility;
        }

        public int GetAmountOfTimes()
        {
            return amountOfTimes;
        }

        public int GetAmountOfTimesUsed()
        {
            return _amountOfTimesUsed;
        }

        public void SetAmountOfTimesUsed(int used)
        {
            _amountOfTimesUsed = used;
        }

        public void IncrementAmountOfTimesUsed()
        {
            _amountOfTimesUsed++;
        }
    }
}
