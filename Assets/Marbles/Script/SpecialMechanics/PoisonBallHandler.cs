using UnityEngine;
using System.Collections;

public class PoisonBallHandler : BallHandler
{
    private float damageCooldown;
    private int poisonCycles;
    override protected void Initialize()
    {
        base.Initialize();
        damageCooldown = ballData.GetFloat("poisonCooldown");
        poisonCycles = (int)ballData.GetFloat("poisonCycles");
    }
    override protected void Damage(float damage, DamageHandler other)
    {

        StartCoroutine(PoisonCoroutine(new object[] { damage, damageCooldown, poisonCycles, other }));
    }

    private IEnumerator PoisonCoroutine(object[] args)
    {
        float damage = (float)args[0];
        float cooldown = (float)args[1];
        int cycles = (int)args[2];
        DamageHandler other = (DamageHandler)args[3];
    
        for(int i=0; i < cycles; i++)
        {
            yield return new WaitForSeconds(cooldown);
            other?.Damage(damage, ballData.name, this);
        }



    }

}
