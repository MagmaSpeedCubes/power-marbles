using UnityEngine;
using System;

public class BallManager : MonoBehaviour
{
    public Ball ballData;
    
    protected Rigidbody2D rb;
    protected SpriteRenderer spr;
    [HideInInspector]public int numBounces = 0;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = ballData.gravityScale;
        transform.localScale = Vector3.one * ballData.sizeMultiplier;
        spr = GetComponent<SpriteRenderer>();
        spr.sprite = ballData.sprite;
        spr.color = ballData.defaultColor;

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();
        if (damageable != null)
        {

            object damageAmount = ReflectionCaller.CallReturnableFunction<float>("DamageFormulas", ballData.name, this);
            try
            {
                float damage = Convert.ToSingle(damageAmount);
                Damage(damage, damageable);
                
                numBounces++;
            }
            catch (Exception e)
            {
                Debug.LogError("Error converting damage amount: " + e.Message);
            }
            
        }
    }


    virtual public void Damage(float damage, Damageable other)
    {
        other?.Damage(damage, ballData.name);

    }
}
