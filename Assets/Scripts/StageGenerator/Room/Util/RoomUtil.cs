using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class RoomUtil
{
    public static Dictionary<string, float> CalculateSizeBasedOnChildren(GameObject parent)
    {
        Dictionary<string, float> sizes = new Dictionary<string, float>();

        float x = 0;
        float y = 0;
        float z = 0;

        float floorY = 0;
        
        Transform[] childrenFloor = parent.GetComponentsInChildren<Transform>().Where(g => g.CompareTag("Floor")).ToArray();
        Transform[] childrenWalls = parent.GetComponentsInChildren<Transform>().Where(g => g.CompareTag("Wall")).ToArray();

        x = childrenFloor.Max(g => g.transform.localScale.x);
        z = childrenFloor.Max(g => g.transform.localScale.z);
        floorY = childrenFloor.Max(g => g.transform.localScale.y);

        foreach (Transform t in parent.transform)
        {
            float tempY = 0;

            if (t.CompareTag("Wall")) tempY = t.localScale.y;

            if (y < tempY) y = tempY;
        }

        sizes.Add("x", x);
        sizes.Add("y", y += floorY);
        sizes.Add("z", z);

        return sizes;
    }
}
