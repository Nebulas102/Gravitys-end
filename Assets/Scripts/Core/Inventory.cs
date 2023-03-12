using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Singleton for inventory
    public static Inventory instance;

    void Awake()
    {
        if (instance != null) {
            return;
        }
        instance = this;
    }

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 6;

    // This code below is for the future for adding items to the inventory  video (https://www.youtube.com/watch?v=HQNl3Ff2Lpo)

    // public List<Item> items = new List<Item>();

    // public bool Add(Item item)
    // {
    //     if (!item.isDefaultItem)
    //     {
    //         if (items.count >= space)
    //         {
    //             // Not enough room
    //             return false;
    //         }
    //         items.Add(item);

    //         if (onItemChangedCallback != null)
    //         {
    //             onItemChangedCallback.Invoke();
    //         }

    //     }
    //     return true;
    // }

    // public void Add(Item item)
    // {
    //     items.Remove(item);

    //     if (onItemChangedCallback != null)
    //     {
    //         onItemChangedCallback.Invoke();
    //     }
    // }

}
