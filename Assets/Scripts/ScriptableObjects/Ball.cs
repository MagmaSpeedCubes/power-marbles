using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "Ball", menuName = "Scriptable Objects/Ball")]
public class Ball : ScriptableObject
{
    public Sprite sprite;
    public Color defaultColor;
    public AudioClip bounceSound;
    public AudioClip specialSound;
    public float gravityScale = 1.0f;
    public float powerMultiplier = 1.0f;
    public float sizeMultiplier = 1.0f;
    [HideInInspector] public int numBounces = 0;

}
