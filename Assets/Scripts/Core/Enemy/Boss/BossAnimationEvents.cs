using System.Collections;
using System.Collections.Generic;
using Core.Enemy;
using UnityEngine;

public class BossAnimationEvents : MonoBehaviour
{   
    public void activateAbility()
    {
        BossManager.Instance.boss.GetComponent<Boss>().GetCurrentAbility().activateAbility = true;    
    }
}
