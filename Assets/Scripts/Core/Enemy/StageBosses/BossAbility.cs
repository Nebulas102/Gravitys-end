using System.Collections;
using UnityEngine;

namespace Core.Enemy.StageBosses
{
    public abstract class BossAbility : MonoBehaviour
    {
        [SerializeField]
        private float baseDamage;

        public abstract IEnumerator UseBossAbility();
    }
}
