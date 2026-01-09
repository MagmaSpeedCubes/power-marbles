using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class ProjectileMarbleHandler : BallHandler
{
    [SerializeField]protected GameObject projectilePrefab;
    int queuedProjectiles = 0;
    Coroutine spawnProjectileCoroutine;
    public float spawnDelay;

    override public void OnAbilityTick()
    {
        queuedProjectiles += numBounces;
        if(spawnProjectileCoroutine == null)
        {
            spawnProjectileCoroutine = StartCoroutine(SpawnProjectileCoroutine());
        }


    }

    virtual protected IEnumerator SpawnProjectileCoroutine()
    {

        Debug.Log("Spawning new shrapnel");
        GameObject shp = Instantiate(projectilePrefab, transform.localPosition, Quaternion.identity);
        shp.name = "shrapnel";
        shp.transform.parent = LocalClickManager.localInstance.ballParent.transform;
        shp.transform.localPosition = transform.localPosition;  


        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
        Vector2 force = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * UnityEngine.Random.Range(0.5f, 1f) * ballData.movementSpeed;
        shp.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        queuedProjectiles--;
        yield return new WaitForSeconds(spawnDelay);
        if(queuedProjectiles > 0)
        {
            spawnProjectileCoroutine = StartCoroutine(SpawnProjectileCoroutine());
        }
        else
        {
            spawnProjectileCoroutine = null;
        }
        yield break;
    }



    override public float GetDamage()
    {
        Debug.Log("Power: " + ballData.power);
        return ballData.power;
    }


}
