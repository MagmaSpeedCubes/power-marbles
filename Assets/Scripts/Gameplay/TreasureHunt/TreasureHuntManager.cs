using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System;
using TMPro;
using System.Collections.Generic;
using MagmaLabs.Economy.Security;
using MagmaLabs.Utilities;
using MagmaLabs.Economy;
[RequireComponent(typeof(AuthorizedModifier))]
public class TreasureHuntManager : MonoBehaviour
{
    
    public static TreasureHuntManager instance;
    public int energy;
    public DateTime lastRecharge;
    public TreasureHuntTile[] tiles;
    
    public TreasureHuntTile barrier;
    public GameObject[] relics;
    public GameObject[] relicGiftBoxes;
    public GameObject lootPrefab;
    public LootTable[] relicLootTables;
    public LootTable[] tileLootTables;
    

    public GameObject tileMap;
    public GameObject gridPrefab;
    public Ownable treasureHuntData, treasureHuntWorldMap, treasureHuntRelicMap;
    private int season;
    private Coroutine minuteUpdater;

    private List<int> queuedRelics = new List<int>();
    private Coroutine relicHandler;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Color damagedText, lowText;
    [SerializeField] private GameObject giftCanvas;

    
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

        season = (int)(CalculateTotalMonths(Constants.LAUNCH_DATE, DateTime.UtcNow) / 2);

        treasureHuntData = SecureProfileStats.instance.FindFirstOwnableOfName("treasureHuntDataSeason" + season);
        treasureHuntWorldMap = SecureProfileStats.instance.FindFirstOwnableOfName("treasureHuntWorldMapSeason" + season);
        treasureHuntRelicMap = SecureProfileStats.instance.FindFirstOwnableOfName("treasureHuntRelicMapSeason" + season);
        

        if(treasureHuntData == null || treasureHuntWorldMap == null || treasureHuntRelicMap == null)
        {
            treasureHuntData = new Ownable("treasureHuntDataSeason" + season, SpriteManager.instance.placeholder);
            
            energy = Constants.TREASURE_HUNT_MAX_ENERGY;
            treasureHuntData.AddTag("energy", ""+energy);
            treasureHuntWorldMap = GenerateTreasureHuntWorldMap();
            treasureHuntRelicMap = GenerateTreasureHuntRelicMap();
            lastRecharge = DateTime.UtcNow;
            SerializableDateTime sdt = new SerializableDateTime(DateTime.UtcNow);
            treasureHuntData.AddTag("lastRecharge", sdt.ToString());
            

        }
        else
        {
            string serialized = treasureHuntData.FindTag("lastRecharge");
            SerializableDateTime sdt = new SerializableDateTime(serialized);
            lastRecharge = sdt.ToDateTimeUtc();
            
        }



        
        Debug.Log("Map to string: " + treasureHuntWorldMap.Serialize());
        tileMap = LoadWorldFromOwnable(treasureHuntWorldMap, treasureHuntRelicMap);

        lastRecharge = UpdateEnergy(lastRecharge);
        

        minuteUpdater = StartCoroutine(MinuteUpdate());
        
