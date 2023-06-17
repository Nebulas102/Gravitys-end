using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopPlayer
{
    void PurchasedItem(Items.ItemType itemType);
    bool TrySpendTimeAmount(float timeAmount);
}
