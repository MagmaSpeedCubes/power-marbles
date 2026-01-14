using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MagmaLabs.Animation;
using MagmaLabs.Economy;

public class GiftBox : TiledElementManager
{
    [SerializeField] private GameObject box, lid;
    [SerializeField] private Image boxImage, lidImage, boxRibbonImage, lidRibbonImage;
    [SerializeField] private GameObject rewardIconPrefab;
    [SerializeField] private float maxHorizontalSize;

    [SerializeField] private Ownable[] rewards;
    private GameObject rewardIconsWrapper;
    [SerializeField] private float openDistance = 2400f;
    [SerializeField] private float openTime = 1f;
    private bool isOpen = false;

    public void OnClick()
    {
        StartCoroutine(Open());
    }
    public IEnumerator Open()
    {

        if(isOpen){yield break;}
        isOpen = true;
        yield return StartCoroutine(LiftLid(openDistance, openTime*1/2f));
        //open the gift box by lifting the lid

        yield return new WaitForSeconds(openTime*1/4f);
        AudioManager.instance.PlaySound("ding", ProfileCustomization.uiVolume);
        rewardIconsWrapper = GenerateRewardIcons(1.15f, openTime*1/4f);
        //generate the reward icons and place them in the box


        Bounds bounds = new Bounds(rewardIconsWrapper.transform.position, Vector3.zero);
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }

        RectTransform wrapperRect = rewardIconsWrapper.GetComponent<RectTransform>();
        float scaleFactor = maxHorizontalSize / wrapperRect.rect.width;
        rewardIconsWrapper.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

        // Position the wrapper so its center matches the configured beginPosition
        // Use transform space conversion so this works regardless of anchor setup
        Vector3 local = box.transform.InverseTransformPoint(centerPosition.position);
        wrapperRect.localPosition = new Vector3(local.x, local.y, 0f);

        // Move the wrapper upward so the icons appear above the gift box (not overlapping)
        float wrapperHeight = wrapperRect.rect.height * scaleFactor;
        wrapperRect.localPosition += new Vector3(0f, wrapperHeight * 0.5f + separationDistance, 0f);
        yield return new WaitForSeconds(openTime*1/4f);
        


    }

    public IEnumerator Close()
    {
        if(!isOpen){yield break;}
        isOpen = false;
        Destroy(rewardIconsWrapper);
        yield return StartCoroutine(LiftLid(-openDistance, openTime));
    }

    IEnumerator LiftLid(float distance, float time)
    {
        float elapsedTime = 0;
        Vector3 startPosition = lid.transform.localPosition;
        Vector3 endPosition = startPosition + Vector3.up * distance;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;
            lid.transform.localPosition = Vector3.Lerp(startPosition, endPosition, Easing.EaseInOutCubic(t));
            yield return null;
        }
    }

    GameObject GenerateRewardIcons(float popOvershoot, float popDuration)
    {
        items = new List<GameObject>();
        numElements = rewards.Length; 
        itemsPerRow = Mathf.CeilToInt(Mathf.Sqrt(numElements));
        GameObject wrapper = new GameObject("Reward Icons Wrapper", typeof(RectTransform));
        // Parent as UI child and keep local coordinates
        wrapper.transform.SetParent(box.transform, false);
        RectTransform wrapperRect = wrapper.GetComponent<RectTransform>();
        wrapperRect.localScale = Vector3.one;
        wrapperRect.anchoredPosition = Vector2.zero;

        RectTransform prefabRect = elementPrefab.GetComponent<RectTransform>();
        float prefabW = prefabRect.rect.width;
        float prefabH = prefabRect.rect.height;

        int rows = Mathf.CeilToInt((float)numElements / itemsPerRow);
        float totalWidth = itemsPerRow * prefabW + (itemsPerRow - 1) * separationDistance;
        float totalHeight = rows * prefabH + (rows - 1) * separationDistance;

        // Calculate starting offset so the grid is centered on the wrapper's origin
        float startX = -totalWidth * 0.5f + prefabW * 0.5f;
        float startY = totalHeight * 0.5f - prefabH * 0.5f;

        for (int i = 0; i < numElements; i++)
        {
            int col = i % itemsPerRow;
            int row = i / itemsPerRow;

            Vector2 offset = new Vector2(
                startX + col * (prefabW + separationDistance),
                startY - row * (prefabH + separationDistance)
            );

            // Instantiate and parent with worldPositionStays = false so local coordinates are used
            GameObject rewardIcon = Instantiate(rewardIconPrefab);
            rewardIcon.transform.SetParent(wrapper.transform, false);
            rewardIcon.name = rewards[i].name + " Icon";
            rewardIcon.GetComponent<Image>().sprite = rewards[i].sprite;
            RectTransform newItemRect = rewardIcon.GetComponent<RectTransform>();
            // Position using anchoredPosition relative to wrapper center
            newItemRect.anchoredPosition = offset;
            StartCoroutine(AnimationManager.instance.PopIn(rewardIcon, popOvershoot, popDuration));

            items.Add(rewardIcon);
        }
        return wrapper;
    }

    public void SetColor(Color boxColor, Color ribbonColor)
    {
        boxImage.color = boxColor;
        lidImage.color = boxColor;
        boxRibbonImage.color = ribbonColor;
        lidRibbonImage.color = ribbonColor;
    }

    public void SetRewards(Ownable[] newRewards)
    {
        rewards = newRewards;
    }

    public Ownable[] GetRewards()
    {
        return rewards;
    }

    

}
