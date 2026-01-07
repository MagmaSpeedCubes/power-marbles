using UnityEngine;

public class FibonacciMarbleHandler : BallHandler
{
    override public float GetDamage()
    {

        Debug.Log("Calculating Fibonacci damage for ball with " + numBounces + " bounces.");
        if(numBounces<=1){return ballData.power;}
        int f1 = 1, f2 = 1;
        for(int i=2; i<numBounces; i++)
        {
            int temp = f2;
            f2 = f1 + f2;
            f1 = temp;
        }
        return f2 * ballData.power;

    }
}
