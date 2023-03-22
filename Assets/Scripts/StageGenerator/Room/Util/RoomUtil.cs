using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomUtil
{
    public static Dictionary<string, float> CalculateSizeBasedOnChildren(GameObject gameObject)
    {
        Dictionary<string, float> sizes = new Dictionary<string, float>();

        float x = 0;
        float y = 0;
        float z = 0;

        float floorY = 0;

        foreach (Transform t in gameObject.transform)
        {
            float tempY = 0;

            if (t.CompareTag("Floor")) floorY = t.localScale.y;

            if (x < t.localScale.x) x = t.localScale.x;

            if (t.CompareTag("Wall")) tempY = t.localScale.y;

            if (y < tempY) y = tempY;

            if (z < t.localScale.z) z = t.localScale.z;
        }

        sizes.Add("x", x);
        sizes.Add("y", y += floorY);
        sizes.Add("z", z);

        return sizes;
    }
}
