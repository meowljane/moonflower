using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InteractionDialogue : MonoBehaviour
{
    //��ũ��Ʈ ĳ��
    public DialManager theDM;

    //������Ʈ ĳ��
    public GameObject confirmOn;
    public WebGLBtn webglBtn;

    // Sprite �迭 �̸� �޾Ƶδ°�
    public List<DialogueData> dialogueData;

    [System.Serializable]
    public struct DialogueData
    {
        public DialogueOption option;
        [TextArea]
        public string dialogueText;
    }

    public enum DialogueOption
    {
        King,
        Kim,
        Park,
        Lee,
        Choi,
        Detective,
        Boss,
        Ending
    }

    //���� �� ���ϴ� �Ұ�
    public bool isColliding = false;
    public bool isAwake = false;

    //�̺�Ʈ
    public Action onDialogueStartedObject;
    public Action onDialogueStartedData;

    private void Awake()
    {
        theDM = FindFirstObjectByType<DialManager>();
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
        confirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
        if (isAwake)
        {
            SendDialogue();
            webglBtn.isClick = false;
        }
    }

    private void Update()
    {
        if (isColliding && !theDM.isTalking && !isAwake)
        {
            confirmOn.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F) || webglBtn.isClick)
            {
                SendDialogue();
                webglBtn.isClick = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        theDM.isTalking = false;
        confirmOn.SetActive(false);
    }

    void SendDialogue()
    {
        if (theDM != null && dialogueData.Count > 0)
        {
            theDM.ShowDialogue(dialogueData);
            onDialogueStartedData?.Invoke();
            onDialogueStartedObject?.Invoke();
        }
    }

}