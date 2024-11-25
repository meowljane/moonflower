using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class LastQuizMethod : MonoBehaviour
{
    //스크립트 캐싱받기
    public WindowManager theWindow;
    private DatabaseManager theDM;
    public GameObject quizWindow;
    public GameObject nextDial;

    public RectTransform rectTransform;
    public Scrollbar verticalScrollbar;

    public Button[] OButtons;   // O 버튼들
    public Button[] XButtons;   // X 버튼들
    public string[] rightQuiz = new string[10]; // 정답 상태 배열

    public Sprite redO;  // 붉은 O 이미지
    public Sprite whiteO; // 흰색 O 이미지
    public Sprite redX;  // 붉은 X 이미지
    public Sprite whiteX; // 흰색 X 이미지


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
                    Debug.Log("값이 없습니다.");
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
        Debug.Log("다했네");
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

        // "ButtonO"와 "ButtonX" 태그를 가진 버튼들을 찾아 배열에 저장
        OButtons = ConvertToButtons(GameObject.FindGameObjectsWithTag("ButtonO"));
        XButtons = ConvertToButtons(GameObject.FindGameObjectsWithTag("ButtonX"));

        // O버튼에 클릭 이벤트 추가
        for (int i = 0; i < OButtons.Length; i++)
        {
            int index = i;
            OButtons[i].onClick.AddListener(() => ChooseBtn(true, index));
            Debug.Log(i);
        }

        // X버튼에 클릭 이벤트 추가
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
                Debug.Log("값이 없습니다.");
                return;
            }
            index++;

            continue;
        }

        Debug.Log("최종으로.");
        nextDial.SetActive(true);
        quizWindow.SetActive(false);
    }
    public void ChooseBtn(bool isO, int index)
    {
        // O 버튼을 눌렀을 경우
        if (isO)
        {
            if (rightQuiz[index] == "true") // O 버튼이 눌린 경우
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
        else // X 버튼을 눌렀을 경우
        {
            if (rightQuiz[index] == "false") // X 버튼이 눌린 경우
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
