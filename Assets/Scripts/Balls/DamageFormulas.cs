using UnityEngine;
using System;
/*

--Rare--


Mitosis
starts off as smaller than a basic ball
increases in size to a basic ball
divides into two clones of the initial mitosis ball

Grenade
every ability tick, releases a number of small balls each dealing 1 damage

Pickaxe
has a rotating pickaxe that deals damage when in collision with a damageable

--Epic--


Poison
each bounce causes the damageable to continue taking damage at a certain rate

Drill
every ability tick, releases a drill bit that deals damage to all below damageables

Paranormal
deals damage completely ignoring armor



--Legendary--


Laser
has a rotating laser that deals damage in its direction regardless of distance of obstacle. Laser is stopped when hit.



--Mythical--


Derivative
deals the difference in damage between the weakest and strongest balls discounting Integral and copies of itself. 

Integral
deals 25% of the combined damage of all balls at its spawning. Only one integral can be spawned per game.

*/

public class DamageFormulas : MonoBehaviour
{
    public float Basic(BallManager ball)
    {
        return ball.ballData.powerMultiplier * 2f;
    }

    public float Sticky(BallManager ball)
    {
        return ball.ballData.powerMultiplier;
    }

    public float Speedy(BallManager ball)
    {
        return ball.ballData.powerMultiplier;
    }

    public float Random(BallManager ball)
    {
        System.Random rng = new System.Random();
        return (float) rng.NextDouble() * 2 * ball.ballData.powerMultiplier;
    }

    public float Mace(BallManager ball)
    {
        return 0;
        //mace needs its own dedicated controller tracking height movement
    }

    public float Grower(BallManager ball)
    {
        return ball.ballData.powerMultiplier;
    }

    public float Coin(BallManager ball)
    {
        return 0;
    }

    public float Chisel(BallManager ball)
    {
        return ball.ballData.powerMultiplier * 0.1f;
        //chisel is very weak but damages armor
    }

    public float Slammed(BallManager ball)
    {
        return ball.numBounces + 1f;
    }

    public float Mitosis(BallManager ball)
    {
        return ball.ballData.sizeMultiplier * 2f;
    }

    public float Fibonacci(BallManager ball)
    {
        Debug.Log("Calculating Fibonacci damage for ball with " + ball.numBounces + " bounces.");
        if(ball.numBounces<=1){return ball.ballData.powerMultiplier;}
        int f1 = 1, f2 = 1;
        for(int i=2; i<ball.numBounces; i++)
        {
            int temp = f2;
            f2 = f1 + f2;
            f1 = temp;
        }
        return f2 * ball.ballData.powerMultiplier;
    }





}
