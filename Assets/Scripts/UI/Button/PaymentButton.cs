using UnityEngine;
using System;
[RequireComponent(typeof(AuthorizedModifier))]
public class PaymentButton : ButtonHandler
{
    [SerializeField] private string currency;
    [SerializeField] private int cost;
    public void OnClick()
    {
        AuthorizedModifier am = GetComponent<AuthorizedModifier>();
        switch (currency)
        {
            case "gems": case "glassMarbles":
                SecureProfileStats.instance.ModifyGlassMarbles(-cost, am);
                break;
            case "xp":
                SecureProfileStats.instance.ModifyXP(-cost, am);
                break;
            default:
                throw new FormatException("Unrecognized currency");
                break;
        }
    }
}
