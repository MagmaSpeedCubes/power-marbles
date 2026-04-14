using UnityEngine;

using MagmaLabs;
public class ShowIfTester : MonoBehaviour
{
    [SerializeField]private bool mainToggle;
    [ShowIf("mainToggle", true)]
    [SerializeField]private bool mainIsTrue;

    [ShowIfNot("mainToggle", true)]
    [SerializeField]private bool mainIsFalse;


    [SerializeField]private int testValue;

    [ShowIfGreaterThan("testValue", 5)]
    [SerializeField]private bool greaterThan5;

    [ShowIfLessThan("testValue", 5)]
    [SerializeField]private bool lessThan5;

    [ShowIfGreaterThanOrEqual("testValue", 5)]
    [SerializeField]private bool greaterThanOrEqual5;

    [ShowIfLessThanOrEqual("testValue", 5)]
    [SerializeField]private bool lessThanOrEqual5;

    [ShowIf("testValue", 5)]
    [SerializeField]private bool equals5;

    [ShowIfNot("testValue", 5)]
    [SerializeField]private bool notEquals5;
}
