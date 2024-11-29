using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameProgress : MonoBehaviour
{
    [Header("Caching Script")]
    [Tooltip("GameSet �ڵ尡 �� ������Ʈ")]
    public GameSet gameSet;

    [Header("Caching Text")]
    [Tooltip("Ÿ�̸Ӹ� ����� �ؽ�Ʈ")]
    public Text timerText;

    [Tooltip("���Ͷ��� ����� �ؽ�Ʈ�� ���")]
    public Text centerLabel;
    public GameObject centerLabelPannel;

    [Tooltip("�� �÷��� Ÿ���� ǥ���� �ؽ�Ʈ")]
    public Text playTimeText;

    [Tooltip("�� �÷��� Ÿ���� ǥ���� �ؽ�Ʈ")]
    public Text overTimeText;

    [Tooltip("�������� ����� �ҽ� ���")]
    public AudioSource audioSource;

    private GameInfos gameInfos;
    private bool isSkipPlay = false;

    private float totalPlayTime = 0f; // �� �÷��� Ÿ�� (�� ����)

    void Awake()
    {
        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!isSkipPlay && Input.GetKeyDown(KeyCode.Escape))
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

    private void PlaySound(AudioClip clip, bool loop = false)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"�� �ε��: {scene.name}");

        // ���� ���� �ش��ϴ� TimerInfo ��������
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

        // 1. �غ� �ؽ�Ʈ ����
        float prepareTotalTime = CalculateTotalTime(textData.prepareText);
        PlaySound(gameInfos.soundData.startSound);
        yield return StartCoroutine(RunTextCountdown(prepareTotalTime, textData.prepareText));

        // 2. �� ���� Ÿ�̸� ����
        var timerData = gameInfos.timerData;
        PlaySound(gameInfos.soundData.BGM);
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

        // 3. ���� �ؽ�Ʈ ����
        float endTotalTime = CalculateTotalTime(textData.endText);
        PlaySound(gameInfos.soundData.endSound);
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

        //while (!isSkipPlay)
        //{
        //    if (timerText != null)
        //    {
        //        int minutes = remainingTime / 60;
        //        int seconds = remainingTime % 60;
        //        timerText.text = $"{minutes:00}:{seconds:00}";
        //    }

        //    // �� �ʸ��� �� �÷��� Ÿ�� ����
        //    totalPlayTime += 1f;

        //    // �� �÷��� Ÿ�� ������Ʈ
        //    SetPlayTime();

        //    yield return new WaitForSeconds(1f);
        //    remainingTime--;
        //}

        if (timerText != null)
        {
            timerText.text = "00:00";
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

            centerLabel.text = message;
            centerLabelPannel.SetActive(true);

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
        centerLabel.text = "";
        centerLabelPannel.SetActive(false);
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
