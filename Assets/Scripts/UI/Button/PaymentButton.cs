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
                SecureProfileStats.instance.ModifyGems(-cost, am);
                break;
            case "coins":
                SecureProfileStats.instance.ModifyCoins(-cost, am);
                break;
            default:
                throw new FormatException("Unrecognized currency");
                break;
        }
    }
}
