using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Projectile : MonoBehaviour
{
    [SerializeField] protected AudioClip hitSound;
    public float damage = 1f;
    public GameObject lastCollide;
    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Trigger Stay");
        lastCollide = collision.gameObject;
        DamageHandler damageable = collision.gameObject.GetComponent<DamageHandler>();
        if (damageable != null)
        {
            HandleCollisions(damageable);
              
        }
 


    }

    virtual public void HandleCollisions(DamageHandler damageable)
    {
        AudioManager.instance.PlaySound(hitSound, ProfileCustomization.masterVolume);
        StartCoroutine(DamageCoroutine(damage, damageable));
        Destroy(this.gameObject);
    }

    virtual public IEnumerator DamageCoroutine(float damage, DamageHandler damageable)
    {
        yield return null;
        Damage(damage, damageable);
    }
    virtual protected void Damage(float damage, DamageHandler other)
    {
        other?.Damage(damage, "projectile", null);

    }

}
