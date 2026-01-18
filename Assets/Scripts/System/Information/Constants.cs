using UnityEngine;
using System;
public class Constants : MonoBehaviour
{
    public static readonly bool DEBUG_MODE = false;
    public const int DEBUG_INFO_LEVEL = 1;
    public static readonly Vector2 SCREEN_SIZE = new Vector2(1200, 2600);

    public static readonly DateTime LAUNCH_DATE = new DateTime(2026, 1, 31);
    public static readonly int TREASURE_HUNT_MAX_ENERGY = 300;
    public static readonly int TREASURE_HUNT_MAP_SIZE = 15;
    public static readonly int TREASURE_HUNT_ENERGY_REGEN_PER_MINUTE = 2;
    public static readonly float TREASURE_HUNT_TILE_SIZE = 1f;

    public static readonly float CAMERA_DEFAULT_SIZE = 10f;

}
