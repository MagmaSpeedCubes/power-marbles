using UnityEngine;

public class MitosisManager : BallManager
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
        DamageManager damageable = collision.gameObject.GetComponent<DamageManager>();
        if (damageable != null && debounce >= DEBOUNCE_TIME)
        {
            HandleCollisions(damageable);
        }
    }

    override public void HandleCollisions(DamageManager damageable)
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
                MitosisManager newBallManager = newBall.GetComponent<MitosisManager>();
                newBallManager.ballData = newBall.GetComponent<Ball>();
                newBallManager.localSizeMultiplier = this.localSizeMultiplier / copies;
                newBallManager.numBounces = 0;


                Debug.Log("New ball data: " + newBallManager.ballData.ToString());

            }
            AudioManager.instance.PlaySound(ballData.specialSound, ProfileCustomization.playerVolume * ProfileCustomization.masterVolume);
            Debug.Log("Split complete");
            Destroy(this.gameObject);
            
            

        } 
    }
}
