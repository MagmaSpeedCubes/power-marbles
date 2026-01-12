using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagmaLabs.Economy;
using MagmaLabs.Economy.Security;
public class XPManager : AuthorizedModifier
{
    [SerializeField]private Ownable[][] levelRewards;
    [SerializeField]private GiftBox giftbox;
    private Ownable profile;
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
            profile = ScriptableObject.CreateInstance<Ownable>();
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

        List<Ownable> levelUpRewards = new List<Ownable>();
        for(int i=previousLevel+1; i<=levelUp; i++)
        {
            Ownable[] rewards = levelRewards[i];
            levelUpRewards.AddRange(rewards);
        }
        giftbox.SetRewards(levelUpRewards.ToArray());
        yield return StartCoroutine(giftbox.Open());

        foreach(Ownable reward in levelUpRewards)
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
