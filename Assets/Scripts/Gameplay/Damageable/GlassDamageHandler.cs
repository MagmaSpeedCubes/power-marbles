using UnityEngine;

public class GlassDamageHandler : DamageHandler
{

    override public void Damage(float amount, string type, BallHandler source = null)
    {
        if (amount >= maxHealth)
        {
            Destroy(source.gameObject);
            base.Damage(maxHealth, type, source);
        }

    }


}
