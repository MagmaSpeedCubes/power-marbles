// CustomTile.cs (derive from Tile)
using UnityEngine;
using UnityEngine.Tilemaps;

using UnityEngine;
[CreateAssetMenu]
public class TreasureHuntTile : Tile
{
    public Ownable tileData;
    public LootTable lootTable;
}
