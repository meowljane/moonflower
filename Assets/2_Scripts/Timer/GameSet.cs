using UnityEngine;
using System.Collections.Generic;

public class GameSet : MonoBehaviour
{
    public List<GameInfos> gameProgress = new List<GameInfos>();
}

[System.Serializable]
public class GameInfos
{
    [Header("데이터가 사용될 씬")]
    public string sceneName;

    [Header("타이머 시간 데이터")]
    public TimerData timerData;

    [Header("텍스트 데이터")]
    public TextData textData;

    // Timer 관련 데이터
    [System.Serializable]
    public struct TimerData
    {
        [Tooltip("플레이 시간이 무제한 ?")]
        public bool infinityTime;  // 플레이 시간이 무제한인지

        [Tooltip("시간 초과를 기록할 것인가 ?")]
        public bool overTime;  // 오버 타임을 기록할 것인지

        [Tooltip("본 게임 시간 [ 1 ~ 15 ] (분)")]
        [Range(0, 15)]
        public int playTime;    // 본 게임 시간 (분)
    }

    // 시작 또는 종료 텍스트 데이터
    [System.Serializable]
    public struct TextData
    {
        [Tooltip("시작 시 사용될 텍스트 내용 / 대기 시간")]
        public List<string> prepareText; // 시작 전 텍스트 내용

        [Tooltip("완료 시 사용될 텍스트 내용 / 대기 시간")]
        public List<string> endText; // 끝나기 전 텍스트 내용
    }
}