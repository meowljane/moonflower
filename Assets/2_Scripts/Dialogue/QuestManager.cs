using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    //��ũ��Ʈ ĳ�̹ޱ�
    public AudioManager theAudio;
    public WindowManager theWindow;
    public PlayerManager thePlayer;

    //���ӿ�����Ʈ ĳ�̹ޱ�
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

    //���� ��������Ʈ ����Ʈ�� �ε���
    public List<Sprite> listSprites;
    public List<InteractionQuest.SpriteAnimation> listSpriteAnimations;
    public List<InteractionQuest.AudioSetting> listAudio;
    public int currentIndex = 0;

    //���� �� ���ϴ� ��(���߿� private ó���ص� ��)
    public bool isTalking = false;
    public int questIndex = 0;
    public string correctAnswer = "feline";

    //�̺�Ʈ
    public Action onQuestEndedObject;
    public Action onQuestEndedData;

    void Update()
    {
        if (isTalking)
        {
            InputFunc();
        }
    }

    //��ȭâ�� ���� �Լ�
    public void ShowQuest(List<Sprite> sprites, List<InteractionQuest.SpriteAnimation> animations, List<InteractionQuest.AudioSetting> audios)
    {
        if (sprites != null && sprites.Count > 0 && !isTalking)
        {
            //�˾� �� �÷��̾� �̵� ���� ����
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

    // ��ȭâ�� �ݴ� �Լ�
    public void CloseQuest()
    {
        if (sprite != null)
        {
            Debug.Log("������");
            //�÷��̾� �̵� ���� ����
            thePlayer.canMove = true;
            isTalking = false;

            currentIndex = 0;
            text.text = "";
            questInput.text = "";

            dialogueWindow.gameObject.SetActive(false);
            questObj.gameObject.SetActive(false);
            dialObj.gameObject.SetActive(false);


            //theWindow ��ũ��Ʈ���� ȭ�� �ʱ�ȭ
            theWindow.CloseWindow();
        }
    }

    //������ �Է��� ��ȭâ�� �ݴ� �Լ�
    public void AnswerQuest()
    {
        if (sprite != null)
        {
            onQuestEndedObject?.Invoke();
            onQuestEndedData?.Invoke();
            Debug.Log("�����̶���Ұ�");
            CloseQuest();
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

    /// <summary>
    /// ����� input ���� ���� ���Ͽ� �������� Ȯ���� �� ���� �̺�Ʈ�� �����ϴ� �޼���
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
            // ���� �Է��ϸ� ���� �������� ����
            if (specialSprite != null)
            {
                specialSprite.SetActive(false);
            }
            NextSprite();
        }
        else
        {
            //����� �Է�â �ʱ�ȭ �� ���� �˾� ���
            questInput.text = "";
            ShowPopupWindowImage();
        }
    }
    /// <summary>
    /// ���� â Ȱ��ȭ
    /// </summary>
    public void ShowPopupWindowImage()
    {
        wrongWindow.gameObject.SetActive(true);
        Invoke("HidePopupWindow", 1f);
    }

    /// <summary>
    /// 1�� �Ŀ� ���� â ��Ȱ��ȭ
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
        //closeBtn ������Ʈ�� Ȱ��ȭ �� ���¿����� Enter�� esc�� ��ȭ ����.
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
