using UnityEngine;
using UnityEngine.UI;

public class QuestStateManager : MonoBehaviour
{
    public string questName;
    public Sprite inactiveSprite;
    public Sprite activeSprite; 
    public Sprite hintSprite;  

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

    public void UpdateButtonState()
    {
        DatabaseManager databaseManager = DatabaseManager.instance;

        QuestInfo questInfo = databaseManager.questInfos.Find(q => q.questName == questName);

        if (questInfo != null)
        {
            switch (questInfo.status)
            {
                case QuestStatus.NotSeen:
                    buttonImage.sprite = inactiveSprite;
                    GetComponent<Button>().interactable = false;
                    break;
                case QuestStatus.Seen:
                    buttonImage.sprite = activeSprite;
                    GetComponent<Button>().interactable = true;
                    break;
                case QuestStatus.HintSeen:
                    buttonImage.sprite = hintSprite;
                    GetComponent<Button>().interactable = true;
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
