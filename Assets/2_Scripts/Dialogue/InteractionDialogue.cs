using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InteractionDialogue : MonoBehaviour
{
    //스크립트 캐싱
    public DialManager theDM;

    //오브젝트 캐싱
    public GameObject confirmOn;
    public WebGLBtn webglBtn;

    // Sprite 배열 미리 받아두는곳
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
        Detective
    }

    //게임 중 변하는 불값
    public bool isColliding = false;

    //이벤트
    public Action onDialogueStartedObject;
    public Action onDialogueStartedData;

    private void Awake()
    {
        theDM = FindFirstObjectByType<DialManager>();
    }

    private void Update()
    {
        if (isColliding && !theDM.isTalking)
        {
            //confirmOn.SetActive(true);
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
