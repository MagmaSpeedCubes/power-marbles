using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
[CreateAssetMenu(fileName = "BallPrefab", menuName = "Scriptable Objects/BallPrefab")]
public class BallPrefab : ScriptableObject
{

    
    public Color defaultColor;
    public AudioClip bounceSound;
    public AudioClip specialSound;
    public float gravity = 1.0f;
    public float power = 1.0f;
    public float size = 1.0f;
    public float movementSpeed = 1.0f;
    public int price = 10;
    


    /*
    Upgradeable attributes of every ball
    Gravity: stronger gravity
    Power: more damage
    Size: larger
    Movement Speed: moves faster
    Tick Speed: If abilities, 
    */



}
