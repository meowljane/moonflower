using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebGLBtn : MonoBehaviour
{
    public bool isClick;

    private Image Image;
    public Sprite ButtonDefaultImage;
    private void Awake()
    {
        Image = GetComponent<Image>();
    }
    private void OnEnable()
    {
        Image.sprite = ButtonDefaultImage;
    }

    public void PressBtnF()
    {
        isClick = true;
    }
}
