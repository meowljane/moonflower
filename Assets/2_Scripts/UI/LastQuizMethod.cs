using System;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LastQuizMethod : MonoBehaviour
{
    //��ũ��Ʈ ĳ�̹ޱ�
    public WindowManager theWindow;
    public DatabaseManager theDM;
    public TextManager theTM;
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
    public Text playerName;
    public TextMeshProUGUI gradePageText;

    private string gradeText = "�����մϴ� ���ٴ�. \r\n����� ��Ž���Դϴ�!\r\n\r\n���ٴ��� ������\r\n\r\n100�� S��ũ\r\n\r\n���� ���� 20�� �߰� ��ǥ 20��\r\n��Ʈ ���� -20�� �ð� ���� -20��\r\n�� ���� 100��\r\n";
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
                    ActiveText("������ ��� �Է����ּ���.");
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

        //dm�κ� ��� �Ƚᵵ �Ǵµ�.. �ϴ� �׳� ��.
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
                ActiveText("ä������ ���� ���� �ֽ��ϴ�.");
                return;
            }
            index++;

            continue;
        }

        Debug.Log("��������.");
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
�����մϴ�, {playerNameText}��!
����� ��Ž���Դϴ�!

{playerNameText}���� ������:
�� ����: {totalScore}�� ��ũ{rank}

���� ����: {quizPoints}��, �߰� ��ǥ ����: {votePoints}��
��Ʈ ����: {hintPenalty}��, �ð� ����: -{timePenalty}��";

        gradePageText.text = gradeText;
    }


    void ChooseBtn(bool isO, int index)
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
