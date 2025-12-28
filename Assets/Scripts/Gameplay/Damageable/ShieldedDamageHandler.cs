using UnityEngine;

public class ShieldedDamageHandler : DamageHandler
{
    [SerializeField] protected AudioClip shieldHitSound;
    [SerializeField] protected AudioClip shieldBreakSound;
    [SerializeField] protected float shieldPercentage;
    protected float maxShield;
    public Infographic[] shieldBars;
    protected float shieldHealth;


    void Awake()
    {
        health = maxHealth;
        maxShield = maxHealth * shieldPercentage / 100f;
        shieldHealth = maxShield;
        UpdateInfographics();

    }
    void Update()
    {
        if (regenRate > 0 && shieldHealth < maxShield && shieldHealth > 0)
        {
            shieldHealth += regenRate * Time.deltaTime;
            if (shieldHealth > maxShield)
            {
                shieldHealth = maxShield;
            }
            UpdateInfographics();
        }
    }

    override public void Damage(float amount, string type, BallHandler source = null)
    {
        if (shieldHealth > 0)
        {
            shieldHealth -= amount * (1 - armor);

            if (shieldHealth < 0)
            {
                float overflowDamage = -shieldHealth;
                shieldHealth = 0;
                base.Damage(overflowDamage, type);
                AudioManager.instance.PlaySound(shieldBreakSound, ProfileCustomization.worldVolume * ProfileCustomization.masterVolume);
            }
            else
            {
                AudioManager.instance.PlaySound(shieldHitSound, ProfileCustomization.worldVolume * ProfileCustomization.masterVolume);
                UpdateInfographics();
            }
        }
        else
        {
            base.Damage(amount, type);
        }
    }


    override protected void UpdateInfographics()
    {
        base.UpdateInfographics();
        foreach (Infographic bar in shieldBars)
        {
            bar.SetValue(shieldHealth);
        }
    }
}
