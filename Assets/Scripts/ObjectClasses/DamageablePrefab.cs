using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "DamageablePrefab", menuName = "Scriptable Objects/DamageablePrefab")]
public class DamageablePrefab : ScriptableObject
{
    public float armor = 0f;
    public float maxHealth;
    public float regenRate = 0f;

    public AudioClip damageSound, deathSound;
    public Sprite[] sprites;
    
}