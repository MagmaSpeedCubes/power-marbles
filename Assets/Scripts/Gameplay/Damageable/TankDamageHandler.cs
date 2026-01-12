using UnityEngine;

public class TankDamageHandler : DamageHandler
{
    [SerializeField] protected Infographic[] tankDisplays;
    [SerializeField] protected float tankAmount = 1f;
    [SerializeField] protected AudioClip tankSound;

    void Start()
    {
        Initialize();

    }

    override protected void Initialize()
    {
        base.Initialize();
        foreach(Infographic tank in tankDisplays)
        {
            tank.SetValue(tankAmount);
        }
        
    }
    override public void Damage(float amount, string type, BallHandler source = null)
    {
        if(amount <= tankAmount)
        {
            AudioManager.instance.PlaySound(tankSound, ProfileCustomization.playerVolume * ProfileCustomization.masterVolume);
        }
        else
        {
            base.Damage(amount - tankAmount, type);
        }
        
    }
}
