using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    // ��ũ��Ʈ ĳ��
    [Header("Caching Script")]
    [Tooltip("TimeSet �ڵ尡 �� ������Ʈ")]
    public TimeSet timeSet;

    // ������Ʈ ĳ��
    [Header("Caching Text")]
    [Tooltip("Ÿ�̸Ӹ� ����� �ؽ�Ʈ")]
    public Text timerText;

    [Tooltip("���Ͷ��� ����� �ؽ�Ʈ")]
    public Text centerLabel;

    private TimerInfo TimerInfo;
    private bool isInfinityTimeActive = false; // InfinityTime ���� ���� �÷���
    private bool isEscPressed = false;        // Esc Ű�� ���ȴ��� Ȯ��

    void Awake()
    {
        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        TimerInfo = timeSet.timerInfos.Find(info => info.sceneName == scene.name);

        if (TimerInfo == null)
        {
            Debug.LogError($"���� ��({scene.name})�� �ش��ϴ� TimerInfo�� �����ϴ�.");
            return;
        }

        // Ÿ�̸� ����
        StopAllCoroutines(); // ���� Ÿ�̸� �ߴ�
        StartCoroutine(StartTimer());
    }

    void Update()
    {
        // Esc Ű �Է� ����
        if (isInfinityTimeActive && Input.GetKeyDown(KeyCode.Escape))
        {
            isEscPressed = true;
        }
    }

    private IEnumerator StartTimer()
    {
        var textData = TimerInfo.textData;

        // 1. �غ� �ؽ�Ʈ ����
        float prepareTotalTime = CalculateTotalTime(textData.prepareText);
        yield return StartCoroutine(RunTextCountdown(prepareTotalTime, textData.prepareText));

        // 2. �� ���� Ÿ�̸� ����
        var timerData = TimerInfo.timerData;
        if (timerData.infinityTime)
        {
            isInfinityTimeActive = true; // InfinityTime Ȱ��ȭ
            yield return StartCoroutine(RunInfinityTimer()); // InfinityTimer ����
        }
        else
        {
            int playTimeInSeconds = timerData.playTime * 60; // �� -> �� ��ȯ
            yield return StartCoroutine(RunGameCountdown(playTimeInSeconds));
        }

        // 3. ���� �ؽ�Ʈ ����
        float endTotalTime = CalculateTotalTime(textData.endText);
        yield return StartCoroutine(RunTextCountdown(endTotalTime, textData.endText));

        // 4. �� ��ȯ
        LoadNextScene();
    }

    private IEnumerator RunInfinityTimer()
    {
        // InfinityTime�� �� Ÿ�̸� ����
        if (timerText != null)
        {
            timerText.text = "99:99";
        }

        // Esc Ű�� ���� ������ ���
        while (!isEscPressed)
        {
            yield return null; // �� ������ ���
        }

        // Esc Ű�� ������ InfinityTime ����
        isInfinityTimeActive = false;
        isEscPressed = false;
        Debug.Log("InfinityTime ����: Esc Ű�� ���Ƚ��ϴ�.");
    }

    private IEnumerator RunTextCountdown(float totalTime, System.Collections.Generic.List<string> textList)
    {
        float remainingTime = totalTime;

        foreach (var entry in textList)
        {
            string[] parts = entry.Split('/');
            string message = parts[0].Trim();
            float displayTime = parts.Length > 1 && float.TryParse(parts[1], out float time) ? time : 2f;

            centerLabel.text = message;

            while (displayTime > 0)
            {
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

        centerLabel.text = "";
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private IEnumerator RunGameCountdown(int totalSeconds)
    {
        int remainingTime = totalSeconds;
        bool oneMinuteMessageShown = false;

        while (remainingTime > 0)
        {
            if (timerText != null)
            {
                int minutes = remainingTime / 60;
                int seconds = remainingTime % 60;
                timerText.text = $"{minutes:00}:{seconds:00}";
            }

            // 1�� ������ �� �޽��� ���
            if (remainingTime == 60 && !oneMinuteMessageShown)
            {
                oneMinuteMessageShown = true;
                StartCoroutine(ShowOneMinuteLeftMessage());
            }

            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        if (timerText != null)
        {
            timerText.text = "00:00";
        }
    }

    private IEnumerator ShowOneMinuteLeftMessage()
    {
        centerLabel.text = "1�� ���ҽ��ϴ�.";
        yield return new WaitForSeconds(2f);
        centerLabel.text = ""; // �޽��� �ʱ�ȭ
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
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("�� �̻� �ε��� ���� �����ϴ�.");
        }
    }
}