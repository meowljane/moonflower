using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    //오브젝트 캐싱
    public GameObject textWindow;
    public Text textComponent;

    //스크립트 캐싱
    public DialManager theDM;

    private void Awake()
    {
        theDM = FindFirstObjectByType<DialManager>();
        CloseText();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //CloseText();
    }

    //대화창을 여는 함수
    public void ShowText(string textbox)
    {
        if (textbox != null && !theDM.isTalking)
        {
            textComponent.text = textbox;
            textWindow.SetActive(true);
        }
    }

    // 대화창을 닫는 함수
    public void CloseText()
    {
        textComponent.text = "";
        textWindow.SetActive(false);
    }
}
