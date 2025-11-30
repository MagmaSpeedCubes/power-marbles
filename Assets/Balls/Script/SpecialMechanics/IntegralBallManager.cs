using UnityEngine;
using System;
using System.Collections.Generic;
public class IntegralBallManager : BallManager
{
    public static IntegralBallManager instance;
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
        List<BallManager> activeBalls = LevelManager.instance.activeBalls;
        foreach(BallManager ball in activeBalls)
        {
            damage += Utility.CallReturnableFunction<float>("DamageFormulas", ball.ballData.name, ball);
        }
    }

    override public void HandleCollisions(DamageManager damageable)
    {
        float damage = this.damage * ballData.power;

        Damage(damage, damageable);
        asc.PlayOneShot(ballData.bounceSound);

        numBounces++;
        debounce = 0f;
    }
}
