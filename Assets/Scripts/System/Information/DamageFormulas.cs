using UnityEngine;
using System;
using System.Collections.Generic;
/*

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


Integral
deals 25% of the combined damage of all balls at its spawning. Only one integral can be spawned per game.

*/

public class DamageFormulas : MonoBehaviour
{
    public float Basic(BallManager ball)
    {
        return ball.ballData.power * 2f;
    }

    public float Speedy(BallManager ball)
    {
        return ball.ballData.power;
    }

    public float Random(BallManager ball)
    {
        System.Random rng = new System.Random();
        return (float) rng.NextDouble() * 2 * ball.ballData.power;
    }



    public float Coin(BallManager ball)
    {
        return ball.ballData.power;
        //energy gained from each bounce
        
    }

    public float Chisel(BallManager ball)
    {
        return ball.ballData.power / 100;
        //amount armor is damaged
        //by percentage so /100
    }

    public float Slammed(BallManager ball)
    {
        return ball.numBounces + 1f;
    }

    public float Mitosis(BallManager ball)
    {
        return ball.ballData.size * 2f;
    }

    public float Fibonacci(BallManager ball)
    {
        Debug.Log("Calculating Fibonacci damage for ball with " + ball.numBounces + " bounces.");
        if(ball.numBounces<=1){return ball.ballData.power;}
        int f1 = 1, f2 = 1;
        for(int i=2; i<ball.numBounces; i++)
        {
            int temp = f2;
            f2 = f1 + f2;
            f1 = temp;
        }
        return f2 * ball.ballData.power;
    }

    public float Derivative(BallManager ball)
    {
        List<BallManager> activeBalls = LevelManager.instance.activeBalls;
        float minDamage = float.MaxValue;
        float maxDamage = float.MinValue;

        foreach(BallManager otherBall in activeBalls)
        {
            if(otherBall == ball){continue;}
            if(otherBall is IntegralBallManager){continue;}
            if(otherBall.ballData.name == "Derivative"){continue;}

            float otherDamage = Utility.CallReturnableFunction<float>("DamageFormulas", otherBall.ballData.name, otherBall);
            if(otherDamage < minDamage)
            {
                minDamage = otherDamage;
            }
            if(otherDamage > maxDamage)
            {
                maxDamage = otherDamage;
            }
        }

        float damageDifference = maxDamage - minDamage;
        float derivativeDamage = damageDifference * ball.ballData.power;
        Debug.Log("Weakest damage: " + minDamage + ", Strongest damage: " + maxDamage + ", Difference: " + damageDifference + ", Derivative damage (after multiplier): " + derivativeDamage);
        return (derivativeDamage > 0) ? derivativeDamage : 0f;
    }






}
