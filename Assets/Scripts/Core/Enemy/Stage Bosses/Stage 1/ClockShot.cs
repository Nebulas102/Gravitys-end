using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockShot : BossAbility
{
    private GameObject boss;

    private void Start()
    {
        boss = GameObject.FindWithTag("Boss");

    }

    public override void UseBossAbility()
    {
        StartCoroutine(Shot());
    }

    private IEnumerator Shot()
    {
        Debug.Log("Clock Shot");

        yield return new WaitForSeconds(4f);
    }
}
