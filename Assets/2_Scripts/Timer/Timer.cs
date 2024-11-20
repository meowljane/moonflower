using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [Header("Caching Script")]
    [Tooltip("TimeSet �ڵ尡 �� ������Ʈ")]
    public TimeSet timeSet;

    [Header("Caching Text")]
    [Tooltip("Ÿ�̸Ӹ� ����� �ؽ�Ʈ")]
    public Text timerText;

    [Tooltip("���Ͷ��� ����� �ؽ�Ʈ")]
    public Text centerLabel;

    [Tooltip("�� �÷��� Ÿ���� ǥ���� �ؽ�Ʈ")]
    public Text playTimeText;

    private TimerInfo TimerInfo;
    private bool isInfinityTimeActive = false;
    private bool isEscPressed = false;

    private float totalPlayTime = 0f; // �� �÷��� Ÿ�� (�� ����)

    void Awake()
    {
        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Esc Ű �Է� ����
        if (isInfinityTimeActive && Input.GetKeyDown(KeyCode.Escape))
        {
            isEscPressed = true;
        }

        // �� �÷��� Ÿ�� ������Ʈ
        if (playTimeText != null)
        {
            int minutes = Mathf.FloorToInt(totalPlayTime / 60);
            int seconds = Mathf.FloorToInt(totalPlayTime % 60);
            playTimeText.text = $"�� �÷��� Ÿ��: {minutes:00}:{seconds:00}";
        }
    }

    private void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// �ε� �� ���� ���� ������ ���� ������ Ÿ�̸Ӹ� �����ϴ� �޼���
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
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

    /// <summary>
    /// Ÿ�̸��� �⺻ ���� �޼���
    /// �غ� -> ���� -> ���� -> �� ��ȯ
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Ÿ�̸Ӱ� ������ �� ����Ǵ� �޼���
    /// </summary>
    /// <returns></returns>
    private IEnumerator RunInfinityTimer()
    {
        // InfinityTime�� �� Ÿ�̸� ����
        if (timerText != null)
        {
            timerText.text = "99:99";
        }

        while (!isEscPressed)
        {
            // �� �ʸ��� �� �÷��� Ÿ�� ����
            totalPlayTime += Time.deltaTime;
            yield return null;
        }

        isInfinityTimeActive = false;
        isEscPressed = false;
        Debug.Log("InfinityTime ����: Esc Ű�� ���Ƚ��ϴ�.");
    }

    /// <summary>
    /// Ÿ�̸� ī��Ʈ �ٿ��� ����ϴ� �޼���
    /// </summary>
    /// <param name="totalSeconds"></param>
    /// <returns></returns>
    private IEnumerator RunGameCountdown(int totalSeconds)
    {
        int remainingTime = totalSeconds;

        while (remainingTime > 0)
        {
            if (timerText != null)
            {
                int minutes = remainingTime / 60;
                int seconds = remainingTime % 60;
                timerText.text = $"{minutes:00}:{seconds:00}";
            }

            // �� �ʸ��� �� �÷��� Ÿ�� ����
            totalPlayTime += 1f;

            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        if (timerText != null)
        {
            timerText.text = "00:00";
        }
    }

    /// <summary>
    /// �غ� �ܰ� �Ǵ� ������ �ܰ迡�� �ؽ�Ʈ�� ������ִ� �޼���
    /// </summary>
    /// <param name="totalTime"></param>
    /// <param name="textList"></param>
    /// <returns></returns>
    private IEnumerator RunTextCountdown(float totalTime, System.Collections.Generic.List<string> textList)
    {
        float remainingTime = totalTime;

        foreach (var entry in textList)
        {
            string[] parts = entry.Split('/');
            string message = parts[0].Trim();

            // \n ���� �� �ؽ�Ʈ ó��
            message = message.Replace("\\n", "\n"); // \n�� Unity���� �ν��� �� �ִ� �ٹٲ����� ��ȯ

            float displayTime = parts.Length > 1 && float.TryParse(parts[1], out float time) ? time : 2f;

            centerLabel.text = message; // �ٹٲ��� ����� �ؽ�Ʈ ����

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

    /// <summary>
    /// �غ� �Ǵ� ������ �ܰ迡�� ���� �Ǿ���ϴ� �ð��� ���ϴ� �޼���
    /// </summary>
    /// <param name="textList"></param>
    /// <returns></returns>
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

    /// <summary>
    /// �� ��ȯ�� ����ϴ� �޼���
    /// </summary>
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
