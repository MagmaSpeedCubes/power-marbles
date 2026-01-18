using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagmaLabs.Audio;
public class Projectile : MonoBehaviour
{
    [SerializeField] protected AudioClip hitSound;
    public float damage = 1f;

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Trigger Stay");

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
        
    }

    virtual public IEnumerator DamageCoroutine(float damage, DamageHandler damageable)
    {
        yield return null;
        Damage(damage, damageable);
        Destroy(this.gameObject);
    }
    virtual protected void Damage(float damage, DamageHandler other)
    {
        other?.Damage(damage, "projectile", null);

    }

}
