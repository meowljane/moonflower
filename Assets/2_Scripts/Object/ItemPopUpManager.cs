
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopUpManager : MonoBehaviour
{
    //스크립트 캐싱받기
    public WindowManager theWindow;

    //게임오브젝트 캐싱받기
    public GameObject itemPopUpWindow;

    public SpriteRenderer sprite;

    public GameObject btnClose;
    public GameObject btnLeft;
    public GameObject btnRight;

    //현재 스프라이트 리스트와 인덱스
    public List<Sprite> listSprites;
    public int currentIndex = 0;

    //게임 중 변하는 불값(InteractionDialogue가 바꿔줌)
    public bool isTalking = false;

    void Update()
    {
        if (isTalking)
        {
            InputFunc();
        }
    }

    //대화창을 여는 함수
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
    // 대화창을 닫는 함수
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
            currentIndex = listSprites.Count - 1; ;
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
