using System;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LastQuizMethod : MonoBehaviour
{
    //스크립트 캐싱받기
    public WindowManager theWindow;
    public DatabaseManager theDM;
    public TextManager theTM;
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
    public Text playerName;
    public TextMeshProUGUI gradePageText;

    private string gradeText = "축하합니다 레다님. \r\n당신은 명탐정입니다!\r\n\r\n레다님의 점수는\r\n\r\n100점 S랭크\r\n\r\n문제 점수 20점 중간 투표 20점\r\n힌트 차감 -20점 시간 차잠 -20점\r\n총 점수 100점\r\n";
    private bool isActive = false;

    public void Awake()
    {
        theWindow = FindFirstObjectByType<WindowManager>();
        theDM = FindObjectOfType<DatabaseManager>();
        theTM = FindFirstObjectByType<TextManager>();
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
                    ActiveText("정답을 모두 입력해주세요.");
                    return; 
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

        //dm부분 사실 안써도 되는데.. 일단 그냥 둠.
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
                ActiveText("채점하지 않은 곳이 있습니다.");
                return;
            }
            index++;

            continue;
        }

        Debug.Log("최종으로.");
        SetGradePage();
        nextDial.SetActive(true);
        quizWindow.SetActive(false);
    }

    void SetGradePage()
    {
        int[] scores = { 10, 10, 10, 15, 15, 15, 25, 10, 10, 10 };
        int totalScore = 0;
        int quizPoints = 0;
        int votePoints = theDM.isCorrectVote ? 10 : 0;
        int hintPenalty = theDM.isCheckedHint ? 10 : 0;
        int timePenalty = (theDM.seconds * (theDM.isHard ? 1 : 0) / 60) * 2;

        string playerNameText = playerName.text;

        for (int i = 0; i < rightQuiz.Length; i++)
        {
            if (rightQuiz[i] == "true")
            {
                quizPoints += scores[i];
            }
        }

        totalScore = quizPoints + votePoints - hintPenalty - timePenalty;
        totalScore = Mathf.Clamp(totalScore, 0, 100);

        string rank;
        if (totalScore >= 90) rank = "S";
        else if (totalScore >= 80) rank = "A";
        else if (totalScore >= 70) rank = "B";
        else if (totalScore >= 60) rank = "C";
        else rank = "D";

        gradeText = $@"
축하합니다, {playerNameText}님!
당신은 명탐정입니다!

{playerNameText}님의 점수는:
총 점수: {totalScore}점 랭크{rank}

문제 점수: {quizPoints}점, 중간 투표 점수: {votePoints}점
힌트 차감: {hintPenalty}점, 시간 차감: -{timePenalty}점";

        gradePageText.text = gradeText;
    }


    void ChooseBtn(bool isO, int index)
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

    void ActiveText(string sentences)
    {
        theTM.ShowText(sentences);
        isActive = true;
        Invoke("CloseTMText", 2);
    }
    void CloseTMText()
    {
        theTM.CloseText();
        isActive = false;
    }
}
