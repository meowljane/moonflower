using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameProgress : MonoBehaviour
{
    [Header("Caching Script")]
    [Tooltip("GameSet 코드가 들어간 오브젝트")]
    public GameSet gameSet;

    [Header("Caching Text")]
    [Tooltip("타이머를 담당할 텍스트")]
    public Text timerText;

    [Tooltip("센터라벨을 담당할 텍스트와 배경")]
    public Text centerLabel;
    public GameObject centerLabelPannel;

    [Tooltip("총 플레이 타임을 표시할 텍스트")]
    public Text playTimeText;

    [Tooltip("총 플레이 타임을 표시할 텍스트")]
    public Text overTimeText;

    [Tooltip("전반적인 오디오 소스 출력")]
    public AudioSource audioSource;

    private GameInfos gameInfos;
    private bool isSkipPlay = false;

    private float totalPlayTime = 0f; // 총 플레이 타임 (초 단위)

    void Awake()
    {
        // 씬 로드 이벤트 등록
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
        // 씬 로드 이벤트 해제
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
        Debug.Log($"씬 로드됨: {scene.name}");

        // 현재 씬에 해당하는 TimerInfo 가져오기
        gameInfos = gameSet.gameProgress.Find(info => info.sceneName == scene.name);

        if (gameInfos == null)
        {
            Debug.LogError($"현재 씬({scene.name})에 해당하는 GameInfo가 없습니다.");
            return;
        }

        // 타이머 시작
        StopAllCoroutines(); // 이전 타이머 중단
        StartCoroutine(StartTimer());
    }

    private void SetPlayTime()
    {
        // 총 플레이 타임 업데이트
        if (playTimeText != null)
        {
            int minutes = Mathf.FloorToInt(totalPlayTime / 60);
            int seconds = Mathf.FloorToInt(totalPlayTime % 60);
            playTimeText.text = $"총 플레이 타임: {minutes:00}:{seconds:00}";
        }
    }

    private IEnumerator StartTimer()
    {
        var textData = gameInfos.textData;

        // 1. 준비 텍스트 실행
        float prepareTotalTime = CalculateTotalTime(textData.prepareText);
        PlaySound(gameInfos.soundData.startSound);
        yield return StartCoroutine(RunTextCountdown(prepareTotalTime, textData.prepareText));

        // 2. 본 게임 타이머 실행
        var timerData = gameInfos.timerData;
        PlaySound(gameInfos.soundData.BGM);
        if (timerData.infinityTime)
        {
            yield return StartCoroutine(RunInfinityTimer()); // InfinityTimer 실행
        }
        else
        {
            int playTimeInSeconds = timerData.playTime * 60; // 분 -> 초 변환
            yield return StartCoroutine(RunGameCountdown(playTimeInSeconds));
        }

        isSkipPlay = false;

        // 3. 종료 텍스트 실행
        float endTotalTime = CalculateTotalTime(textData.endText);
        PlaySound(gameInfos.soundData.endSound);
        yield return StartCoroutine(RunTextCountdown(endTotalTime, textData.endText));

        // 4. 씬 전환
        LoadNextScene();
    }

    private IEnumerator RunInfinityTimer()
    {
        // InfinityTime일 때 타이머 고정
        if (timerText != null)
        {
            timerText.text = "99:99";
        }

        while (!isSkipPlay)
        {
            // 매 초마다 총 플레이 타임 증가
            totalPlayTime += Time.deltaTime;

            // 총 플레이 타임 업데이트
            SetPlayTime();

            yield return null;
        }

        Debug.Log("InfinityTime 종료: Esc 키가 눌렸습니다.");
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

            // 매 초마다 총 플레이 타임 증가
            totalPlayTime += 1f;

            // 총 플레이 타임 업데이트
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

        //    // 매 초마다 총 플레이 타임 증가
        //    totalPlayTime += 1f;

        //    // 총 플레이 타임 업데이트
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

            // \n 처리
            message = message.Replace("\\n", "\n");

            float displayTime = parts.Length > 1 && float.TryParse(parts[1], out float time) ? time : 2f;

            centerLabel.text = message;
            centerLabelPannel.SetActive(true);

            while (displayTime > 0)
            {
                // 남은 시간 업데이트
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

        // 텍스트 초기화
        centerLabel.text = "";
        centerLabelPannel.SetActive(false);
        if (timerText != null)
        {
            timerText.text = "00:00"; // 텍스트 완료 후 타이머 00:00으로 초기화
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
            Debug.Log("더 이상 로드할 씬이 없습니다.");
        }
    }
}
