using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialManager : MonoBehaviour
{
    //스크립트 캐싱받기
    public AudioManager theAudio;
    public WindowManager theWindow;
    public PlayerManager thePlayer;

    //게임오브젝트 캐싱받기
    public GameObject dialogueWindow;
    public GameObject dialObj;
    public GameObject questObj;

    public SpriteRenderer sprite;
    public Text text;

    public GameObject btnNext;
    public GameObject btnClose;
    public GameObject btnLeft;
    public GameObject btnRight;

    //현재 스프라이트 리스트와 인덱스
    public List<Sprite> listSprites;
    public List<InteractionDialogue.SpriteAnimation> listSpriteAnimations;
    public List<InteractionDialogue.AudioSetting> listAudio;
    public int currentIndex = 0;

    //게임 중 변하는 불값(InteractionDialogue가 바꿔줌)
    public bool isSpinner = false;
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
    public void ShowDialogue(List<Sprite> sprites, List<InteractionDialogue.SpriteAnimation> animations, List<InteractionDialogue.AudioSetting> audios)
    {
        if (sprites != null && sprites.Count > 0 && !isTalking)
        {
            //팝업 시 플레이어 이동 제한 조건
            thePlayer.canMove = false;
            isTalking = true;
            theAudio.TestPlay(0, false);

            dialogueWindow.gameObject.SetActive(true);
            dialObj.gameObject.SetActive(true);
            questObj.gameObject.SetActive(false);

            listSprites = sprites;
            listSpriteAnimations = animations;
            listAudio = audios;
            currentIndex = 0;
            text.text = "※ マウスを使ってください。\r\n （モバイルはタッチ。)";

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
            dialObj.gameObject.SetActive(false);
            questObj.gameObject.SetActive(false);

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
        if (listSprites != null && currentIndex < listSprites.Count - 1)
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

    public void BackSprite()
    {
        if (listSprites != null && currentIndex > 0)
        {
            currentIndex--;
            ShowSprite();
        }
        else
        {
            currentIndex = listSprites.Count;
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

    private void UpdateButtons()
    {
        if (!isSpinner)
        {
            if (currentIndex == listSprites.Count - 1)
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
            btnLeft.SetActive(false);
            btnRight.SetActive(false);
        }
        else
        {
            btnLeft.SetActive(true);
            btnRight.SetActive(true);
            btnClose.SetActive(true);
            btnNext.SetActive(false);
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
        if (isSpinner)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                BackSprite();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                NextSprite();
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                CloseDialogue();
            }
        }
        else
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
}
