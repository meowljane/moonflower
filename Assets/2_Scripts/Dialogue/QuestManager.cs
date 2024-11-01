using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    //스크립트 캐싱받기
    public AudioManager theAudio;
    public WindowManager theWindow;
    public PlayerManager thePlayer;

    //게임오브젝트 캐싱받기
    public GameObject dialogueWindow;
    public GameObject questObj;
    public GameObject dialObj;

    public SpriteRenderer sprite;
    public Text text;

    private GameObject specialSprite;
    public GameObject btnQuestNext;
    public GameObject btnQuestClose;
    public GameObject btnQuestConfirm;
    public InputField questInput;
    public SpriteRenderer wrongWindow;
    public GameObject btnQuestAnswer;

    //현재 스프라이트 리스트와 인덱스
    public List<Sprite> listSprites;
    public List<InteractionQuest.SpriteAnimation> listSpriteAnimations;
    public List<InteractionQuest.AudioSetting> listAudio;
    public int currentIndex = 0;

    //게임 중 변하는 값(나중에 private 처리해도 됨)
    public bool isTalking = false;
    public int questIndex = 0;
    public string correctAnswer = "feline";

    //이벤트
    public Action onQuestEndedObject;
    public Action onQuestEndedData;

    void Update()
    {
        if (isTalking)
        {
            InputFunc();
        }
    }

    //대화창을 여는 함수
    public void ShowQuest(List<Sprite> sprites, List<InteractionQuest.SpriteAnimation> animations, List<InteractionQuest.AudioSetting> audios)
    {
        if (sprites != null && sprites.Count > 0 && !isTalking)
        {
            //팝업 시 플레이어 이동 제한 조건
            thePlayer.canMove = false;
            isTalking = true;
            theAudio.TestPlay(0, false);

            listSprites = sprites;
            listSpriteAnimations = animations;
            listAudio = audios;
            currentIndex = 0;
            questInput.text = "";

            dialogueWindow.gameObject.SetActive(true);
            questObj.gameObject.SetActive(true);
            dialObj.gameObject.SetActive(false);

            ShowSprite();
            theWindow.OpenWindow(dialogueWindow);
        }
    }

    // 대화창을 닫는 함수
    public void CloseQuest()
    {
        if (sprite != null)
        {
            Debug.Log("닫힐게");
            //플레이어 이동 제한 해제
            thePlayer.canMove = true;
            isTalking = false;

            currentIndex = 0;
            text.text = "";
            questInput.text = "";

            dialogueWindow.gameObject.SetActive(false);
            questObj.gameObject.SetActive(false);
            dialObj.gameObject.SetActive(false);


            //theWindow 스크립트에서 화면 초기화
            theWindow.CloseWindow();
        }
    }

    //정답을 입력해 대화창을 닫는 함수
    public void AnswerQuest()
    {
        if (sprite != null)
        {
            onQuestEndedObject?.Invoke();
            onQuestEndedData?.Invoke();
            Debug.Log("정답이라고할게");
            CloseQuest();
        }
    }

    // 다음 스프라이트를 표시하는 함수
    public void NextSprite()
    {
        if (listSprites != null && currentIndex < listSprites.Count - 1)
        {
            currentIndex++;
            ShowSprite();
        }
    }

    //현재 인덱스에 해당하는 스프라이트를 대화창에 출력
    private void ShowSprite()
    {
        if (listSprites != null && currentIndex < listSprites.Count)
        {
            sprite.sprite = listSprites[currentIndex];
            UpdateButtons();
            theAudio.TestPlay(0, false);
            ControlSound();
        }
    }

    /// <summary>
    /// 정답과 input 받은 값을 비교하여 정답인지 확인한 뒤 이후 이벤트를 진행하는 메서드
    /// </summary>
    public void ConfirmAnswer()
    {
        string inputText = questInput.text.ToUpper();
        if (inputText == correctAnswer || inputText == "FELINE")
        {
            if (questIndex == listSprites.Count)
            {
                AnswerQuest();
            }
            // 정답 입력하면 문제 페이지는 제거
            if (specialSprite != null)
            {
                specialSprite.SetActive(false);
            }
            NextSprite();
        }
        else
        {
            //오답시 입력창 초기화 및 오답 팝업 출력
            questInput.text = "";
            ShowPopupWindowImage();
        }
    }
    /// <summary>
    /// 오답 창 활성화
    /// </summary>
    public void ShowPopupWindowImage()
    {
        wrongWindow.gameObject.SetActive(true);
        Invoke("HidePopupWindow", 1f);
    }

    /// <summary>
    /// 1초 후에 오답 창 비활성화
    /// </summary>
    public void HidePopupWindow()
    {
        wrongWindow.gameObject.SetActive(false);
    }


    private void UpdateButtons()
    {
        if (currentIndex == questIndex - 1)
        {
            btnQuestNext.SetActive(false);
            btnQuestClose.SetActive(true);
            btnQuestConfirm.SetActive(true);
            questInput.gameObject.SetActive(true);
            if (specialSprite != null)
            {
                specialSprite.SetActive(true);
            }
            btnQuestAnswer.SetActive(false);
        }
        else if (currentIndex == listSprites.Count - 1)
        {
            if (btnQuestNext != null)
            {
                btnQuestNext.SetActive(false);
            }
            btnQuestClose.SetActive(false);
            btnQuestConfirm.SetActive(false);
            questInput.gameObject.SetActive(false);
            btnQuestAnswer.SetActive(true);
        }
        else
        {
            if (btnQuestNext != null)
            {
                btnQuestNext.SetActive(true);
            }
            btnQuestClose.SetActive(true);
            btnQuestConfirm.SetActive(false);
            questInput.gameObject.SetActive(false);
            btnQuestAnswer.SetActive(false);
        }
    }

    /// <summary>
    /// Dialogue에 출력되는 Sprite를 종료
    /// </summary>
    public void StopCurrentAnimation()
    {
        Animator animator = sprite.GetComponent<Animator>();
        if (animator != null)
        {
            animator.runtimeAnimatorController = null;
        }
    }

    /// <summary>
    /// Dialogue 순서에 맞게 Sprite 출력
    /// </summary>
    public void ControlAnimation()
    {
        RuntimeAnimatorController animatorController = null;
        foreach (var spriteAnimation in listSpriteAnimations)
        {
            if (spriteAnimation.sprite == listSprites[currentIndex])
            {
                animatorController = spriteAnimation.animatorController;
                break;
            }
        }

        if (animatorController != null)
        {
            Animator animator = sprite.GetComponent<Animator>();
            if (animator != null)
            {
                animator.runtimeAnimatorController = animatorController;
                animator.Play("AnimationState");
            }
        }
    }

    /// <summary>
    /// Dialogue 순서에 맞게 사운드 출력
    /// </summary>
    public void ControlSound()
    {
        foreach (var Pages in listAudio)
        {
            if (Pages.PageNum == currentIndex)
                theAudio.TestPlay(Pages.AudioNum, Pages.isLoop);
        }
    }


    public void InputFunc()
    {
        //closeBtn 오브젝트가 활성화 된 상태에서는 Enter와 esc로 대화 종료.
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (btnQuestConfirm.activeInHierarchy)
            {
                ConfirmAnswer();
            }
            else if (btnQuestAnswer.activeInHierarchy)
            {
                AnswerQuest();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (btnQuestNext.activeInHierarchy)
            {
                NextSprite();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (btnQuestAnswer.activeInHierarchy)
            {
                AnswerQuest();
            }
            else if (btnQuestClose.activeInHierarchy)
            {
                CloseQuest();
            }
        }
    }
}
