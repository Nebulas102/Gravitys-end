using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSeeThrough : MonoBehaviour
{
    public List<GameObject> tiles;

    public void EnableTiles()
    {
        tiles.ForEach(t => t.SetActive(true));
    }

    public void DisableTiles()
    {
        tiles.ForEach(t => t.SetActive(false));
    }
}
