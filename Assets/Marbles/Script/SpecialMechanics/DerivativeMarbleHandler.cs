using UnityEngine;
using System.Collections.Generic;
public class DerivativeMarbleHandler : BallHandler
{
    override public float GetDamage()
    {
        List<BallHandler> activeBalls = LevelManager.instance.currentLevel.activeBalls;
        float minDamage = float.MaxValue;
        float maxDamage = float.MinValue;

        foreach(BallHandler otherBall in activeBalls)
        {
            if(otherBall == this){continue;}
            if(otherBall is IntegralMarbleHandler){continue;}
            if(otherBall is DerivativeMarbleHandler){continue;}

            float otherDamage = otherBall.GetDamage();
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
        float derivativeDamage = damageDifference * this.ballData.power;
        Debug.Log("Weakest damage: " + minDamage + ", Strongest damage: " + maxDamage + ", Difference: " + damageDifference + ", Derivative damage (after multiplier): " + derivativeDamage);
        return (derivativeDamage > 0) ? derivativeDamage : 0f;
    }
}
