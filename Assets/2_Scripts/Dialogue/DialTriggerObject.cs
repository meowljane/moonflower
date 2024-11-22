using UnityEngine;

public class DialTriggerObject : MonoBehaviour
{
    //자기자신한테 달린 dial 스크립트
    public InteractionDialogue interactionDialogue;

    public DialManager dialManager;

    //활성화, 비활성화할 오브젝트들
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    //대화 시작중에 적용할것인가?
    public bool isStartTrigger = false;


    private void Awake()
    {
        interactionDialogue = GetComponent<InteractionDialogue>();
        dialManager = FindObjectOfType<DialManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactionDialogue != null)
        {
            interactionDialogue.onDialogueStartedObject += CheckStart;
            dialManager.onDialogueEndedObject += CheckEnd;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interactionDialogue != null)
        {
            interactionDialogue.onDialogueStartedObject -= CheckStart;
            dialManager.onDialogueEndedObject -= CheckEnd;
        }
    }


    public void CheckStart()
    {
        if (isStartTrigger)
        {
            UpdateObjects();
        }

    }

    public void CheckEnd()
    {
        if (!isStartTrigger)
        {
            UpdateObjects();
        }
    }

    private void UpdateObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }

}
