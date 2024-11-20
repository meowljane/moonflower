using UnityEngine;

public class DialTriggerObject : MonoBehaviour
{
    //�ڱ��ڽ����� �޸� dial ��ũ��Ʈ
    public InteractionDialogue interactionDialogue;

    public DialManager dialManager;

    //Ȱ��ȭ, ��Ȱ��ȭ�� ������Ʈ��
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    //��ȭ �����߿� �����Ұ��ΰ�?
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
