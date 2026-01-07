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

    public float Speedy(BallHandler ball)
    {
        return ball.ballData.power;
    }

    public float Random(BallHandler ball)
    {
        System.Random rng = new System.Random();
        return (float) rng.NextDouble() * 2 * ball.ballData.power;
    }

    public float Slammed(BallHandler ball)
    {
        return ball.numBounces + 1f;
    }

    public float Mitosis(BallHandler ball)
    {
        return ball.ballData.size * 2f;
    }

    public float Fibonacci(BallHandler ball)
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








}
