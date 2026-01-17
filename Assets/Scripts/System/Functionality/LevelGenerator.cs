using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;
    public GameObject levelPrefab;
    public GameObject wallPrefab;
    public GameObject platformPrefab;

    public DamageablePrefab[] damageablePrefabs;

    public GameObject[] prebuiltLevels;

    float[] rowYs = new float[] { 100f, 300f, 500f, 700f };
    const float platformHeight = 100f;
    const double splitProb = 0.2;    // chance a platform segment will split into two
    const double wallProb = 0.4;     // chance a split will include a wall between parts
    const double missingProb = 0.1;  // chance a platform is removed at the end

    const float minPlatformWidth = 100f; // do not create platforms smaller than this
    const float wallMin = 20f;
    const float wallMax = 80f;



    
    void Awake(){
        if(instance==null){
            instance = this;
        }else{
            Debug.LogWarning("Multiple instances of LevelGenerator detected. Destroying duplicate.");
        }


    }



    public void TestLevelDifficulties(){
        int normal = 0;
        int difficult = 0;
        float difficultCutoff = 185f;
        int testDuration = 300;

        float sum = 0;

        Debug.Log("Testing levels 1-" + testDuration );
        for(int i=0; i<testDuration; i++){
            float td = GetLevelDifficulty(i+1);
            if(td > difficultCutoff){
                difficult++;
                Debug.Log("Level " + (i+1) + " is a difficult level with a difficulty of " + td);
            }else{
                normal++;
                Debug.Log("Level " + (i+1) + " is a normal level with a difficulty of " + td);
            }
            sum += td;

        }

        Debug.Log("Tested levels 1-" + testDuration );
        Debug.Log("# of normal levels: " + normal);
        Debug.Log("# of difficult levels: " + difficult);
        Debug.Log("Average difficulty: " + sum/testDuration);

        
    }
    public void TestGenerateLevel(){
        GenerateLevel(1);
    }

    public float GetLevelDifficulty(int levelNumber){
        System.Random rng = new System.Random(levelNumber * 73856093 ^ 19349663); // better seed mixing
        float difficulty = 100 * levelNumber / (levelNumber + 40) + rng.Next(0, 101) + 50;
        return difficulty;
    }

    public bool IsDifficultLevel(int levelNumber){
        return GetLevelDifficulty(levelNumber) > 185;
    }

    public GameObject LoadLevel(int levelNumber)
    {
        /// <summary>
        /// Loads a prebuilt level if it exists; otherwise generates a new level
        /// </summary>
        /// <param name="levelNumber">The number of the level. Level difficulty increases over time but plateaus.</param>

        if(levelNumber-1 < prebuiltLevels.Length){
            GameObject lp = Instantiate(prebuiltLevels[levelNumber-1], new Vector3(0,2,0), Quaternion.identity);
            return lp;
        }else{
            return GenerateLevel(levelNumber);
        }
    }
    
    public GameObject GenerateLevel(int levelNumber)
    {
        /// <summary>
        /// Generates a level of size 800x by 900y
        /// </summary>
        /// <param name="levelNumber">The number of the level. Level difficulty increases over time but plateaus.</param>

        System.Random rng = new System.Random(levelNumber * 73856093 ^ 19349663);
        float difficulty = 100 * levelNumber / (levelNumber + 40) + rng.Next(-50, 51);



        //difficulty determined by random generation
        
        GameObject lp = Instantiate(levelPrefab, new Vector3(0, 2, 0), Quaternion.identity);
        Rect levelFrame = GetLocalFrameFromRenderers(lp);
        Vector2 localSize = new Vector2(800f, 1000f);
        List<LevelObjectData> levelBase = GeneratePlatforms(rng, levelFrame, localSize);

        int baseHealth = 100;
        int totalHealth = 0;
        for(int i=levelBase.Count-1; i>=0; i--){
            LevelObjectData gen = levelBase[i];
            if(!gen.isWall){
                // Instantiate as a child of the level frame and use local coordinates/sizes
                GameObject newPlatform = Instantiate(platformPrefab, lp.transform);
                newPlatform.transform.localPosition = new Vector3(gen.center.x, gen.center.y, 0f);
                newPlatform.transform.localScale = new Vector3(gen.size.x, gen.size.y, 1f);

                DamageHandler dh = newPlatform.GetComponent<DamageHandler>();
                DamageablePrefab dp = damageablePrefabs[0];

                dh.prefab = dp;
                dh.maxHealth = baseHealth;
                totalHealth += baseHealth;
                baseHealth *= (rng.Next(10) + 1);

            }else{
                // Parent walls to the level frame as well and set local transforms
                GameObject newWall = Instantiate(wallPrefab, lp.transform);
                newWall.transform.localPosition = new Vector3(gen.center.x, gen.center.y, 0f);
                newWall.transform.localScale = new Vector3(gen.size.x, gen.size.y, 1f);
            }
        }

        float maxTime = (float)Math.Log(totalHealth / difficulty) * 10f;
        LevelManager.instance.currentLevel.levelMaxTime = (int)maxTime;
        LevelStats.timeRemaining = (int)maxTime;

        return lp;
    }



    /// <summary>
    /// Generates platform positions and sizes for a level. Deterministic when passed the same System.Random.
    /// - Rows at y = 100, 300, 600, 800 are fully covered by either platforms or walls.
    /// - Each platform can be split into two; when split there's a chance a wall interrupts the two parts.
    /// - After generation, platforms may be randomly removed, but at least two platforms will remain if possible.
    /// This method only returns geometry data and does not instantiate GameObjects.
    /// </summary>
     List<LevelObjectData> GeneratePlatforms(System.Random rng, Rect frame, Vector2 localSize)
    {
        if (rng == null) throw new ArgumentNullException(nameof(rng));

        // Tunable probabilities (deterministic via rng)

        var objects = new List<LevelObjectData>();
        var platformIndices = new List<int>(); // indices into objects list for platforms

        // Reference height used when the original row positions were authored.
        const float referenceHeight = 900f;
        // Map rows into fractions of the reference and then to the provided local size
        var rowFractions = new float[rowYs.Length];
        for (int i = 0; i < rowYs.Length; i++) rowFractions[i] = rowYs[i] / referenceHeight;

        float frameWidth = localSize.x;
        float frameHeight = localSize.y;
        // scale platformHeight (originally authored in reference units) to frame units
        float platformHeightLocal = platformHeight / referenceHeight * frameHeight;

        // We'll also track walls internally to ensure rows remain fully covered (platforms + walls)
        for (int r = 0; r < rowYs.Length; r++)
        {
            // compute local y within the frame (origin bottom-left)
            float y = rowFractions[r] * frameHeight;

            // start with single covering segment [0, frameWidth] and iteratively split
            var segments = new List<(float a, float b)> { (0f, frameWidth) };

            // Keep splitting segments until none split (each segment checked for split chance)
            bool anySplit = true;
            int iterCount = 0;
            while (anySplit)
            {
                anySplit = false;
                var nextSegments = new List<(float a, float b)>();
                //Debug.Log($"Row {r}, iteration {iterCount}: processing {segments.Count} segments");

                foreach (var seg in segments)
                {
                    float segWidth = seg.b - seg.a;
                    //Debug.Log($"  Segment [{seg.a}, {seg.b}] width={segWidth}, minPlatformWidth*2={minPlatformWidth * 2f}");

                    if (segWidth <= minPlatformWidth * 2f)
                    {
                        // too small to reasonably split, keep as-is
                        nextSegments.Add(seg);
                        continue;
                    }

                    if (rng.NextDouble() < splitProb)
                    {
                        //Debug.Log($"    -> SPLIT!");
                        anySplit = true;
                        // choose split location with margin so both sides >= minPlatformWidth
                        float minSplit = minPlatformWidth;
                        float maxSplit = segWidth - minPlatformWidth;
                        double t = rng.NextDouble();
                        float splitAt = (float)(minSplit + t * (maxSplit - minSplit));

                        if (rng.NextDouble() < wallProb)
                        {
                            // place a wall between the two platform parts
                            // choose a wall width
                            double tw = rng.NextDouble();
                            float wallW = Mathf.Clamp((float)(wallMin + tw * (wallMax - wallMin)), wallMin, wallMax);

                            // left platform: [seg.a, seg.a + splitAt)
                            float leftA = seg.a;
                            float leftB = seg.a + splitAt;

                            // wall: [leftB, leftB + wallW)
                            float wallA = leftB;
                            float wallB = Mathf.Min(seg.b, wallA + wallW);

                            // right platform: [wallB, seg.b)
                            float rightA = wallB;
                            float rightB = seg.b;

                            // Only add valid platform parts and record wall object
                            if (leftB - leftA >= minPlatformWidth)
                                nextSegments.Add((leftA, leftB));

                            // create wall object (only if it has positive width)
                            float actualWallW = wallB - wallA;
                            if (actualWallW > 0f)
                            {
                                // wall height randomized between 120 and 200 (scale to frame units)
                                double th = rng.NextDouble();
                                float wallH = (float)(120.0 + th * (200.0 - 120.0));
                                // scale wall height from reference units into the frame
                                float wallHLocal = wallH / referenceHeight * frameHeight;
                                float wx = wallA + actualWallW * 0.5f;
                                // place wall so its bottom sits at the row y (so center.y = y + wallHLocal/2)
                                Vector2 wCenter = new Vector2(wx, y + wallHLocal * 0.5f);
                                Vector2 wSize = new Vector2(actualWallW, wallHLocal);
                                objects.Add(new LevelObjectData(wCenter, wSize, true));
                            }

                            if (rightB - rightA >= minPlatformWidth)
                                nextSegments.Add((rightA, rightB));
                        }
                        else
                        {
                            // split into two contiguous platform parts (no wall)
                            float leftA = seg.a;
                            float leftB = seg.a + splitAt;
                            float rightA = leftB;
                            float rightB = seg.b;

                            if (leftB - leftA >= minPlatformWidth)
                                nextSegments.Add((leftA, leftB));
                            if (rightB - rightA >= minPlatformWidth)
                                nextSegments.Add((rightA, rightB));
                        }
                    }
                    else
                    {
                        // keep as-is
                        nextSegments.Add(seg);
                    }
                }

                segments = nextSegments;
            }

            //Debug.Log($"Row {r} finished with {segments.Count} final platform segments");

            // build platforms from final segments
            foreach (var s in segments)
            {
                float left = s.a;
                float right = s.b;
                float width = right - left;
                if (width <= 0f) continue;
                float cx = left + width * 0.5f;
                // center returned in local frame coordinates (origin bottom-left)
                Vector2 center = new Vector2(cx, y);
                Vector2 size = new Vector2(width, platformHeightLocal);
                platformIndices.Add(objects.Count);
                objects.Add(new LevelObjectData(center, size, false));
            }
        }
        // Randomly remove some platforms but ensure at least two remain if possible
        if (platformIndices.Count <= 2)
        {
            return objects; // nothing to remove
        }

        var keptPlatformIndices = new List<int>();
        var removedPlatformIndices = new List<int>();
        foreach (var pIdx in platformIndices)
        {
            if (rng.NextDouble() >= missingProb)
                keptPlatformIndices.Add(pIdx);
            else
                removedPlatformIndices.Add(pIdx);
        }

        // if too many removed, restore randomly until at least two remain
        if (keptPlatformIndices.Count < 2)
        {
            // shuffle removedPlatformIndices deterministically using rng
            for (int i = removedPlatformIndices.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                int tmp = removedPlatformIndices[i]; removedPlatformIndices[i] = removedPlatformIndices[j]; removedPlatformIndices[j] = tmp;
            }

            int addIdx = 0;
            while (keptPlatformIndices.Count < 2 && addIdx < removedPlatformIndices.Count)
            {
                keptPlatformIndices.Add(removedPlatformIndices[addIdx]);
                addIdx++;
            }
        }

        // Build final object list containing all walls and kept platforms
        var keepSet = new HashSet<int>(keptPlatformIndices);
        var finalObjects = new List<LevelObjectData>();
        for (int i = 0; i < objects.Count; i++)
        {
            var obj = objects[i];
            if (obj.isWall)
                finalObjects.Add(obj);
            else if (keepSet.Contains(i))
                finalObjects.Add(obj);
        }

        // Convert all coordinates from local size space to frame-local space
        // Map from [0, localSize.x/y] to [frame.x, frame.x + frame.width] and [frame.y, frame.y + frame.height]
        var convertedObjects = new List<LevelObjectData>();
        foreach (var obj in finalObjects)
        {
            // Scale center and size from local size coordinates to frame-local coordinates
            Vector2 scaledCenter = new Vector2(
                frame.x + (obj.center.x / localSize.x) * frame.width,
                frame.y + (obj.center.y / localSize.y) * frame.height
            );
            Vector2 scaledSize = new Vector2(
                (obj.size.x / localSize.x) * frame.width,
                (obj.size.y / localSize.y) * frame.height
            );
            convertedObjects.Add(new LevelObjectData(scaledCenter, scaledSize, obj.isWall));
        }

        return convertedObjects;
    }


    // Simple POD to return object geometry without instantiating anything
    public struct LevelObjectData
    {
        public Vector2 center;
        public Vector2 size;
        public bool isWall;

        public LevelObjectData(Vector2 c, Vector2 s, bool wall)
        {
            center = c;
            size = s;
            isWall = wall;
        }
    }

    Rect GetLocalFrameFromRenderers(GameObject lp)
{
    var rends = lp.GetComponentsInChildren<Renderer>();
    if (rends == null || rends.Length == 0)
    {
        // fallback default
        return new Rect(0f, 0f, 800f, 900f);
    }

    Bounds union = rends[0].bounds;
    for (int i = 1; i < rends.Length; i++) union.Encapsulate(rends[i].bounds);

    // bounds.min/max are in world space; convert to local
    Vector3 localMin = lp.transform.InverseTransformPoint(union.min);
    Vector3 localMax = lp.transform.InverseTransformPoint(union.max);

    float width = localMax.x - localMin.x;
    float height = localMax.y - localMin.y;
    return new Rect(localMin.x, localMin.y, width, height);
}
}
