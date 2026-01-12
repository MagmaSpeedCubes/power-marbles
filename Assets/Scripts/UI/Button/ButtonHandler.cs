using UnityEngine;
using System.Collections;

public class ButtonHandler : UIElementHandler
{
    [SerializeField] protected bool popOnClick = true;
    [ShowIf("popOnClick", true)]
    [SerializeField] protected float popDuration = 0.2f;
    [ShowIf("popOnClick", true)]
    [SerializeField] protected float popScale = 1.2f;

    private bool debounce = false;

    virtual public void OnClick()
    {
        if(debounce)
        {
            return;
        }
        debounce = true;
        if(popOnClick)
        {
            StartCoroutine(PopCoroutine());
        }
        else
        {
            // If there's no pop animation, invoke LateOnClick immediately
            LateOnClick();
        }
    }

    virtual public void LateOnClick()
    {
        debounce = false;
    }

    protected IEnumerator PopCoroutine()
    {
        AudioManager.instance.PlaySoundWithPitchShift("pop", ProfileCustomization.uiVolume);
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * popScale;

        float elapsedTime = 0f;
        while (elapsedTime < popDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / popDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        while (elapsedTime > 0f)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / popDuration);
            elapsedTime -= Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        LateOnClick();
    }
}
