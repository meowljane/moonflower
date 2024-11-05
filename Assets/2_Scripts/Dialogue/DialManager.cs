using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InteractionDialogue;


public class DialManager : MonoBehaviour
{
    //스크립트 캐싱받기
    public WindowManager theWindow;
    public PlayerManager thePlayer;

    //게임오브젝트 캐싱받기
    public GameObject dialogueWindow;
    public SpriteRenderer sprite;
    public Text text;
    public GameObject btnNext;
    public GameObject btnClose;

    // Sprite 이미지 리소스
    public List<spriteData> spriteDatas;
    [System.Serializable]
    public struct spriteData
    {
        public string name;
        public Sprite sprite;
    }

    //현재 스프라이트 리스트와 인덱스
    public List<DialogueData> currentList;
    public int currentIndex = 0;

    //게임 중 변하는 불값(InteractionDialogue가 바꿔줌)
    public bool isTalking = false;

    //이벤트
    public Action onDialogueEndedObject;
    public Action onDialogueEndedData;

    void Update()
    {
        if (isTalking)
        {
            InputFunc();
        }
    }

    //대화창을 여는 함수
    public void ShowDialogue(List<DialogueData> dialogueData)
    {
        if (dialogueData != null && dialogueData.Count > 0 && !isTalking)
        {
            //팝업 시 플레이어 이동 제한 조건
            thePlayer.canMove = false;
            isTalking = true;

            dialogueWindow.gameObject.SetActive(true);

            currentList = dialogueData;
            currentIndex = 0;

            ShowSprite();

            theWindow.OpenWindow(dialogueWindow);
        }
    }
    // 대화창을 닫는 함수
    public void CloseDialogue()
    {
        Debug.Log("dial닫힐게");
        if (sprite != null)
        {
            //플레이어 이동 제한 해제
            thePlayer.canMove = true;
            isTalking = false;

            dialogueWindow.gameObject.SetActive(false);

            onDialogueEndedObject?.Invoke();
            onDialogueEndedData?.Invoke();

            currentIndex = 0;
            text.text = "";
            //theWindow 스크립트에서 화면 초기화
            theWindow.CloseWindow();
        }
    }

    // 다음 스프라이트를 표시하는 함수
    public void NextSprite()
    {
        if (currentList != null && currentIndex < currentList.Count - 1)
        {
            currentIndex++;
            ShowSprite();
        }
        else
        {
            currentIndex = 0;
            ShowSprite();
        }
    }

    //현재 인덱스에 해당하는 스프라이트를 대화창에 출력
    private void ShowSprite()
    {
        if (currentList != null && currentIndex < currentList.Count)
        {
            text.text = currentList[currentIndex].dialogueText;

            string optionName = currentList[currentIndex].option.ToString();
            Sprite matchedSprite = null;
            foreach (var spriteData in spriteDatas)
            {
                if (spriteData.name == optionName)
                {
                    matchedSprite = spriteData.sprite;
                    break;
                }
            }
            if (matchedSprite != null)
            {
                sprite.sprite = matchedSprite;
            }

            UpdateButtons();
        }
    }

    private void UpdateButtons()
    {
        if (currentIndex == currentList.Count - 1)
        {
            btnClose.SetActive(true);
            if (btnNext != null)
            {
                btnNext.SetActive(false);
            }
        }
        else
        {
            btnClose.SetActive(false);
            if (btnNext != null)
            {
                btnNext.SetActive(true);
            }
        }
    }

    public void InputFunc()
    {

        //closeBtn 오브젝트가 활성화 된 상태에서는 Enter와 esc로 대화 종료.
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (btnClose.activeInHierarchy)
            {
                CloseDialogue();
            }
        }
        //closeBtn 오브젝트가 비활성화 된 상태에서는 Space키로 넘기는 것만 받아들임
        else if (Input.GetKeyDown(KeyCode.Space) && btnNext.activeInHierarchy)
        {
            NextSprite();
        }
    }
}

