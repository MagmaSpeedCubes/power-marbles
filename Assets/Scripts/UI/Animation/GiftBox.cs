using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
        yield return StartCoroutine(LiftLid(openDistance, openTime));
        //open the gift box by lifting the lid

        rewardIconsWrapper = GenerateRewardIcons();
        //generate the reward icons and place them in the box


        Bounds bounds = new Bounds(rewardIconsWrapper.transform.position, Vector3.zero);
        foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }

        float scaleFactor = maxHorizontalSize / rewardIconsWrapper.GetComponent<RectTransform>().rect.width;
        rewardIconsWrapper.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

        rewardIconsWrapper.transform.localPosition = new Vector3(-maxHorizontalSize/2 + separationDistance, rewardIconsWrapper.transform.position.y, rewardIconsWrapper.transform.position.z);
        


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
            lid.transform.localPosition = Vector3.Lerp(startPosition, endPosition, CustomFunctions.EaseInOutCubic(t));
            yield return null;
        }
    }

    GameObject GenerateRewardIcons()
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
        for (int i = 0; i < numElements; i++)
        {
            Vector3 offset = new Vector3(
                (i % itemsPerRow) * (prefabRect.rect.width + separationDistance),
                -(int)(i / itemsPerRow) * (prefabRect.rect.height + separationDistance), 0
            );


            // Instantiate without parenting then set parent with worldPositionStays = false
            GameObject rewardIcon = Instantiate(rewardIconPrefab);
            rewardIcon.transform.SetParent(wrapper.transform, false);
            rewardIcon.name = rewards[i].name + " Icon";
            rewardIcon.GetComponent<Image>().sprite = rewards[i].sprite;
            RectTransform newItemRect = rewardIcon.GetComponent<RectTransform>();
            // Position in local space relative to the beginPosition
            newItemRect.localPosition = beginPosition.localPosition + offset;
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
