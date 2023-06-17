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
    }

    private void Start() {
        List<Items.ItemType> itemTypeList = populateItems();
        int[] uniqueNumbers = generateUniqueNumbers();

        CreateItemButton(itemTypeList[uniqueNumbers[0]], Items.GetSprite(itemTypeList[uniqueNumbers[0]]), Items.GetCost(itemTypeList[uniqueNumbers[0]]), 0);
        CreateItemButton(itemTypeList[uniqueNumbers[1]], Items.GetSprite(itemTypeList[uniqueNumbers[1]]), Items.GetCost(itemTypeList[uniqueNumbers[1]]), 1);
        CreateItemButton(itemTypeList[uniqueNumbers[2]], Items.GetSprite(itemTypeList[uniqueNumbers[2]]), Items.GetCost(itemTypeList[uniqueNumbers[2]]), 2);
        itemTemplate.gameObject.SetActive(false);

        Hide();
    }

    private void CreateItemButton(Items.ItemType itemType, Sprite itemSprite, int itemCost, int positionIndex) {
        Transform itemTransform = Instantiate(itemTemplate, container);
        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();

        float itemWidth = 200f;
        itemRectTransform.anchoredPosition = new Vector2(itemWidth * positionIndex, 0);

        itemTransform.Find("item").GetComponent<Image>().sprite = itemSprite;
        itemTransform.Find("price").GetComponent<TextMeshProUGUI>().SetText($"0{itemCost}:00");


        itemTransform.GetComponent<Button>().onClick.AddListener(() => AttemptPurchaseItem(itemType, itemTransform.gameObject));
    }


    private void AttemptPurchaseItem(Items.ItemType itemType, GameObject itemButtonObject) {
        if (UI.Inventory.InventoryManager.instance.TrySpendTimeAmount(Items.GetCost(itemType))) {
            UI.Inventory.InventoryManager.instance.PurchasedItem(itemType);
            Destroy(itemButtonObject);
        }
    }

    private int[] generateUniqueNumbers() {
        HashSet<int> uniqueNumbers = new HashSet<int>();

        while (uniqueNumbers.Count < 3) {
            int randomNumber = UnityEngine.Random.Range(0,7);

            if (!uniqueNumbers.Contains(randomNumber))
            {
                uniqueNumbers.Add(randomNumber);
            }
        }
        int[] randomNumbersArray = new List<int>(uniqueNumbers).ToArray();

        return randomNumbersArray;
    }

    private List<Items.ItemType> populateItems() {
        List<Items.ItemType> itemTypeList = new List<Items.ItemType>();
        itemTypeList.Add(Items.ItemType.AssaultRifle);
        itemTypeList.Add(Items.ItemType.PlasmaGun);
        itemTypeList.Add(Items.ItemType.NeonAR);
        itemTypeList.Add(Items.ItemType.Shotgun);
        itemTypeList.Add(Items.ItemType.EnergySword);
        itemTypeList.Add(Items.ItemType.NeonSword);
        itemTypeList.Add(Items.ItemType.PlasmaScythe);
        itemTypeList.Add(Items.ItemType.PlasmaSword);
        return itemTypeList;
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
