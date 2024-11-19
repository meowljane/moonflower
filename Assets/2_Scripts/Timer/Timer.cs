using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    // 스크립트 캐싱
    [Header("Caching Script")]
    [Tooltip("TimeSet 코드가 들어간 오브젝트")]
    public TimeSet timeSet;

    // 오브젝트 캐싱
    [Header("Caching Text")]
    [Tooltip("타이머를 담당할 텍스트")]
    public Text timerText;

    [Tooltip("센터라벨을 담당할 텍스트")]
    public Text centerLabel;

    private TimerInfo TimerInfo;
    private bool isInfinityTimeActive = false; // InfinityTime 상태 관리 플래그
    private bool isEscPressed = false;        // Esc 키가 눌렸는지 확인

    void Awake()
    {
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

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

    void Update()
    {
        // Esc 키 입력 감지
        if (isInfinityTimeActive && Input.GetKeyDown(KeyCode.Escape))
        {
            isEscPressed = true;
        }
    }

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

    private IEnumerator RunInfinityTimer()
    {
        // InfinityTime일 때 타이머 고정
        if (timerText != null)
        {
            timerText.text = "99:99";
        }

        // Esc 키가 눌릴 때까지 대기
        while (!isEscPressed)
        {
            yield return null; // 매 프레임 대기
        }

        // Esc 키가 눌리면 InfinityTime 종료
        isInfinityTimeActive = false;
        isEscPressed = false;
        Debug.Log("InfinityTime 종료: Esc 키가 눌렸습니다.");
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

            // 1분 남았을 때 메시지 출력
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
        centerLabel.text = "1분 남았습니다.";
        yield return new WaitForSeconds(2f);
        centerLabel.text = ""; // 메시지 초기화
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
