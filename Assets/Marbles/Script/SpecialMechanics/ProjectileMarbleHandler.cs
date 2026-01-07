using UnityEngine;
using System;

public class ProjectileMarbleHandler : BallHandler
{
    [SerializeField]private GameObject projectilePrefab;
    override public void OnAbilityTick()
    {
        for(int i=0; i<numBounces; i++)
        {
            Debug.Log("Spawning new shrapnel");
            GameObject shp = Instantiate(projectilePrefab, transform.localPosition, Quaternion.identity);
            shp.name = "shrapnel";
            shp.transform.parent = LocalClickManager.localInstance.ballParent.transform;
            shp.transform.localPosition = transform.localPosition;  


            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
            Vector2 force = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * UnityEngine.Random.Range(0.5f, 1f) * ballData.movementSpeed;
            shp.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }


    }


}
