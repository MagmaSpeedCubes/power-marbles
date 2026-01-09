using UnityEngine;

public class MitosisBallHandler : BallHandler
{
    [SerializeField] protected float bounceGrowAmount = 0.1f;
    [SerializeField] protected int copies = 2;
    
    public float localSizeMultiplier = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    override protected void Initialize()
    {
        base.Initialize();
        transform.localScale = Vector3.one * ballData.size * localSizeMultiplier;
    }

    void Update()
    {
        debounce += Time.deltaTime;
        transform.localScale = Vector3.one * ballData.size * localSizeMultiplier;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        DamageHandler damageable = collision.gameObject.GetComponent<DamageHandler>();
        if (damageable != null && debounce >= DEBOUNCE_TIME)
        {
            HandleCollisions(damageable);
        }
    }

    override public void HandleCollisions(DamageHandler damageable)
    {
        base.HandleCollisions(damageable);
        localSizeMultiplier += bounceGrowAmount;
        transform.localScale = Vector3.one * ballData.size * localSizeMultiplier;

        if(localSizeMultiplier > 1.0f)
        {
            Debug.Log("Beginning split");
            //split into two balls of half size each
            for(int i=0; i < copies; i++)
            {
                Debug.Log("Split #" + i);
                GameObject newBall = Instantiate(this.gameObject, transform.position, Quaternion.identity);
                newBall.name = this.name;
                MitosisBallHandler newBallHandler = newBall.GetComponent<MitosisBallHandler>();
                newBallHandler.ballData = newBall.GetComponent<Ball>();
                newBallHandler.localSizeMultiplier = this.localSizeMultiplier / copies;
                newBallHandler.numBounces = 0;


                Debug.Log("New ball data: " + newBallHandler.ballData.ToString());

            }
            AudioManager.instance.PlaySound(ballData.specialSound, ProfileCustomization.playerVolume * ProfileCustomization.masterVolume);
            Debug.Log("Split complete");
            Destroy(this.gameObject);
            
            

        } 
    }

    override public float GetDamage()
    {
        return ballData.size * 2f;
    }
}
