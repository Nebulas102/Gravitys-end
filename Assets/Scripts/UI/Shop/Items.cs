using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items
{
    public enum ItemType {
        AssaultRifle,
        PlasmaGun
    }

    public static Sprite GetSprite(ItemType itemType) {
        switch (itemType) {
            default:
            case ItemType.AssaultRifle: return GameAssets.instance.SpriteAR;
            case ItemType.PlasmaGun:    return GameAssets.instance.SpritePlasmaGun;
        }
    }
}
