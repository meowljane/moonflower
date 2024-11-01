using UnityEngine;
using UnityEngine.UI;

public class ItemStateManager : MonoBehaviour
{
    public string itemName;
    public Sprite inactiveSprite;
    public Sprite activeSprite; 

    public GameObject usedObj;   

    private Image buttonImage; 

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        UpdateButtonState();
    }

    private void OnEnable()
    {
        buttonImage = GetComponent<Image>();
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        DatabaseManager databaseManager = DatabaseManager.instance;

        ItemInfo itemInfo = databaseManager.itemInfos.Find(q => q.itemName == itemName);

        if (itemInfo != null)
        {
            switch (itemInfo.status)
            {
                case ItemStatus.NotHave:
                    buttonImage.sprite = inactiveSprite;
                    GetComponent<Button>().interactable = false;
                    usedObj.SetActive(false);
                    break;
                case ItemStatus.Have:
                    buttonImage.sprite = activeSprite;
                    GetComponent<Button>().interactable = true;
                    usedObj.SetActive(false);
                    break;
                case ItemStatus.Used:
                    buttonImage.sprite = activeSprite;
                    GetComponent<Button>().interactable = false;
                    usedObj.SetActive(true);
                    break;
            }
        }
        else
        {
            buttonImage.sprite = inactiveSprite;
            GetComponent<Button>().interactable = false;
        }
    }
}
