using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class GameProgress : MonoBehaviour
{
    public GameObject button;
    private WindowManager theWM;

    [Header("Caching Script")]
    [Tooltip("GameSet �ڵ尡 �� ������Ʈ")]
    public GameSet gameSet;

    [Tooltip("Database �ڵ尡 �� ������Ʈ")]
    public DatabaseManager databaseManager;    
    
    [Tooltip("Database �ڵ尡 �� ������Ʈ")]
    public TextManager textManager;
    
    [Tooltip("AudioManager �ڵ尡 �� ������Ʈ")]
    public AudioManager audioManager;

    [Header("Caching Text")]
    [Tooltip("Ÿ�̸Ӹ� ����� �ؽ�Ʈ")]
    public Text timerText;

    [Tooltip("�� �÷��� Ÿ���� ǥ���� �ؽ�Ʈ")]
    public Text playTimeText;

    private GameInfos gameInfos;
    private int nextSceneIndex;

    public bool isF6 = false;
    private bool isSkipPlay = false;

    private float totalPlayTime = 0f; // �� �÷��� Ÿ�� (�� ����)
    private float overPlayTime = 0f; // �� �÷��� Ÿ�� (�� ����)

    void Awake()
    {
        theWM = FindFirstObjectByType<WindowManager>();
        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (!isSkipPlay && Input.GetKeyDown(KeyCode.N))
        {
            isSkipPlay = !isSkipPlay;
        }
    }

    public void SkipPlayCoroutine()
    {
        isSkipPlay = true;
    }

    private void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"�� �ε��: {scene.name}");

        // ���� ���� �ش��ϴ� TimerInfo ��������
        if (isF6)
        {
            return;
        }

        gameInfos = gameSet.gameProgress.Find(info => info.sceneName == scene.name);

        if (gameInfos == null)
        {
            Debug.LogError($"���� ��({scene.name})�� �ش��ϴ� GameInfo�� �����ϴ�.");
            return;
        }

        // Ÿ�̸� ����
        StopAllCoroutines(); // ���� Ÿ�̸� �ߴ�
        StartCoroutine(StartTimer());
    }

    private void SetPlayTime()
    {
        // �� �÷��� Ÿ�� ������Ʈ
        if (playTimeText != null)
        {
            int minutes = Mathf.FloorToInt(totalPlayTime / 60);
            int seconds = Mathf.FloorToInt(totalPlayTime % 60);
            playTimeText.text = $"�� �÷��� Ÿ��: {minutes:00}:{seconds:00}";
        }
    }

    private IEnumerator StartTimer()
    {
        var textData = gameInfos.textData;
        var timerData = gameInfos.timerData;
        float endTotalTime = CalculateTotalTime(textData.endText);
        float prepareTotalTime = CalculateTotalTime(textData.prepareText);

        // 1. �غ� �ؽ�Ʈ ����

        audioManager.PlaySound("START");
        button.SetActive(false);
        yield return StartCoroutine(RunTextCountdown(prepareTotalTime, textData.prepareText));

        // 2. �� ���� Ÿ�̸� ����

        audioManager.StopSound("");
        audioManager.PlaySound("BGM");
        button.SetActive(true);
        if (timerData.infinityTime)
        {
            yield return StartCoroutine(RunInfinityTimer()); // InfinityTimer ����
        }
        else
        {
            int playTimeInSeconds = timerData.playTime * 60; // �� -> �� ��ȯ
            yield return StartCoroutine(RunGameCountdown(playTimeInSeconds));
        }

        isSkipPlay = false;
        databaseManager.seconds = (int)overPlayTime;

        // 3. ���� �ؽ�Ʈ ����

        audioManager.StopSound("BGM");
        audioManager.PlaySound("END");
        button.SetActive(false);
        theWM.DeactivateTotalWindows();
        yield return StartCoroutine(RunTextCountdown(endTotalTime, textData.endText));

        // 4. �� ��ȯ
        button.SetActive(true);
        LoadNextScene();
    }

    private IEnumerator RunInfinityTimer()
    {
        // InfinityTime�� �� Ÿ�̸� ����
        if (timerText != null)
        {
            timerText.text = "99:99";
        }

        while (!isSkipPlay)
        {
            // �� �ʸ��� �� �÷��� Ÿ�� ����
            totalPlayTime += Time.deltaTime;

            // �� �÷��� Ÿ�� ������Ʈ
            SetPlayTime();

            yield return null;
        }

        Debug.Log("InfinityTime ����: Esc Ű�� ���Ƚ��ϴ�.");
    }

    private IEnumerator RunGameCountdown(int totalSeconds)
    {
        int remainingTime = totalSeconds;

        while (remainingTime > 0 && !isSkipPlay)
        {
            if (timerText != null)
            {
                int minutes = remainingTime / 60;
                int seconds = remainingTime % 60;
                timerText.text = $"{minutes:00}:{seconds:00}";
            }

            // �� �ʸ��� �� �÷��� Ÿ�� ����
            totalPlayTime += 1f;

            // �� �÷��� Ÿ�� ������Ʈ
            SetPlayTime();

            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        if (timerText != null)
        {
            timerText.text = "00:00";
        }

        while (!isSkipPlay && gameInfos.timerData.overTime)
        {
            // �� �ʸ��� �� �÷��� Ÿ�� ����
            totalPlayTime += 1f;
            overPlayTime += 1f;

            // �� �÷��� Ÿ�� ������Ʈ
            SetPlayTime();

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator RunTextCountdown(float totalTime, System.Collections.Generic.List<string> textList)
    {
        float remainingTime = totalTime;

        foreach (var entry in textList)
        {
            string[] parts = entry.Split('/');
            string message = parts[0].Trim();

            // \n ó��
            message = message.Replace("\\n", "\n");

            float displayTime = parts.Length > 1 && float.TryParse(parts[1], out float time) ? time : 2f;

            textManager.ShowText(message);

            while (displayTime > 0)
            {
                // ���� �ð� ������Ʈ
                if (timerText != null)
                {
                    int minutes = Mathf.FloorToInt(remainingTime / 60);
                    int seconds = Mathf.FloorToInt(remainingTime % 60);
                    timerText.text = $"{minutes:00}:{seconds:00}";
                }

                yield return new WaitForSeconds(1f);
                displayTime--;
                remainingTime--;
            }
        }

        // �ؽ�Ʈ �ʱ�ȭ
        textManager.CloseText();

        if (timerText != null)
        {
            timerText.text = "00:00"; // �ؽ�Ʈ �Ϸ� �� Ÿ�̸� 00:00���� �ʱ�ȭ
        }
    }


    private float CalculateTotalTime(System.Collections.Generic.List<string> textList)
    {
        float totalTime = 0f;
        foreach (var entry in textList)
        {
            string[] parts = entry.Split('/');
            if (parts.Length > 1 && float.TryParse(parts[1], out float time))
            {
                totalTime += time;
            }
        }
        return totalTime;
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("�� �̻� �ε��� ���� �����ϴ�.");
        }
    }

    public int CheckSceneNum()
    {
        return nextSceneIndex;
    }
}
