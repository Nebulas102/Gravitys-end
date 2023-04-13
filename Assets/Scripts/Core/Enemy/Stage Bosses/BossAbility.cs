using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAbility : MonoBehaviour
{
    [SerializeField]
    private float baseDamage;

    public abstract void UseBossAbility();
}
