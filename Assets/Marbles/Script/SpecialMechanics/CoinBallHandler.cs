using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]

public class CoinMarbleHandler : BallHandler
{

    override public IEnumerator DamageCoroutine(float damage, DamageHandler damageable)
    {
        yield return null;
        LevelManager.instance.currentLevel.AddEnergy((int)damage);
    }

    virtual public float GetDamage()
    {
        return ballData.power;
    }



}
