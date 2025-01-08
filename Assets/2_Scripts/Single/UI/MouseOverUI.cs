using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage; // 변경할 실제 Image 컴포넌트
    public Sprite title_btnImage; // 기본 이미지
    public Sprite title_btnImage_Over; // 마우스 오버 시 이미지

    void OnEnable()
    {
        targetImage.sprite = title_btnImage;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetImage.sprite = title_btnImage_Over; // 마우스 오버 시 이미지로 변경
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetImage.sprite = title_btnImage; // 기본 이미지로 되돌림
    }
}
