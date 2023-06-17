using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollider : MonoBehaviour
{
    private ShopUI shopUI;

    private void Awake()
    {
        GameObject shopObject = GameObject.Find("Shop");

        if (shopObject == null)
        {
            Transform canvas = GameObject.Find("Canvas").transform;
            shopObject = canvas.Find("Shop").gameObject;
        }

        shopUI = shopObject.GetComponent<ShopUI>();
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.CompareTag("Player")) {
            shopUI.Show();
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.CompareTag("Player")) {
            shopUI.Hide();
        }
    }
}
