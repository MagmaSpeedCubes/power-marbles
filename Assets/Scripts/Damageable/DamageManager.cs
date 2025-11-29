using UnityEngine;

public class DamageManager : MonoBehaviour
{

    public Damageable damageableData;

    public Infographic[] healthBars;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float regenRate = 0f;
    protected float health;
    


    void Awake()
    {
        health = maxHealth;
        UpdateInfographics();

    }

    void Update()
    {
        if (regenRate > 0 && health < maxHealth)
        {
            health += regenRate * Time.deltaTime;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            UpdateInfographics();
        }
    }

    virtual public void Damage(float amount, string type, BallManager source = null)
    {
        health -= amount * (1-damageableData.armor);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            AudioManager.instance.PlaySound(damageableData.damageSound, ProfileCustomization.worldVolume * ProfileCustomization.masterVolume);
            UpdateInfographics();
        }
    }

    virtual protected void UpdateInfographics()
    {
        foreach (Infographic bar in healthBars)
        {
            bar.SetValue(health);
        }
    }

    

    virtual protected void Die()
    {
        AudioManager.instance.PlaySound(damageableData.deathSound, ProfileCustomization.worldVolume * ProfileCustomization.masterVolume);
        Destroy(gameObject);
    }
}


