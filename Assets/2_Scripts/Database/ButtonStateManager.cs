using UnityEngine;
using UnityEngine.UI;

public class ButtonStateManager : MonoBehaviour
{
    public string personName;

    public Sprite[] sprites;

    public Image Image;


    //public Image buttonImage;

    private void Start()
    {
        //buttonImage = GetComponent<Image>();

        UpdateButtonState();
    }

    private void OnEnable()
    {
        //buttonImage = GetComponent<Image>();
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        DatabaseManager databaseManager = DatabaseManager.instance;

        PersonInfo personInfo = databaseManager.personInfos.Find(p => p.name == personName);

        if (personInfo != null && personInfo.isActive)
        {
            Image.sprite = sprites[0];
            GetComponent<Button>().interactable = true;
        }
        else
        {
            Image.sprite = sprites[1];
            GetComponent<Button>().interactable = false;
        }
    }
}
