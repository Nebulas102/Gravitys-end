using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject boss;

    #region Singleton

    public static BossManager instance;

    private void Awake()
    {
        if(instance != null)
        return;

        instance = this;
    }

    #endregion
}
