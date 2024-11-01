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
    public List<Sprite> dialogueSprite;
    public List<SpriteAnimation> dialogueAnimation;
    public List<AudioSetting> dialogueAudio;

    //인스펙터로 지정해주는 불값
    public bool isTouch = false;
    public bool isSpinner = false;

    //게임 중 변하는 불값
    public  bool isColliding = false;

    //이벤트
    public Action onDialogueStartedObject;
    public Action onDialogueStartedData;

    [System.Serializable]
    public struct SpriteAnimation
    {
        public Sprite sprite;
        public RuntimeAnimatorController animatorController;
    }

    [System.Serializable]
    public struct AudioSetting
    {
        public int PageNum;
        public int AudioNum;
        public bool isLoop;
    }

    private void Awake()
    {
        theDM = FindFirstObjectByType<DialManager>();
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
        confirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
    }

    private void Update()
    {
        if (isColliding && !theDM.isTalking)
        {
            if (isTouch)
            {
                SendDialogue();
            }
            else
            {
                confirmOn.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F) || webglBtn.isClick)
                {
                    SendDialogue();
                    webglBtn.isClick = false;
                }
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
        if (theDM != null && dialogueSprite.Count > 0)
        {
            theDM.isSpinner = isSpinner;
            theDM.ShowDialogue(dialogueSprite, dialogueAnimation, dialogueAudio);
            onDialogueStartedData?.Invoke();
            onDialogueStartedObject?.Invoke();

        }
    }

}
