using UnityEngine;

public class SupportButton : InputButton
{
    [SerializeField] private float value;
    override public void ModifyValue()
    {
        SecureProfileStats.instance.ModifyDevSupport(value, this.gameObject);
    }
}
