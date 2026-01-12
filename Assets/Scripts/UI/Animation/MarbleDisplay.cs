using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MagmaLabs.Economy;
using MagmaLabs.Economy.Security;
public class MarbleDisplay : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Place marble in the center of intended bounce area")]
    [SerializeField] private GameObject fakeMarble;
    [SerializeField] private Image main, temp, background;
    [SerializeField] private BallPrefab[] prefabs;
    private Vector3 defaultPosition;
    private Vector3 targetPosition;
    public List<Sprite> marbleSprites;
    private float secondsInMotion, currentVelocity;

    private int index;

    private bool initialized = false;

    

    void LateStart()
    {
        defaultPosition = transform.localPosition;
        marbleSprites = new List<Sprite>();
        List<Ownable> ownedMarbles = SecureProfileStats.instance.GetMarbles();
        foreach (Ownable ownable in ownedMarbles)
        {
            marbleSprites.Add(ownable.sprite); 
        }
        initialized = true;
        StartCoroutine(LoopSprites(10f, 1f));

    }

    void Update()
    {
        if (!initialized)
        {
            LateStart();
        }


    }

    IEnumerator ChangeSprite(Sprite newSprite, float duration){
        Debug.Log("Changing sprite to " + newSprite);
        float elapsedTime = 0f;
        temp.sprite = newSprite;
        while(elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            main.color = Color.Lerp(Color.white, Color.clear, elapsedTime / duration);
            temp.color = Color.Lerp(Color.clear, Color.white, elapsedTime / duration);
            yield return null;
        }
        main.sprite = newSprite;
        main.color = Color.white;
        temp.color = Color.clear;


    }

    IEnumerator LoopSprites(float timeOnEach, float transitionTime)
    {
        Debug.Log("Starting sprite loop");
        foreach(Sprite sprite in marbleSprites){
            yield return StartCoroutine(ChangeSprite(sprite, transitionTime));
            yield return new WaitForSeconds(timeOnEach);
        }

    }





    public void OnDrag(PointerEventData eventData)
    {

        fakeMarble.transform.position = eventData.position;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        secondsInMotion = 0f;
        currentVelocity = 0f;
        // Set target to the mouse position in world space
        targetPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        secondsInMotion = 0f;
        currentVelocity = 0f;
        // Return target to the default world position so Update moves it back
        targetPosition = defaultPosition;
        float distance = Vector3.Distance(fakeMarble.transform.localPosition, targetPosition);
        StartCoroutine(ReturnToDefaultPosition(0.3f));
    }

    IEnumerator ReturnToDefaultPosition(float duration)
    {
        Vector3 startPosition = fakeMarble.transform.localPosition;
        Vector3 endPosition = defaultPosition;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            fakeMarble.transform.localPosition = Vector3.Lerp(startPosition, endPosition, EaseInOutCubic(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fakeMarble.transform.localPosition = endPosition;
        
    }

    public float EaseInOutCubic(float t)
    {
        if (t < 0.5) return InCubic(t * 2) / 2;
        return 1 - InCubic((1 - t) * 2) / 2;
    }
    public float InCubic(float t) => t * t * t;









    // public float PIDVelocity(float secondsInMotion, float remainingDistance, float currentVelocity)
    // {
    //     float kP = 1f;
    //     float kI = 0.1f;
    //     float kD = 0.01f;
    //     float integral = 0f;
    //     float derivative = 0f;
    //     float error = remainingDistance;
    //     float lastError = error; // initialize to avoid huge derivative on first step
    //     float velocity = currentVelocity;

    //     float dt = Time.deltaTime;
    //     if (dt <= 0f) dt = 0.02f; // fallback if called outside of normal update

    //     // Ensure we run at least one iteration so PID produces an output when motion just started
    //     float simTime = Mathf.Max(secondsInMotion, dt);
    //     for (float t = 0f; t < simTime; t += dt)
    //     {
    //         integral += error * dt;
    //         derivative = (error - lastError) / dt;
    //         velocity = kP * error + kI * integral + kD * derivative;
    //         lastError = error;
    //         // simple prediction of remaining error after applying velocity for t seconds
    //         error = Mathf.Max(0f, remainingDistance - velocity * t);
    //     }
    //     Debug.Log("Velocity: " + velocity);
    //     return velocity;

    // }
}
