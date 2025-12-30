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
    public GameObject[,] relics;

    override public void Damage(float amount, string type, BallHandler source = null)
    {
        Vector3 marblePos = source.transform.position;
        Vector3 velocity = source.GetVelocity();
        Vector3 targetTile = marblePos + velocity.normalized * Constants.TREASURE_HUNT_TILE_SIZE * 2;
        Vector3Int cellPosition = treasureTilemap.WorldToCell(targetTile);
        Debug.Log("Tile Position: " + cellPosition);


       


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
