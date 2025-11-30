using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public DamageablePrefab prefab;

    protected float armor;
    protected float maxHealth;
    protected float regenRate;

    protected AudioClip damageSound, deathSound;
    [Tooltip("Sprites in order of damaged first one is full health and slowly get more damaged")]
    protected Sprite[] sprites;

    public Infographic[] healthBars;
    public Infographic[] armorBars;
    protected float health;
    


    void Awake()
    {
        Instantiate();

    }

    void Instantiate()
    {
        armor = prefab.armor;
        maxHealth = prefab.maxHealth;
        regenRate = prefab.regenRate;

        damageSound = prefab.damageSound;
        deathSound = prefab.deathSound;

        sprites = prefab.sprites;

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
            
        }
        //regenerate
        int spriteIndex = (health==maxHealth) ? sprites.Length - 1 :(int) (sprites.Length * (health / maxHealth-0.01));
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
        UpdateInfographics();
    }

    virtual public void Damage(float amount, string type, BallManager source = null)
    {
        switch (type)
        {
            case "Chisel":
                armor -= amount;
                if (armor < 0)
                {
                    armor = 0;
                }
                break;
            default:
                health -= amount * (1-armor);
                break;
        }
        

        if (health <= 0)
        {
            Die();
        }
        else
        {
            AudioManager.instance.PlaySound(damageSound, ProfileCustomization.worldVolume * ProfileCustomization.masterVolume);
            UpdateInfographics();
        }
    }

    

    virtual protected void UpdateInfographics()
    {
        foreach (Infographic bar in healthBars)
        {
            bar.SetValue(health);
        }
        foreach (Infographic bar in armorBars)
        {
            bar.SetValue(armor * 100);
        }
    }

    

    virtual protected void Die()
    {
        AudioManager.instance.PlaySound(deathSound, ProfileCustomization.worldVolume * ProfileCustomization.masterVolume);
        Destroy(gameObject);
    }
}