        // Save newly created data immediately
        AutoSave();
    }

    void OnDestroy()
    {
        AutoSave();
    }

    public void AutoSave()
    {


        
        AuthorizedModifier source = GetComponent<AuthorizedModifier>();
        SecureProfileStats sps = SecureProfileStats.instance;
        sps.OverwriteOwnable(treasureHuntData, source);

        sps.OverwriteOwnable(treasureHuntWorldMap, source);

        sps.OverwriteOwnable(treasureHuntRelicMap, source);


    }

    public void RemoveTileAt(Vector2 coords)
    {
        Destroy(tileMap);
        tileMap = LoadWorldFromOwnable(treasureHuntWorldMap, treasureHuntRelicMap);
    }

    public void RemoveRelicAt(Vector2 coords)
    {
        Destroy(tileMap);
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
        Ownable output = new Ownable("treasureHuntWorldMapSeason" + season, SpriteManager.instance.placeholder);
        int goldPerMap = 30;
        int diamondPerMap = 15;
        int treasurePerMap = 10;
        
        for(int y=0; y<Constants.TREASURE_HUNT_MAP_SIZE; y++)
        {
            for(int x=0; x<Constants.TREASURE_HUNT_MAP_SIZE; x++)
            {
                Func<int, int> Invert = x => Constants.TREASURE_HUNT_MAP_SIZE-x;
                if (y == Invert(1))
                {
                    output.AddTag(""+x+"-"+y, "grass");
                    output.AddTag(""+x+"-"+y+"Health", "3");
                }
                else if (y > Invert(3))
                {
                    output.AddTag(""+x+"-"+y, "dirt");
                    output.AddTag(""+x+"-"+y+"Health", "5");
                }
                else if (y > Invert(8))
                {
                    output.AddTag(""+x+"-"+y, "stone");
                    output.AddTag(""+x+"-"+y+"Health", "20");
                }
                else
                {
                    output.AddTag(""+x+"-"+y, "deepslate");
                    output.AddTag(""+x+"-"+y+"Health", "100");
                }

            }
        }
        //spawn base layers of blocks

        do
        {
            int randX = UnityEngine.Random.Range(0, Constants.TREASURE_HUNT_MAP_SIZE);
            int randY = UnityEngine.Random.Range(0, Constants.TREASURE_HUNT_MAP_SIZE);
            string tileAtRandom = output.FindTag(""+randX+"-"+randY);
            if (tileAtRandom.Equals("stone"))
            {
                output.ModifyTagValue(""+randX+"-"+randY, "stoneGold");
                goldPerMap--;
            }else if (tileAtRandom.Equals("deepslate"))
            {
                output.ModifyTagValue(""+randX+"-"+randY, "deepslateGold");
                goldPerMap--;
            }

        }while(goldPerMap > 0);
        //spawn gold in stone layers

        do
        {
            int randX = UnityEngine.Random.Range(0, Constants.TREASURE_HUNT_MAP_SIZE);
            int randY = UnityEngine.Random.Range(0, Constants.TREASURE_HUNT_MAP_SIZE);
            string tileAtRandom = output.FindTag(""+randX+"-"+randY);
            if (tileAtRandom.Equals("deepslate"))
            {
                output.ModifyTagValue(""+randX+"-"+randY, "deepslateDiamond");
                diamondPerMap--;
            }
        }while(diamondPerMap > 0);
        //spawn diamond in deepslate layers

        return output;
        
    }


    public Ownable GenerateTreasureHuntRelicMap()
    {
        Ownable output = new Ownable("treasureHuntRelicMapSeason" + season, SpriteManager.instance.placeholder);
        int relicsPerWorld = 50;
        do
        {
            int randX = UnityEngine.Random.Range(0, Constants.TREASURE_HUNT_MAP_SIZE);
            int randY = UnityEngine.Random.Range(0, Constants.TREASURE_HUNT_MAP_SIZE);
            string tileAtRandom = output.FindTag(""+randX+"-"+randY);
            if (tileAtRandom == null)
            {
                output.AddTag(""+randX+"-"+randY, ""+UnityEngine.Random.Range(0, relics.Length));
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
        grid.cellSize = new Vector3(Mathf.Sqrt(3), 2f, 2f) * Constants.TREASURE_HUNT_TILE_SIZE; // or adjust as needed

        GameObject tilemapGO = new GameObject("HexTilemap");
        tilemapGO.transform.SetParent(gridGO.transform);
        Tilemap tilemap = tilemapGO.AddComponent<Tilemap>();
        tilemap.tileAnchor = new Vector3(0, 0, 0);
        TilemapRenderer renderer = tilemapGO.AddComponent<TilemapRenderer>();
        TilemapCollider2D collider = tilemapGO.AddComponent<TilemapCollider2D>();
        Rigidbody2D rb = tilemapGO.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        
        TreasureTilemapHandler handler = tilemapGO.AddComponent<TreasureTilemapHandler>();
        handler.treasureTilemap = tilemap;

        TextMeshPro[,] healthText = new TextMeshPro[Constants.TREASURE_HUNT_MAP_SIZE,Constants.TREASURE_HUNT_MAP_SIZE];
        GameObject[,] relicObjects = new GameObject[Constants.TREASURE_HUNT_MAP_SIZE,Constants.TREASURE_HUNT_MAP_SIZE];
        for(int y = -1; y <= Constants.TREASURE_HUNT_MAP_SIZE+5; y++)
        {
            Vector3Int pos = new Vector3Int(-1, y, 0);
            tilemap.SetTile(pos, barrier);
            pos = new Vector3Int(Constants.TREASURE_HUNT_MAP_SIZE, y, 0);
            tilemap.SetTile(pos, barrier);
        }
        
        for(int x = 0; x <= Constants.TREASURE_HUNT_MAP_SIZE; x++)
        {
            Vector3Int pos = new Vector3Int(x, -1, 0);
            tilemap.SetTile(pos, barrier);
        }
        for (int x = 0; x < Constants.TREASURE_HUNT_MAP_SIZE; x++)
        {

            for (int y = 0; y < Constants.TREASURE_HUNT_MAP_SIZE; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                Vector3 offset = (y%2==0) ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 0f, 0f);
                if(y==0){
                    offset = new Vector3(0.5f, 0f, 0f);
                    //idk why i need this edge case hardcode the first row is just weird
                }
                Vector3 localPos = tilemap.CellToLocalInterpolated((Vector3)pos + offset);
                string tileAt = worldMap.FindTag(""+x+"-"+y);
                //Debug.Log("Tile at "+x+"-"+y+": " + tileAt);

                foreach(TreasureHuntTile tile in tiles)
                {
                    if (tile.name.Equals(tileAt))
                    {
                        

                        tilemap.SetTile(pos, tile);

                        GameObject textObject = Instantiate(textPrefab, tilemapGO.transform);
                        textObject.transform.localPosition = localPos;


                        TextMeshPro textComponent = textObject.GetComponent<TextMeshPro>();
                        textComponent.text = ""+x+", "+y;
                        healthText[x,y] = textComponent;

                        break;
                    }
                }
                //place the tile at the desired location
                if (relicMap.FindTag(""+x+"-"+y)!=null)
                {
                    int index = int.Parse(relicMap.FindTag(""+x+"-"+y));
                    GameObject relic = Instantiate(relics[index], tilemapGO.transform);
                    relicObjects[x,y] = relic;

                    relic.transform.localPosition = localPos;
                    RelicHandler rm = relic.GetComponent<RelicHandler>();


                    rm.tileX = x;
                    rm.tileY = y;
                    rm.type = index;
                    
                }
                //spawns relic if exists



                //Debug.Log("Spawned tile and relic for tile " + x + ", " + y);
                //Tile tileToPlace = 
                
            }
        }

        handler.healthGrid = healthText;
        handler.relicGrid = relicObjects;
        handler.worldMap = treasureHuntWorldMap;
        handler.damaged = damagedText;
        handler.low = lowText;

        int midpoint = (Constants.TREASURE_HUNT_MAP_SIZE-1)/2;
        Vector3Int midpointCell = new Vector3Int(midpoint, midpoint, 0);
        Vector3 midpointPos = tilemap.CellToLocalInterpolated((Vector3)midpointCell);
        
        tilemapGO.transform.localPosition = -midpointPos;
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

    public void HandleTileBreak(Tilemap tilemap, Vector3Int tilePos)
    {
        string tileAt = treasureHuntWorldMap.FindTag(""+tilePos.x+"-"+tilePos.y);
        foreach(LootTable lt in tileLootTables)
        {
            if (lt.name.Equals(tileAt))
            {
                StartCoroutine(OnTileBreak(tilemap, tilePos, lt));
                break;
            }
        }

    }
    public IEnumerator OnTileBreak(Tilemap tilemap, Vector3Int tilePos, LootTable lt)
    {

        

        treasureHuntWorldMap.ModifyTagValue(""+tilePos.x+"-"+tilePos.y+"Health", "0");
        treasureHuntWorldMap.ModifyTagValue(""+tilePos.x+"-"+tilePos.y, "air");
        tilemap.SetTile(tilePos, null);
        tilemap.GetComponent<TilemapCollider2D>().ProcessTilemapChanges();


        Ownable lootWon = lt.GetLoot();
        GameObject lootObject = Instantiate(lootPrefab, tilemap.transform);
        lootObject.GetComponent<SpriteRenderer>().sprite = lootWon.sprite;
        lootObject.AddComponent<CanvasGroup>();





        Vector3Int pos = new Vector3Int(tilePos.x, tilePos.y, 0);
        Vector3 offset = (tilePos.y%2==0) ? new Vector3(0.5f, 0.5f, 0f) : new Vector3(0.5f, 0.5f, 0f);
        if(tilePos.y==0){
            offset = new Vector3(0.5f, 0.5f, 0f);
            //idk why i need this edge case hardcode the first row is just weird
        }
        Vector3 localPos = tilemap.CellToLocalInterpolated((Vector3)pos + offset);
        lootObject.transform.localPosition = localPos;

        AnimationManager am = AnimationManager.instance;
        yield return StartCoroutine(am.PopIn(lootObject, 1.15f, 0.5f));
        SecureProfileStats.instance.AddOwnable(lootWon, GetComponent<AuthorizedModifier>());
        yield return StartCoroutine(am.FadeSprite(lootObject, 1f, 0f, 1.5f));
        Destroy(lootObject);
        
        
    }
    public void HandleRelicLoot(Vector3Int position, int type)
    {
        queuedRelics.Add(type);
        if(relicHandler == null)
        {
            relicHandler = StartCoroutine(RelicHandlingCoroutine());
        }
    }

    IEnumerator RelicHandlingCoroutine()
    {

        int index = queuedRelics[0];
        queuedRelics.Remove(index);
        LootTable lt = relicLootTables[index];
        List<Ownable> rewards = new List<Ownable>();
        for(int i=0; i<5; i++)
        {
            rewards.Add(lt.GetLoot());
        }
        
        GameObject relicGiftbox = Instantiate(relicGiftBoxes[index], giftCanvas.transform);
        relicGiftbox.transform.localScale = new Vector3(1, 1, 1);
        GiftBox gb = relicGiftbox.GetComponent<GiftBox>();

        gb.SetRewards(rewards.ToArray());
        yield return StartCoroutine(gb.Open());
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(AnimationManager.instance.FadeUI(relicGiftbox, 1f, 0f, 0.5f));
        Destroy(relicGiftbox);

        if(queuedRelics.Count > 0)
        {
            relicHandler = StartCoroutine(RelicHandlingCoroutine());
        }
        else
        {
            relicHandler = null;
        }
        
        yield break;
    }

    IEnumerator MinuteUpdate()
    {
        lastRecharge = UpdateEnergy(lastRecharge);
        SerializableDateTime sdt = new SerializableDateTime(lastRecharge);
        treasureHuntData.ModifyTagValue("lastRecharge", ""+sdt.ToString());
        treasureHuntData.ModifyTagValue("energy", ""+energy);
        
        
        AutoSave();
        yield return new WaitForSeconds(60f);
        minuteUpdater = StartCoroutine(MinuteUpdate());
    }


    void AddEnergy(int amount)
    {
        energy += amount;
        if(energy > Constants.TREASURE_HUNT_MAX_ENERGY)
        {
            energy = Constants.TREASURE_HUNT_MAX_ENERGY;
        }
    }

    public bool UseEnergy(int amount){
        if(Constants.DEBUG_MODE){
            return true;
        }
        if(amount > energy){
            return false;
        }else{

            energy -= amount;

            
            
            AutoSave();
            return true;
        }
    }

    

    DateTime UpdateEnergy(DateTime last)
    {
        TimeSpan timePassed = DateTime.UtcNow - last;
        int energyToRegen = timePassed.Minutes * Constants.TREASURE_HUNT_ENERGY_REGEN_PER_MINUTE;
        energy += energyToRegen;
        if(energy > Constants.TREASURE_HUNT_MAX_ENERGY)
        {
            energy = Constants.TREASURE_HUNT_MAX_ENERGY;
        }
        AutoSave();
        return last.AddMinutes(timePassed.Minutes);
        


    }
}



