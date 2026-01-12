using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class AlertHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI body;
    [SerializeField] private GameObject closeButton;

    [SerializeField] private Transform leftButton, middleButton, rightButton;

    void Awake()
    {
        GetComponent<Canvas>().enabled = true;
    }
    void Update()
    {
        if (!GetComponent<Canvas>().enabled)
        {
            Destroy(this.gameObject);
        }
    }

    public void Initialize(string type, string titleText, string[] messages)
    {
        ProfileCustomization pc = ProfileCustomization.instance;
        switch (type)
        {
            case "success":
                title.color = pc.success;
                body.color = pc.success;
                break;
            case "warning":
                title.color = pc.warning;
                body.color = pc.warning;
                break;
            case "error":
                title.color = pc.error;
                body.color = pc.error;
                break;
            default:
                title.color = pc.info;
                body.color = pc.info;
                break;
        }


        title.text = titleText;

        string bodyText = "";
        for(int il = (int)ProfileCustomization.InfoLevel.aesthetic; il < (int)ProfileCustomization.infoLevel; il++)
        {
            bodyText += messages[il + 1] + "\n";
        }
        body.text = bodyText;
        


    }

    public void InitializeButton(GameObject specialButton)
    {
        if(specialButton != null)
        {
            GameObject nsb = Instantiate(specialButton, transform);
            nsb.transform.position = leftButton.position;
            nsb.transform.localScale = new Vector3(1f, 1f, 1f);
            closeButton.transform.position = rightButton.position;
        }
        else
        {
            closeButton.transform.position = middleButton.position;
        }
    }
}
