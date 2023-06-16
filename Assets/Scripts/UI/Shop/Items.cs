using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items
{
    public enum ItemType {
        AssaultRifle,
        PlasmaGun,
        NeonAR,
        Shotgun,
        EnergySword,
        NeonSword,
        PlasmaScythe,
        PlasmaSword
    }

    public static Sprite GetSprite(ItemType itemType) {
        switch (itemType) {
            default:
            case ItemType.AssaultRifle: return GameAssets.instance.SpriteAR;
            case ItemType.PlasmaGun:    return GameAssets.instance.SpritePlasmaGun;
            case ItemType.NeonAR:       return GameAssets.instance.SpriteNeonAR;
            case ItemType.Shotgun:      return GameAssets.instance.SpriteShotgun;
            case ItemType.EnergySword:  return GameAssets.instance.SpriteEnergySword;
            case ItemType.NeonSword:    return GameAssets.instance.SpriteNeonSword;
            case ItemType.PlasmaScythe: return GameAssets.instance.SpritePlasmaScythe;
            case ItemType.PlasmaSword:  return GameAssets.instance.SpritePlasmaSword;
        }
    }

    public static int GetCost(ItemType itemType) {
        switch (itemType) {
            default:
            case ItemType.AssaultRifle: return 3;
            case ItemType.PlasmaGun:    return 5;
            case ItemType.NeonAR:       return 3;
            case ItemType.Shotgun:      return 5;
            case ItemType.EnergySword:  return 2;
            case ItemType.NeonSword:    return 5;
            case ItemType.PlasmaScythe: return 6;
            case ItemType.PlasmaSword:  return 5;
        }
    }
}
