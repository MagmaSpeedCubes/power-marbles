using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System;
using TMPro;

public class TreasureTilemapHandler : DamageHandler
{
    public Tilemap treasureTilemap;
    public TreasureHuntTile latestHit;

    public TextMeshPro[,] healthGrid;
    public GameObject[,] relicGrid;

    public Ownable worldMap;

    public Color damaged, low;
    public Vector3Int lastHitCell;



    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Tilemap Hit");
        foreach (ContactPoint2D hit in collision.contacts)
        {
            //Debug.Log("Normal Y: " +  hit.point.y);
            
            Vector2 correctedHitPoint = new Vector2(hit.point.x, hit.point.y);
            //Debug.Log("Adjusted Y: " +  correctedHitPoint.y);
            Vector3 hitPosition = correctedHitPoint + hit.normal * 0.1f;
            Vector3Int cellCoords = treasureTilemap.WorldToCell(hitPosition);
            //Vector3Int correctedCellCoordinates = new Vector3Int(cellCoords.x-1, Constants.TREASURE_HUNT_MAP_SIZE - cellCoords.y, cellCoords.z);
            lastHitCell = cellCoords;
            //Debug.Log("Found center: " + treasureTilemap.CellToWorld(cellCoords));
            //Debug.Log("Hit Tile Coordinates: " + cellCoordinates);
        }
    }

    override public void Damage(float amount, string type, BallHandler source = null)
    {
        Vector3Int cellPosition = lastHitCell;
        //cellPosition.y--;
        //Debug.Log("Cell position: " + cellPosition);

        TextMeshPro textAtHit;
        try
        {
            textAtHit = healthGrid[cellPosition.x, cellPosition.y];
        }catch(IndexOutOfRangeException e)
        {
            Debug.LogWarning("Attempted to collide with an out of range tile");
            return;
        }
        
        int health = int.Parse(worldMap.FindTag(""+cellPosition.x+"-"+cellPosition.y+"Health"));
        health -= (int)amount;


        textAtHit.text = ""+health;
        //Debug.Log("Health: " + health);
        worldMap.ModifyTagValue(""+cellPosition.x+"-"+cellPosition.y+"Health", ""+health);
        if(health <= 0)
        {
            //Debug.Log("Dead");
            textAtHit.text = "";
            
            TreasureHuntManager.instance.HandleTileBreak(treasureTilemap, cellPosition);


            Destroy(source.gameObject);

        }
        else if(health < 3)
        {
            //Debug.Log("Low health");
            textAtHit.color = low;
        }else
        {
            //Debug.Log("Damaged");
            textAtHit.color = damaged;
        }

        TreasureHuntManager.instance.treasureHuntWorldMap = worldMap;

        TreasureHuntManager.instance.AutoSave();
    }

    void Awake()
    {
        
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
