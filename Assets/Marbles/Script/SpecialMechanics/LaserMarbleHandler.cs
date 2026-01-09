using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


public class LaserMarbleHandler : ProjectileMarbleHandler{
    public float rpm;
    public float spawnAngleDegrees;

    protected override void Initialize()
    {
        base.Initialize();
        InvokeRepeating(nameof(SpawnProjectile), spawnDelay, spawnDelay);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        spawnAngleDegrees = (spawnAngleDegrees + Time.deltaTime * rpm / 60 * 360 )%360;
    }
    protected void SpawnProjectile()
    {


        GameObject lsr = Instantiate(projectilePrefab, transform.localPosition, Quaternion.identity);
        lsr.name = "laserBeam";
        lsr.transform.parent = LocalClickManager.localInstance.ballParent.transform;
        lsr.transform.localPosition = transform.localPosition;  


        float spawnAngleRadians = spawnAngleDegrees / 180 * Mathf.PI;
        Vector2 force = new Vector2(Mathf.Cos(spawnAngleRadians), Mathf.Sin(spawnAngleRadians)) * ballData.movementSpeed * 7.5f;
        lsr.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        Quaternion rot=new Quaternion();
        rot.eulerAngles = new Vector3(0, 0, spawnAngleDegrees);
        lsr.transform.rotation = rot;
        Debug.Log("Spawned new laserbeam with force " + force + " at angle " + spawnAngleDegrees);


    }

    public override void OnAbilityTick()
    {
        CancelInvoke();

        float delayTime = spawnDelay - spawnDelay * numBounces/(numBounces + 10);
        InvokeRepeating(nameof(SpawnProjectile), delayTime, delayTime);
        Debug.Log("Restarted laser spawning with delay " + delayTime);
    }
}
