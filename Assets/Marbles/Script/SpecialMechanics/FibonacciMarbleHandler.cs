using UnityEngine;
using MagmaLabs.Utilities.Numerics;
public class FibonacciMarbleHandler : BallHandler
{
    override public float GetDamage()
    {

        Debug.Log("Calculating Fibonacci damage for ball with " + numBounces + " bounces.");
        if(numBounces<=1){return ballData.power;}
        return ballData.power * Numerics.Fibonacci(numBounces+1);

    }
}
