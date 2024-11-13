
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopUpManager : MonoBehaviour
{
    //��ũ��Ʈ ĳ�̹ޱ�
    public WindowManager theWindow;

    //���ӿ�����Ʈ ĳ�̹ޱ�
    public GameObject itemPopUpWindow;

    public SpriteRenderer sprite;

    public GameObject btnClose;
    public GameObject btnLeft;
    public GameObject btnRight;

    //���� ��������Ʈ ����Ʈ�� �ε���
    public List<Sprite> listSprites;
    public int currentIndex = 0;

    //���� �� ���ϴ� �Ұ�(InteractionDialogue�� �ٲ���)
    public bool isTalking = false;

    void Update()
    {
        if (isTalking)
        {
            InputFunc();
        }
    }

    //��ȭâ�� ���� �Լ�
    public void ShowItem(List<Sprite> sprites)
    {
        if (sprites != null && sprites.Count > 0 && !isTalking)
        {
            isTalking = true;

            itemPopUpWindow.gameObject.SetActive(true);

            listSprites = sprites;
            currentIndex = 0;

            ShowSprite();

            theWindow.OpenWindow(itemPopUpWindow);
        }
    }
    // ��ȭâ�� �ݴ� �Լ�
    public void CloseItem()
    {
        if (sprite != null)
        {
            isTalking = false;

            itemPopUpWindow.gameObject.SetActive(false);
            btnLeft.SetActive(false);
            btnRight.SetActive(false);

            currentIndex = 0;

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
            currentIndex = listSprites.Count - 1; ;
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
        }
    }

    private void UpdateButtons()
    {
        if (listSprites.Count != 1)
        {
            btnLeft.SetActive(true);
            btnRight.SetActive(true);
        }
        else
        {
            btnLeft.SetActive(false);
            btnRight.SetActive(false);
        }
    }


    public void InputFunc()
    {
        if (listSprites.Count != 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                BackSprite();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                NextSprite();
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CloseItem();
        }
    }
}
