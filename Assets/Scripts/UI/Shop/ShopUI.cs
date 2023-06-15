using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public Transform container;
    public Transform itemTemplate;
    private IShopPlayer shopPlayer;

    private void awake() {
        //container = transform.Find("container");
        //itemTemplate = container.Find("itemTemplate");
        itemTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        CreateItemButton(Items.ItemType.AssaultRifle, Items.GetSprite(Items.ItemType.AssaultRifle), 0);
        CreateItemButton(Items.ItemType.PlasmaGun, Items.GetSprite(Items.ItemType.PlasmaGun), 1);

        Hide();
    }

    private void CreateItemButton(Items.ItemType itemType, Sprite itemSprite, int positionIndex) {
        Transform itemTransform = Instantiate(itemTemplate, container);
        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();

        float itemWidth = 200f;
        itemRectTransform.anchoredPosition = new Vector2(itemWidth * positionIndex, 0);

        itemTransform.Find("item").GetComponent<Image>().sprite = itemSprite;
        itemTransform.Find("price").GetComponent<TextMeshProUGUI>().SetText("05:00");

        itemTransform.GetComponent<Button>().onClick.AddListener(() => AttemptPurchaseItem(itemType));
    }


    private void AttemptPurchaseItem(Items.ItemType itemType) {
        if (UI.Inventory.InventoryManager.instance.TrySpendTimeAmount(300)) {
            UI.Inventory.InventoryManager.instance.PurchasedItem(itemType);
        }
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
