using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public Item item;

    [HideInInspector]
    public bool isInInventory = false;
}
