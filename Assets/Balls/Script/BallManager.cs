using UnityEngine;
using System;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class BallManager : MonoBehaviour
{
    public Ball ballData;
    
    protected Rigidbody2D rb;
    protected SpriteRenderer spr;
    protected AudioSource asc;
    [HideInInspector]public int numBounces = 0;
    protected static readonly float DEBOUNCE_TIME = 0.1f;
    protected float debounce = DEBOUNCE_TIME;
    void Start()
    {
        Initialize();

    }

    void Update()
    {
        debounce += Time.deltaTime;
    }

    virtual protected void Initialize()
    {
        numBounces = 0;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = ballData.gravity;
        transform.localScale = Vector3.one * ballData.size;
        spr = GetComponent<SpriteRenderer>();
        spr.sprite = ballData.sprite;
        asc = GetComponent<AudioSource>();
        ApplyRandomForce();
        
        LevelManager.instance.AddBall(this);


    }

    protected void ApplyRandomForce()
    {
        float angle;
        do
        {
            angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
        } while (Math.Abs(angle) < Mathf.PI / 6 || Math.Abs(angle) > Mathf.PI / 3); // prevent too shallow angles
        if (UnityEngine.Random.value > 0.5f)
        {
            angle = Mathf.PI - angle;
        }
        //Debug.Log("Initial launch angle (radians): " + angle);
        //Debug.Log("Initial launch angle (degrees): " + angle * Mathf.Rad2Deg);
        // bound angle between 30 and 60 degrees
        Vector2 force = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * UnityEngine.Random.Range(5f, 10f) * ballData.movementSpeed;
        rb.AddForce(force, ForceMode2D.Impulse);
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        DamageManager damageable = collision.gameObject.GetComponent<DamageManager>();


        if (damageable != null && debounce >= DEBOUNCE_TIME)
        {
            HandleCollisions(damageable);
        }


    }

    virtual public void HandleCollisions(DamageManager damageable)
    {
        object damageAmount = Utility.CallReturnableFunction<float>("DamageFormulas", ballData.name, this);
        float damage = Convert.ToSingle(damageAmount);

        Damage(damage, damageable);
        asc.PlayOneShot(ballData.bounceSound);

        numBounces++;
        debounce = 0f;
    }


    virtual protected void Damage(float damage, DamageManager other)
    {
        other?.Damage(damage, ballData.name, this);

    }

    virtual public void OnAbilityTick()
    {
        
    }
}



