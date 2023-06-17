using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;

    public static GameAssets instance {
        get {
            if (_instance == null) _instance = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _instance;
        }
    }

    public Sprite SpriteAR;
    public Sprite SpritePlasmaGun;
    public Sprite SpriteNeonAR;
    public Sprite SpriteShotgun;
    public Sprite SpriteEnergySword;
    public Sprite SpriteNeonSword;
    public Sprite SpritePlasmaScythe;
    public Sprite SpritePlasmaSword;
}
