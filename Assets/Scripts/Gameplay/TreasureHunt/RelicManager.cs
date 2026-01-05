using UnityEngine;
using System;
public class RelicHandler : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public int type;
    

    void Start()
    {

        System.Random r = new System.Random((int)(tileX*tileY+tileX-tileY));
        float scaleFactor = (float)(0.9 + 0.2 * r.NextDouble())*0.6f;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        transform.rotation = Quaternion.Euler(0f, 0f, (float)r.NextDouble() * 360f);
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);
        TreasureHuntManager.instance.HandleRelicLoot(tilePos, type);
        Destroy(this.gameObject);

    }


}
