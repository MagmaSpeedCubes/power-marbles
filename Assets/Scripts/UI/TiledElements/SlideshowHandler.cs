using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagmaLabs.Animation;
public class SlideshowHandler : MonoBehaviour
{
    [SerializeField]private GameObject[] slides;
    [SerializeField]private Transform hideLeft, main, hideRight;
    [SerializeField]private float slideTime;
    private int index;
    private bool isSliding;
    

    void Start()
    {
        index = 0;
        foreach(GameObject slide in slides){
            slide.transform.localPosition = hideRight.localPosition;
        }
        slides[index].transform.localPosition = main.localPosition;
        
    }

    public IEnumerator NextSlideCoroutine()
    {
        if(isSliding){yield break;}
        isSliding = true;
        GameObject current = slides[index];
        index = (index+1)%slides.Length;
        GameObject next = slides[index];

        next.transform.localPosition = hideRight.localPosition;
        StartCoroutine(Slide(current, hideLeft.localPosition, slideTime));
        yield return StartCoroutine(Slide(next, main.localPosition, slideTime)); 
        isSliding = false;  

    }

    public IEnumerator PreviousSlideCoroutine()
    {
        if(isSliding){yield break;}
        isSliding = true;
        GameObject current = slides[index];
        index = (index==0) ? slides.Length-1 : index-1;
        GameObject previous = slides[index];

        previous.transform.localPosition = hideLeft.localPosition;
        StartCoroutine(Slide(current, hideRight.localPosition, slideTime));
        yield return StartCoroutine(Slide(previous, main.localPosition, slideTime));   
        isSliding = false;
    }

    public void ChangeSlide(bool changeToNext)
    {

        if (changeToNext)
        {
            StartCoroutine(NextSlideCoroutine());
        }
        else
        {
            StartCoroutine(PreviousSlideCoroutine());
        }
        
    }

    

    public IEnumerator Slide(GameObject obj, Vector3 endPosition, float duration)
    {
        Vector3 startPosition = obj.transform.localPosition;


        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            obj.transform.localPosition = Vector3.Lerp(startPosition, endPosition, Easing.EaseInOutCubic(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.localPosition = endPosition;
        Debug.Log("End Position: " + obj.transform.localPosition);

    }
}
