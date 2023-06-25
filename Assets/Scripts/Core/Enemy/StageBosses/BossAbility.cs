using System.Collections;
using UnityEngine;

namespace Core.Enemy.StageBosses
{
    public abstract class BossAbility : MonoBehaviour
    {

        [HideInInspector]
        public bool activateAbility;

        public abstract IEnumerator UseBossAbility();
    }
}
