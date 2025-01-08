using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;


public class InteractionDialogue_Multi : AbstractInteraction
{
    //스크립트 캐싱
    public DialManager_Multi dialManager_Multi;

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
        Detective,
        Boss,
        Ending
    }

    //게임 중 변하는 불값
    public bool isAwake = false;

    //이벤트
    public Action onDialogueStartedObject;
    public Action onDialogueStartedData;

    void Awake()
    {
        if (isAwake)
        {
            SendDialogue();
            webglBtn.isClick = false;
        }
    }

    void Update()
    {
        UpdateMethod();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        dialManager_Multi = playerManager_Multi.canvas.GetComponent<DialManager_Multi>();
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        isActive = false;
    }

    public override void UpdateMethod()
    {
        if (isColliding && !isActive && !isAwake)
        {
            confirmBtn.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F) || webglBtn.isClick)
            {
                SendDialogue();
                webglBtn.isClick = false;
            }
        }
    }

    private void SendDialogue()
    {
        if (dialManager_Multi != null && dialogueData.Count > 0)
        {
            dialManager_Multi.ShowDialogue(dialogueData);
            onDialogueStartedData?.Invoke();
            onDialogueStartedObject?.Invoke();
        }
    }
}
