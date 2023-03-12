using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;
        public List<Gear> items = new();

        [SerializeField]
        private int _itemCap = 6;

        void Awake()
        {
            // For better code quality, it's better to use singleton pattern for the inventory
            if(Instance == null)
            {
                Instance = new Inventory();
            }

            Instance = this;
        }

        // Add new item to the inventory
        public bool AddToInventory(Gear item)
        {
            if(items.Count >= _itemCap)
                return false;

            items.Add(item);
            return true;
        }

        // Remove item from the inventory
        public bool RemoveFromInventory(Gear item)
        {
            return items.Remove(item);
        }
    }
}
