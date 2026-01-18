using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagmaLabs.Audio;
public class DrillBit : Projectile
{
    public Vector2 velocity;
    public float tickRate;
    Dictionary<DamageHandler, float> debounces = new();


    public void Launch(Vector2 velocity, float damagePerTick, float tickRate)
    {
        damage = 1;
        this.velocity = velocity;
        this.damage = damagePerTick;
        this.tickRate = tickRate;

    }




    public override void HandleCollisions(DamageHandler damageable)
    {
        if (debounces.TryGetValue(damageable, out float nextTime))
        {
            if (Time.time < nextTime)
                return;
        }

        debounces[damageable] = Time.time + (1f / tickRate);

        StartCoroutine(DamageCoroutine(damage, damageable));
        AudioManager.instance.PlaySound(hitSound, ProfileCustomization.playerVolume);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var dmg = col.GetComponent<DamageHandler>();
        if (dmg != null)
            debounces.Remove(dmg);
    }
    override public IEnumerator DamageCoroutine(float damage, DamageHandler damageable)
    {
        yield return null;
        Damage(damage, damageable);

    }

    override protected void Damage(float damage, DamageHandler other)
    {
        other?.Damage(damage, "NegateArmor", null);

    }

    void FixedUpdate()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
