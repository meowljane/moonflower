using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage; // ������ ���� Image ������Ʈ
    public Sprite title_btnImage; // �⺻ �̹���
    public Sprite title_btnImage_Over; // ���콺 ���� �� �̹���

    void OnEnable()
    {
        targetImage.sprite = title_btnImage;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetImage.sprite = title_btnImage_Over; // ���콺 ���� �� �̹����� ����
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetImage.sprite = title_btnImage; // �⺻ �̹����� �ǵ���
    }
}
