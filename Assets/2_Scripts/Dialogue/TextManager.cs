using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    //������Ʈ ĳ��
    public GameObject textWindow;
    public Text textComponent;

    //��ũ��Ʈ ĳ��
    public DialManager theDM;
    public QuestManager theQM;

    private void Awake()
    {
        theDM = FindFirstObjectByType<DialManager>();
        theQM = FindFirstObjectByType<QuestManager>();
        CloseText();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CloseText();
    }

    //��ȭâ�� ���� �Լ�
    public void ShowText(string textbox)
    {
        if (textbox != null && !theDM.isTalking && !theQM.isTalking)
        {
            textComponent.text = textbox;
            textWindow.SetActive(true);
        }
    }

    // ��ȭâ�� �ݴ� �Լ�
    public void CloseText()
    {
        textComponent.text = "";
        textWindow.SetActive(false);
    }
}
