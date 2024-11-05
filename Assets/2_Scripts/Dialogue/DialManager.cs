using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InteractionDialogue;


public class DialManager : MonoBehaviour
{
    //��ũ��Ʈ ĳ�̹ޱ�
    public WindowManager theWindow;
    public PlayerManager thePlayer;

    //���ӿ�����Ʈ ĳ�̹ޱ�
    public GameObject dialogueWindow;
    public SpriteRenderer sprite;
    public Text text;
    public GameObject btnNext;
    public GameObject btnClose;

    // Sprite �̹��� ���ҽ�
    public List<spriteData> spriteDatas;
    [System.Serializable]
    public struct spriteData
    {
        public string name;
        public Sprite sprite;
    }

    //���� ��������Ʈ ����Ʈ�� �ε���
    public List<DialogueData> currentList;
    public int currentIndex = 0;

    //���� �� ���ϴ� �Ұ�(InteractionDialogue�� �ٲ���)
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
    public void ShowDialogue(List<DialogueData> dialogueData)
    {
        if (dialogueData != null && dialogueData.Count > 0 && !isTalking)
        {
            //�˾� �� �÷��̾� �̵� ���� ����
            thePlayer.canMove = false;
            isTalking = true;

            dialogueWindow.gameObject.SetActive(true);

            currentList = dialogueData;
            currentIndex = 0;

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

    //���� �ε����� �ش��ϴ� ��������Ʈ�� ��ȭâ�� ���
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

