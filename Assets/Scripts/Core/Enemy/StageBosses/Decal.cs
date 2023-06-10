using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decal : MonoBehaviour
{
    private float radius;

    public void SetRadius(float radius)
    {
        this.radius = radius;
        transform.localScale = new Vector3(radius, transform.localScale.y, radius);
    }
}
