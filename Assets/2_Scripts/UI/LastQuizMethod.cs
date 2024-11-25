using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class LastQuizMethod : MonoBehaviour
{
    //��ũ��Ʈ ĳ�̹ޱ�
    public WindowManager theWindow;
    private DatabaseManager theDM;
    public GameObject quizWindow;
    public GameObject nextDial;

    public RectTransform rectTransform;
    public Scrollbar verticalScrollbar;

    public Button[] OButtons;   // O ��ư��
    public Button[] XButtons;   // X ��ư��
    public string[] rightQuiz = new string[10]; // ���� ���� �迭

    public Sprite redO;  // ���� O �̹���
    public Sprite whiteO; // ��� O �̹���
    public Sprite redX;  // ���� X �̹���
    public Sprite whiteX; // ��� X �̹���


    public Button answerButton;
    public Sprite endingBtnImage;

    public void Awake()
    {
        theWindow = FindFirstObjectByType<WindowManager>();
        theDM = FindObjectOfType<DatabaseManager>();
        theWindow.OpenWindow(quizWindow);
        answerButton.onClick.AddListener(() => ShowAnswerBtn());
    }
    private Button[] ConvertToButtons(GameObject[] gameObjects)
    {
        Button[] buttons = new Button[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; i++)
        {
            buttons[i] = gameObjects[i].GetComponent<Button>();
        }
        return buttons;
    }
    public void ShowAnswerBtn()
    {
        int index = 0;

        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {

            var inputField = child.GetComponent<InputField>();
            if (inputField != null)
            {
                if (string.IsNullOrEmpty(inputField.text))
                {
                    Debug.Log("���� �����ϴ�.");
                    //return; 
                }
                index++;

                continue;
            }
        }

        ShowAnswer();
    }

    public void ShowAnswer()
    {
        Debug.Log("���߳�");
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            var inputField = child.GetComponent<InputField>();
            if (inputField != null)
            {
                inputField.interactable = false;
            }
        }
        foreach (Transform child in rectTransform)
        {
            if (child.CompareTag("AnswerPannel"))
            {
                child.gameObject.SetActive(true);
            }
        }
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 2900f);

        // "ButtonO"�� "ButtonX" �±׸� ���� ��ư���� ã�� �迭�� ����
        OButtons = ConvertToButtons(GameObject.FindGameObjectsWithTag("ButtonO"));
        XButtons = ConvertToButtons(GameObject.FindGameObjectsWithTag("ButtonX"));

        // O��ư�� Ŭ�� �̺�Ʈ �߰�
        for (int i = 0; i < OButtons.Length; i++)
        {
            int index = i;
            OButtons[i].onClick.AddListener(() => ChooseBtn(true, index));
            Debug.Log(i);
        }

        // X��ư�� Ŭ�� �̺�Ʈ �߰�
        for (int i = 0; i < XButtons.Length; i++)
        {
            int index = i;
            XButtons[i].GetComponent<Button>().onClick.AddListener(() => ChooseBtn(false, index));
        }
        answerButton.GetComponent<RectTransform>().sizeDelta = new Vector2(101f, 50f);
        answerButton.image.sprite = endingBtnImage;
        answerButton.onClick.RemoveAllListeners();
        answerButton.onClick.AddListener(() => CheckAnswerBtn());
        if (verticalScrollbar != null)
        {
            verticalScrollbar.value = 1f;
        }
    }
    public void CheckAnswerBtn()
    {
        int index = 0;

        foreach (string child in rightQuiz)
        {
            if (child == "true")
            {
                theDM.quizCorrect[index] = true;
            }
            else
            {
                theDM.quizCorrect[index] = false;
            }
            if (string.IsNullOrEmpty(child))
            {
                Debug.Log("���� �����ϴ�.");
                return;
            }
            index++;

            continue;
        }

        Debug.Log("��������.");
        nextDial.SetActive(true);
        quizWindow.SetActive(false);
    }
    public void ChooseBtn(bool isO, int index)
    {
        // O ��ư�� ������ ���
        if (isO)
        {
            if (rightQuiz[index] == "true") // O ��ư�� ���� ���
            {
                OButtons[index].image.sprite = whiteO;
                rightQuiz[index] = null;
            }
            else
            {
                XButtons[index].image.sprite = whiteX;
                OButtons[index].image.sprite = redO;
                rightQuiz[index] = "true";
            }
        }
        else // X ��ư�� ������ ���
        {
            if (rightQuiz[index] == "false") // X ��ư�� ���� ���
            {
                XButtons[index].image.sprite = whiteX;
                rightQuiz[index] = null;
            }
            else
            {
                OButtons[index].image.sprite = whiteO;
                XButtons[index].image.sprite = redX;
                rightQuiz[index] = "false";
            }
        }
    }
}
