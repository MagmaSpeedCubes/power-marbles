using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System;
public class TreasureHuntManager : MonoBehaviour
{
    
    public static TreasureHuntManager instance;
    public int energy;
    public DateTime lastRecharge;
    public TreasureHuntTile[] tiles;
    public Sprite[] relicSprites;
    public GameObject tileMap;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of TreasureHuntManager detected. Destroying duplicate.");
        }
    }
    void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();

        int season = (int)(CalculateTotalMonths(Constants.LAUNCH_DATE, DateTime.UtcNow) / 2);

        Ownable treasureHuntData = SecureProfileStats.instance.FindFirstOwnableOfName("treasureHuntDataSeason" + season);
        Ownable treasureHuntWorldMap = SecureProfileStats.instance.FindFirstOwnableOfName("treasureHuntWorldMapSeason" + season);
        Ownable treasureHuntRelicMap = SecureProfileStats.instance.FindFirstOwnableOfName("treasureHuntRelicMapSeason" + season);

        if(treasureHuntData == null || treasureHuntWorldMap == null || treasureHuntRelicMap == null)
        {
            treasureHuntData = new Ownable("treasureHuntDataSeason" + season, SpriteManager.instance.placeholder);
            
            energy = Constants.TREASURE_HUNT_MAX_ENERGY;
            treasureHuntData.AddTag("energy", ""+energy);
            treasureHuntWorldMap = GenerateTreasureHuntWorldMap();

        }
        tileMap = LoadWorldFromOwnable(treasureHuntWorldMap, treasureHuntRelicMap);




    }

    public Ownable GenerateTreasureHuntWorldMap()
    {
        /*
        Tile 0 = grass
        Tile 1 = dirt
        Tile 2 = stone
        Tile 3 = stoneGold
        Tile 4 = deepslate
        Tile 5 = deepslateGold
        Tile 6 = deepslateDiamond
        Tile 7 = treasure
        
        
        */
        int season = (int)(CalculateTotalMonths(Constants.LAUNCH_DATE, DateTime.UtcNow) / 2);
        Ownable output = new Ownable("treasureHuntWorldMapSeason" + season, SpriteManager.instance.placeholder);
        int goldPerMap = 30;
        int diamondPerMap = 15;
        int treasurePerMap = 10;
        
        for(int y=0; y<Constants.TREASURE_HUNT_MAP_SIZE; y++)
        {
            for(int x=0; x<Constants.TREASURE_HUNT_MAP_SIZE; x++)
            {
                if (y == 0)
                {
                    output.AddTag(""+x+","+y, "grass");
                    output.AddTag(""+x+","+y+"Health", "3");
                }
                else if (y < 3)
                {
                    output.AddTag(""+x+","+y, "dirt");
                    output.AddTag(""+x+","+y+"Health", "5");
                }
                else if (y < 10)
                {
                    output.AddTag(""+x+","+y, "stone");
                    output.AddTag(""+x+","+y+"Health", "20");
                }
                else
                {
                    output.AddTag(""+x+","+y, "deepslate");
                    output.AddTag(""+x+","+y+"Health", "100");
                }

            }
        }
        //spawn base layers of blocks

        do
        {
            int randX = UnityEngine.Random.Range(0, Constants.TREASURE_HUNT_MAP_SIZE);
            int randY = UnityEngine.Random.Range(0, Constants.TREASURE_HUNT_MAP_SIZE);
            string tileAtRandom = output.FindTag(""+randX+","+randY);
            if (tileAtRandom.Equals("stone"))
            {
                output.ModifyTagValue(""+randX+","+randY, "stoneGold");
                goldPerMap--;
            }else if (tileAtRandom.Equals("deepslate"))
            {
                output.ModifyTagValue(""+randX+","+randY, "deepslateGold");
                goldPerMap--;
            }

        }while(goldPerMap > 0);
        //spawn gold in stone layers

        do
        {
            int randX = UnityEngine.Random.Range(0, Constants.TREASURE_HUNT_MAP_SIZE);
            int randY = UnityEngine.Random.Range(0, Constants.TREASURE_HUNT_MAP_SIZE);
            string tileAtRandom = output.FindTag(""+randX+","+randY);
            if (tileAtRandom.Equals("deepslate"))
            {
                output.ModifyTagValue(""+randX+","+randY, "deepslateDiamond");
                diamondPerMap--;
            }
        }while(diamondPerMap > 0);
        //spawn diamond in deepslate layers

        return output;
        
    }


    public Ownable GenerateTreasureHuntRelicMap()
    {
        int season = (int)(CalculateTotalMonths(Constants.LAUNCH_DATE, DateTime.UtcNow) / 2);
        Ownable output = new Ownable("treasureHuntRelicMapSeason" + season, SpriteManager.instance.placeholder);
        int relicsPerWorld = 50;
        do
        {
            int randX = UnityEngine.Random.Range(0, Constants.TREASURE_HUNT_MAP_SIZE);
            int randY = UnityEngine.Random.Range(0, Constants.TREASURE_HUNT_MAP_SIZE);
            string tileAtRandom = output.FindTag(""+randX+","+randY);
            if (tileAtRandom == null)
            {
                output.AddTag(""+randX+","+randY, ""+UnityEngine.Random.Range(0, relicSprites.Length));
                relicsPerWorld--;
            }

        }while(relicsPerWorld > 0);
        return output;
    }

    public GameObject LoadWorldFromOwnable(Ownable worldMap, Ownable relicMap)
    {
        GameObject gridGO = new GameObject("HexGrid");
        Grid grid = gridGO.AddComponent<Grid>();
        grid.cellLayout = GridLayout.CellLayout.Hexagon;
        grid.cellSwizzle = GridLayout.CellSwizzle.XYZ; // or another swizzle if needed
        grid.cellSize = new Vector3(Mathf.Sqrt(3)/2f, 1f, 1f); // or adjust as needed

        GameObject tilemapGO = new GameObject("HexTilemap");
        tilemapGO.transform.SetParent(gridGO.transform);
        Tilemap tilemap = tilemapGO.AddComponent<Tilemap>();
        TilemapRenderer renderer = tilemapGO.AddComponent<TilemapRenderer>();
        TilemapCollider2D collider = tilemapGO.AddComponent<TilemapCollider2D>();

        for (int x = 0; x < Constants.TREASURE_HUNT_MAP_SIZE; x++)
        {
            for (int y = 0; y < Constants.TREASURE_HUNT_MAP_SIZE; y++)
            {
                Vector3Int pos = new Vector3Int(x, -y, 0);
                string tileAt = worldMap.FindTag(""+x+","+y);
                Tile tileToPlace;
                foreach(TreasureHuntTile tile in tiles)
                {
                    if (tile.name.Equals(tileAt))
                    {
                        tileToPlace = tile;
                        tilemap.SetTile(pos, tileToPlace);
                        break;
                    }
                }
                //Tile tileToPlace = 
                
            }
        }
        collider.ProcessTilemapChanges();
        return gridGO;
    }
    
    public static int CalculateTotalMonths(DateTime startDate, DateTime endDate)
    {
        // Ensure the start date is before the end date.
        if (startDate > endDate)
        {
            DateTime temp = startDate;
            startDate = endDate;
            endDate = temp;
        }

        int months = (endDate.Year - startDate.Year) * 12 + (endDate.Month - startDate.Month);

        // Optional: adjust if the end date's day is earlier in the month than the start date's day
        if (endDate.Day < startDate.Day)
        {
            months--;
        }

        return months;
    }



    void Update()
    {
        
    }

    void RefillEnergy()
    {
        
    }

    void AddEnergy(int amount)
    {
        
    }
}



