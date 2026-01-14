using UnityEngine;
using System;
using System.Collections.Generic;
using MagmaLabs.Utilities.Reflection;
public class IntegralMarbleHandler : BallHandler
{
    public static IntegralMarbleHandler instance;
    public float damage;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Only one instance of Integral allowed per level. Destroying duplicate.");
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Initialize();
    }

    override protected void Initialize()
    {
        base.Initialize();
        damage = 0f;
        List<BallHandler> activeBalls = LevelManager.instance.currentLevel.activeBalls;
        foreach(BallHandler ball in activeBalls)
        {
            damage += Utility.CallReturnableFunction<float>("DamageFormulas", ball.ballData.name, ball);
        }
    }

    override public void HandleCollisions(DamageHandler damageable)
    {
        float damage = this.damage * ballData.power;

        Damage(damage, damageable);
        asc.PlayOneShot(ballData.bounceSound);

        numBounces++;
        debounce = 0f;
    }

    override public float GetDamage()
    {
        return damage;
    }
}
