using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagmaLabs.Economy;
using MagmaLabs.Economy.Security;
public class OwnableManager : MonoBehaviour
{
    [SerializeField] private Ball[] marblePrefabs;
    public List<Ball> ownedMarbles;
    public static OwnableManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of OwnableManager detected. Destroying duplicate.");
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        List<Ownable> marbles = SecureProfileStats.instance.GetMarbles();
        foreach(Ownable marble in marbles)
        {

                
            string removeBall = marble.name.ToLower().Replace("ball", string.Empty);
            string name = removeBall.Replace("marble", string.Empty);
            foreach(Ball prefab in marblePrefabs)
            {
                string testName = prefab.name.ToLower();
                if (name.Equals(testName))
                {
                    ownedMarbles.Add(prefab);
                }
            }




        }
    }

    public List<Ball> GetOwnedMarblePrefabs()
    {
        return ownedMarbles;
    }
}
