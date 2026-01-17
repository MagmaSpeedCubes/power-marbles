using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class BallHandler : MonoBehaviour
{
    public Ball ballData;
    
    protected Rigidbody2D rb;
    protected SpriteRenderer spr;
    protected AudioSource asc;
    [HideInInspector]public int numBounces = 0;
    protected static readonly float DEBOUNCE_TIME = 0.1f;
    protected float debounce = DEBOUNCE_TIME;

    [SerializeField] protected SpriteRenderer iconSprite;
    protected void Start()
    {
        Initialize();

    }

    protected void Update()
    {
        OnUpdate();   
    }

    virtual protected void Initialize()
    {
        numBounces = 0;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = ballData.gravity;
        // rb.sleepThreshold = 0f;             // Modern Unity
        // rb.sleepAngularVelocity = 0f;       // Legacy compatibility



        transform.localScale = Vector3.one * ballData.size;
        spr = GetComponent<SpriteRenderer>();
        spr.sprite = ballData.ballSprite;
        spr.color = ballData.spriteColor;
        
        iconSprite.sprite = ballData.mainSprite;
        
        
        
        asc = GetComponent<AudioSource>();
        ApplyRandomForce();
        
        LevelManager.instance.currentLevel.AddBall(this);


    }

    virtual protected void OnUpdate()
    {
        debounce += Time.deltaTime;
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
        DamageHandler damageable = collision.gameObject.GetComponent<DamageHandler>();


        if (damageable != null && debounce >= DEBOUNCE_TIME)
        {
            Vector2 velocity = rb.linearVelocity;
            rb.linearVelocity = Vector3.zero;
            //Debug.Log("Stopped ball, handling collision");
            HandleCollisions(damageable);
            //Debug.Log("Reinitializing movement with corrected velocity");
            Vector2 corrected = velocity.normalized * 7.5f * ballData.movementSpeed;
            //Debug.Log("New velocity: " + corrected);
            corrected.y = -corrected.y;
            rb.AddForce(corrected, ForceMode2D.Impulse);  
            debounce = 0;
              
        }
 


    }

    virtual public void HandleCollisions(DamageHandler damageable)
    {
        
        float damage = GetDamage();

        StartCoroutine(DamageCoroutine(damage, damageable));
        //delay the damage by one clock cycle so the tilemap can respond first
        asc.PlayOneShot(ballData.bounceSound);

        numBounces++;

    }

    virtual public IEnumerator DamageCoroutine(float damage, DamageHandler damageable)
    {
        yield return null;
        Damage(damage, damageable);
    }

    public Vector3 GetVelocity()
    {
        return rb.linearVelocity;
    }

    virtual public float GetDamage()
    {
        return ballData.power * numBounces;
    }


    virtual protected void Damage(float damage, DamageHandler other)
    {
        other?.Damage(damage, ballData.name, this);

    }

    virtual public void OnAbilityTick()
    {
        Debug.Log("Ability Tick Activated");
    }
}

public class MarbleHandler : BallHandler
{
    
}



