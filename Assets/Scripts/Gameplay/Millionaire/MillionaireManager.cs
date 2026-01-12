using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MillionaireManager : MonoBehaviour
{
    [SerializeField]private TextMeshPro dialogue;

    [SerializeField]private int numLevels;
    [SerializeField]private LootTable[] levelRewards;
    private Ownable[] usedMarbles;

    public enum Lifeline
    {
        FiftyPercent,
        ReuseMarble,
        ExtraTime
    }

    private bool[] lifelines;


    IEnumerator PlayOpening()
    {
        yield break;
    }

    IEnumerator PlayIntro()
    {
        yield return StartCoroutine(AnimateDialogue("Hello and welcome to Marble Millionaire!", 0.2f, 2f));

        StartCoroutine(AnimateDialogue("I'm your host Miles Rolland ", 0.2f, 2f));
        yield break;
    }

    IEnumerator AnimateDialogue(string text, float entryTime, float holdTime)
    {
        yield break;
    }


}
