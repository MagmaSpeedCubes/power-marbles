using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System;
public class TreasureTilemapHandler : DamageHandler
{
    public Tilemap treasureTilemap;
    public TreasureHuntTile latestHit;


    override public void Damage(float amount, string type, BallHandler source = null)
    {
        Vector3 marblePos = source.transform.position;
        Vector3 velocity = source.GetVelocity();
        Vector3 targetTile = marblePos + velocity.normalized * Constants.TREASURE_HUNT_TILE_SIZE;
        Vector3Int cellPosition = treasureTilemap.WorldToCell(targetTile);
        TreasureHuntTile tile = (TreasureHuntTile) treasureTilemap.GetTile(cellPosition);

        latestHit = tile;


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
