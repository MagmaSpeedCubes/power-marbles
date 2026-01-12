using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PoisonMarbleHandler : MarbleHandler
{
    [SerializeField] private float cycleCooldown, damagePerCycle;
    [SerializeField] private int cycles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override public IEnumerator DamageCoroutine(float damage, DamageHandler damageable)
    {
        yield return null;
        int cyclesLeft = cycles;
        do
        {
            Damage(damage, damageable);
            yield return new WaitForSeconds(cycleCooldown);
            cyclesLeft--;
        }while(cyclesLeft > 0);
        yield break;
        
    }

    override public float GetDamage()
    {
        return ballData.power * damagePerCycle;
    }

    override public void OnAbilityTick()
    {
        cycles++;
        damagePerCycle++;
        cycleCooldown = cycleCooldown - 0.5f * cycleCooldown * numBounces/(numBounces + 10f);

    }
}
