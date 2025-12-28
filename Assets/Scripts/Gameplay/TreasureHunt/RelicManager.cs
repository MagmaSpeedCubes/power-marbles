using UnityEngine;
using System;
public class RelicManager : MonoBehaviour
{
    public int tileX;
    public int tileY;
    

    void Start()
    {

        System.Random r = new System.Random((int)(tileX*tileY+tileX-tileY));
        float scaleFactor = (float)(0.9 + 0.2 * r.NextDouble())*0.3f;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        transform.rotation = Quaternion.Euler(0f, 0f, (float)r.NextDouble() * 360f);
    }
}
