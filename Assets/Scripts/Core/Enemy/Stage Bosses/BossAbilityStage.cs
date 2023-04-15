using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAbilityStage
{
   [SerializeField]
   private BossAbility bossAbility;
   [SerializeField]
   private int amountOfTimes;

   private int amountOfTimesUsed;

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
        return amountOfTimesUsed;
   }

   public void SetAmountOfTimesUsed(int used)
   {
        amountOfTimesUsed = used;
   }

   public void IncrementAmountOfTimesUsed()
   {
        amountOfTimesUsed++;
   }
}
