using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialManager : MonoBehaviour
{
    //��ũ��Ʈ ĳ�̹ޱ�
    public AudioManager theAudio;
    public WindowManager theWindow;
    public PlayerManager thePlayer;

    //���ӿ�����Ʈ ĳ�̹ޱ�
    public GameObject dialogueWindow;
    public GameObject dialObj;
    public GameObject questObj;

    public SpriteRenderer sprite;
    public Text text;

    public GameObject btnNext;
    public GameObject btnClose;
    public GameObject btnLeft;
    public GameObject btnRight;

    //���� ��������Ʈ ����Ʈ�� �ε���
    public List<Sprite> listSprites;
    public List<InteractionDialogue.SpriteAnimation> listSpriteAnimations;
    public List<InteractionDialogue.AudioSetting> listAudio;
    public int currentIndex = 0;

    //���� �� ���ϴ� �Ұ�(InteractionDialogue�� �ٲ���)
    public bool isSpinner = false;
    public bool isTalking = false;

    //�̺�Ʈ
    public Action onDialogueEndedObject;
    public Action onDialogueEndedData;

    void Update()
    {
        if (isTalking)
        {
            InputFunc();
        }
    }

    //��ȭâ�� ���� �Լ�
    public void ShowDialogue(List<Sprite> sprites, List<InteractionDialogue.SpriteAnimation> animations, List<InteractionDialogue.AudioSetting> audios)
    {
        if (sprites != null && sprites.Count > 0 && !isTalking)
        {
            //�˾� �� �÷��̾� �̵� ���� ����
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
            text.text = "�� �ޫ������Ūêƪ���������\r\n ����Ы���ϫ��ë���)";

            ShowSprite();
            
            theWindow.OpenWindow(dialogueWindow);
        }
    }
    // ��ȭâ�� �ݴ� �Լ�
    public void CloseDialogue()
    {
        Debug.Log("dial������");
        if (sprite != null)
        {
            //�÷��̾� �̵� ���� ����
            thePlayer.canMove = true;
            isTalking = false;

            dialogueWindow.gameObject.SetActive(false);
            dialObj.gameObject.SetActive(false);
            questObj.gameObject.SetActive(false);

            onDialogueEndedObject?.Invoke();
            onDialogueEndedData?.Invoke();

            currentIndex = 0;
            text.text = "";
            //theWindow ��ũ��Ʈ���� ȭ�� �ʱ�ȭ
            theWindow.CloseWindow();
        }
    }

    // ���� ��������Ʈ�� ǥ���ϴ� �Լ�
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

    //���� �ε����� �ش��ϴ� ��������Ʈ�� ��ȭâ�� ���
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
    /// Dialogue�� ��µǴ� Sprite�� ����
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
    /// Dialogue ������ �°� Sprite ���
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
    /// Dialogue ������ �°� ���� ���
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
            //closeBtn ������Ʈ�� Ȱ��ȭ �� ���¿����� Enter�� esc�� ��ȭ ����.
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (btnClose.activeInHierarchy)
                {
                    CloseDialogue();
                }
            }
            //closeBtn ������Ʈ�� ��Ȱ��ȭ �� ���¿����� SpaceŰ�� �ѱ�� �͸� �޾Ƶ���
            else if (Input.GetKeyDown(KeyCode.Space) && btnNext.activeInHierarchy)
            {
                NextSprite();
            }
        }
    }
}
