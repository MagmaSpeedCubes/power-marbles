using UnityEngine;
using System;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]

public class CoinBallManager : BallManager
{

    override public void HandleCollisions(DamageManager damageable)
    {
        object gainAmount = Utility.CallReturnableFunction<float>("DamageFormulas", ballData.name, this);
        float gain = Convert.ToSingle(gainAmount);

        LevelStats.energy += gain;
        asc.PlayOneShot(ballData.bounceSound);

        numBounces++;
        debounce = 0f;
    }

}
