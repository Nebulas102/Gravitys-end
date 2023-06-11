using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
using Core.Enemy;
using ScriptableObjects;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField]
    private int minDamage = 5;
    [SerializeField]
    private int maxDamage = 10;

    private bool isEquipped = false;

    public int GetMinDamage()
    {
        return minDamage;
    }

    public int GetMaxDamage()
    {
        return maxDamage;
    }
}
