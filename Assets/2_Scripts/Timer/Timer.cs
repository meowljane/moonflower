using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [Header("Caching Script")]
    [Tooltip("TimeSet 코드가 들어간 오브젝트")]
    public TimeSet timeSet;

    [Header("Caching Text")]
    [Tooltip("타이머를 담당할 텍스트")]
    public Text timerText;

    [Tooltip("센터라벨을 담당할 텍스트")]
    public Text centerLabel;

    [Tooltip("총 플레이 타임을 표시할 텍스트")]
    public Text playTimeText;

    private TimerInfo TimerInfo;
    private bool isInfinityTimeActive = false;
    private bool isEscPressed = false;

    private float totalPlayTime = 0f; // 총 플레이 타임 (초 단위)

    void Awake()
    {
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Esc 키 입력 감지
        if (isInfinityTimeActive && Input.GetKeyDown(KeyCode.Escape))
        {
            isEscPressed = true;
        }

        // 총 플레이 타임 업데이트
        if (playTimeText != null)
        {
            int minutes = Mathf.FloorToInt(totalPlayTime / 60);
            int seconds = Mathf.FloorToInt(totalPlayTime % 60);
            playTimeText.text = $"총 플레이 타임: {minutes:00}:{seconds:00}";
        }
    }

    private void OnDestroy()
    {
        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 로드 된 씬에 따라서 가져온 정보 값으로 타이머를 구동하는 메서드
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"씬 로드됨: {scene.name}");

        // 현재 씬에 해당하는 TimerInfo 가져오기
        TimerInfo = timeSet.timerInfos.Find(info => info.sceneName == scene.name);

        if (TimerInfo == null)
        {
            Debug.LogError($"현재 씬({scene.name})에 해당하는 TimerInfo가 없습니다.");
            return;
        }

        // 타이머 시작
        StopAllCoroutines(); // 이전 타이머 중단
        StartCoroutine(StartTimer());
    }

    /// <summary>
    /// 타이머의 기본 로직 메서드
    /// 준비 -> 시작 -> 종료 -> 씬 전환
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartTimer()
    {
        var textData = TimerInfo.textData;

        // 1. 준비 텍스트 실행
        float prepareTotalTime = CalculateTotalTime(textData.prepareText);
        yield return StartCoroutine(RunTextCountdown(prepareTotalTime, textData.prepareText));

        // 2. 본 게임 타이머 실행
        var timerData = TimerInfo.timerData;
        if (timerData.infinityTime)
        {
            isInfinityTimeActive = true; // InfinityTime 활성화
            yield return StartCoroutine(RunInfinityTimer()); // InfinityTimer 실행
        }
        else
        {
            int playTimeInSeconds = timerData.playTime * 60; // 분 -> 초 변환
            yield return StartCoroutine(RunGameCountdown(playTimeInSeconds));
        }

        // 3. 종료 텍스트 실행
        float endTotalTime = CalculateTotalTime(textData.endText);
        yield return StartCoroutine(RunTextCountdown(endTotalTime, textData.endText));

        // 4. 씬 전환
        LoadNextScene();
    }

    /// <summary>
    /// 타이머가 무한일 때 실행되는 메서드
    /// </summary>
    /// <returns></returns>
    private IEnumerator RunInfinityTimer()
    {
        // InfinityTime일 때 타이머 고정
        if (timerText != null)
        {
            timerText.text = "99:99";
        }

        while (!isEscPressed)
        {
            // 매 초마다 총 플레이 타임 증가
            totalPlayTime += Time.deltaTime;
            yield return null;
        }

        isInfinityTimeActive = false;
        isEscPressed = false;
        Debug.Log("InfinityTime 종료: Esc 키가 눌렸습니다.");
    }

    /// <summary>
    /// 타이머 카운트 다운을 담당하는 메서드
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

            // 매 초마다 총 플레이 타임 증가
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
    /// 준비 단계 또는 마무리 단계에서 텍스트를 출력해주는 메서드
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

            // \n 제거 및 텍스트 처리
            message = message.Replace("\\n", "\n"); // \n을 Unity에서 인식할 수 있는 줄바꿈으로 변환

            float displayTime = parts.Length > 1 && float.TryParse(parts[1], out float time) ? time : 2f;

            centerLabel.text = message; // 줄바꿈이 적용된 텍스트 설정

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
    /// 준비 또는 마무리 단계에서 지연 되어야하는 시간을 구하는 메서드
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
    /// 씬 전환을 담당하는 메서드
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
            Debug.Log("더 이상 로드할 씬이 없습니다.");
        }
    }
}
