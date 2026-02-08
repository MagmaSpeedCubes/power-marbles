using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagmaLabs.Economy.Security;
using MagmaLabs.Economy;
public class XPManager : AuthorizedModifier
{
    [SerializeField]private Savable[][] levelRewards;
    [SerializeField]private GiftBox giftbox;
    private Savable profile;
    private Coroutine levelUpCoroutine;
    // Update is called once per frame
    void Start()
    {
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return null;
        profile = SecureProfileStats.instance.FindFirstOwnableOfName("profile");
        if(profile == null)
        {
            profile = ScriptableObject.CreateInstance<Savable>();
            profile.AddTag("level", ""+GetLevel());
            profile.AddTag("hideInInventory", "true");
        }


    }
    void Update()
    {
        if(GetLevel() > int.Parse(profile.FindTag("Level")) && levelUpCoroutine == null)
        {
            levelUpCoroutine = StartCoroutine(LevelUp());

        }
    }

    IEnumerator LevelUp()
    {
        int previousLevel = int.Parse(profile.FindTag("Level"));
        int levelUp = GetLevel();

        List<Savable> levelUpRewards = new List<Savable>();
        for(int i=previousLevel+1; i<=levelUp; i++)
        {
            Savable[] rewards = levelRewards[i];
            levelUpRewards.AddRange(rewards);
        }
        giftbox.SetRewards(levelUpRewards.ToArray());
        yield return StartCoroutine(giftbox.Open());

        foreach(Savable reward in levelUpRewards)
        {
            SecureProfileStats.instance.AddOwnable(reward, this);
        }

        profile.ModifyTagValue("level", ""+levelUp);
        
        SecureProfileStats.instance.OverwriteOwnable(profile, this);
        levelUpCoroutine = null;
    }

    int GetLevel()
    {
        int xp = SecureProfileStats.instance.GetXP();
        return (int)Mathf.Sqrt(xp/100);

    }
}
