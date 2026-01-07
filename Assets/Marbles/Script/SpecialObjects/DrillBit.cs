using UnityEngine;

public class DrillBit : Projectile
{
    public Vector2 velocity;
    public float tickRate;
    private float debounce;


    public void Launch(Vector2 velocity, float damagePerTick, float tickRate)
    {
        damage = 1;
        this.velocity = velocity;
        this.damage = damagePerTick;
        this.tickRate = tickRate;

    }


    override public void HandleCollisions(DamageHandler damageable)
    {
        Debug.Log("Handling collisions");
        if(debounce>0){return;}
        debounce = 1/tickRate;
        StartCoroutine(DamageCoroutine(damage, damageable));
        //delay the damage by one clock cycle so the tilemap can respond first
        AudioManager.instance.PlaySound(hitSound, ProfileCustomization.playerVolume);

    }

    void FixedUpdate()
    {
        debounce -= Time.deltaTime;
        if(debounce<0){debounce=0;}
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
