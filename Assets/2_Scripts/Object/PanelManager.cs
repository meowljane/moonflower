using System.Collections.Generic;
using UnityEngine;
using System;

public class PanelManager : MonoBehaviour
{
    //��ũ��Ʈ ĳ�̹ޱ�
    public WindowManager theWindow;

    //���ӿ�����Ʈ ĳ�̹ޱ�
    public GameObject PanelWindow;

    public SpriteRenderer sprite;

    public GameObject btnClose;
    public GameObject btnLeft;
    public GameObject btnRight;
    public GameObject btnConfirm;
    public GameObject btnCancel;

    //���� ��������Ʈ ����Ʈ�� �ε���
    public List<Sprite> listSprites;
    public int currentIndex = 0;

    //���� �� ���ϴ� �Ұ�(InteractionDialogue�� �ٲ���)
    public bool isTalking = false;
    public bool isDoor = false;

    //Ȱ��ȭ, ��Ȱ��ȭ�� ������Ʈ��
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    void Update()
    {
        if (isTalking)
        {
            InputFunc();
        }
    }

    //��ȭâ�� ���� �Լ�
    public void ShowPanel(List<Sprite> sprites, bool interactWithDoor)
    {
        if (sprites != null && sprites.Count > 0 && !isTalking)
        {
            isTalking = true;

            PanelWindow.gameObject.SetActive(true);

            listSprites = sprites;
            currentIndex = 0;

            isDoor = interactWithDoor;
            ShowSprite();

            theWindow.OpenWindow(PanelWindow);
        }
    }
    // ��ȭâ�� �ݴ� �Լ�
    public void ClosePanel()
    {
        if (sprite != null)
        {
            isTalking = false;

            PanelWindow.gameObject.SetActive(false);
            btnClose.SetActive(false);
            btnLeft.SetActive(false);
            btnRight.SetActive(false);
            btnConfirm.SetActive(false);
            btnCancel.SetActive(false);

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
        if (isDoor)
        {
            btnConfirm.SetActive(true);
            btnCancel.SetActive(true);
        }
        else
        {
            btnClose.SetActive(true);
            if (listSprites.Count != 1)
            {
                btnLeft.SetActive(true);
                btnRight.SetActive(true);
            }
            ButtonLocation();
        }
    }

    private void ButtonLocation()
    {
        Vector2 spriteSize = sprite.sprite.bounds.size * 0.3f;
        Vector2 parentScale = transform.lossyScale;
        Vector2 adjustedSize = new Vector2(spriteSize.x * parentScale.x, spriteSize.y * parentScale.y);

        Vector3 topRightPosition = transform.position +
            new Vector3(adjustedSize.x / 2 - 20f, adjustedSize.y / 2 - 20f, 0);
        Vector3 rightPosition = transform.position +
            new Vector3(adjustedSize.x / 2 - 20f, 0, 0);
        Vector3 leftPosition = transform.position -
            new Vector3(adjustedSize.x / 2 - 20f, 0, 0);

        btnClose.transform.position = topRightPosition;
        btnRight.transform.position = rightPosition;
        btnLeft.transform.position = leftPosition;
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
            ClosePanel();
        }
    }

    public void UpdateObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}
