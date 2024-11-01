using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InteractionQuest : MonoBehaviour
{
    //��ũ��Ʈ ĳ��
    public QuestManager theQM;

    //������Ʈ ĳ��
    public GameObject confirmOn;
    public WebGLBtn webglBtn;

    // Sprite �迭 �̸� �޾Ƶδ°�
    public List<Sprite> dialogueSprite;
    public List<SpriteAnimation> dialogueAnimation;
    public List<AudioSetting> dialogueAudio;

    //�ν����ͷ� �������ִ� �Ұ�
    public bool isTouch = false;
    public int questIndex = 0;
    public string correctAnswer = "feline";
    public string temptext = "�� �ޫ������Ūêƪ���������\r\n ����Ы���ϫ��ë���)";
    public Sprite wrongSprite;

    //���� �� ���ϴ� �Ұ�
    private bool isColliding = false;

    //�̺�Ʈ
    public Action onQuestStartedData;
    public Action onQuestStartedObject;

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
        theQM = FindFirstObjectByType<QuestManager>();
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
        confirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
    }

    private void Update()
    {
        if (isColliding && !theQM.isTalking)
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
        theQM.isTalking = false;
        confirmOn.SetActive(false);
    }

    void SendDialogue()
    {
        if (theQM != null && dialogueSprite.Count > 0)
        {

            theQM.questIndex = questIndex;
            theQM.correctAnswer = correctAnswer;
            theQM.wrongWindow.sprite = wrongSprite;
            theQM.text.text = temptext;
            onQuestStartedData?.Invoke();
            onQuestStartedObject?.Invoke();

            theQM.ShowQuest(dialogueSprite, dialogueAnimation, dialogueAudio);


        }
    }

}
