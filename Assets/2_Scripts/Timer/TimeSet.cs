using UnityEngine;
using System.Collections.Generic;

public class TimeSet : MonoBehaviour
{
    public List<TimerInfo> timerInfos = new List<TimerInfo>();
}

[System.Serializable]
public class TimerInfo
{
    [Header("데이터가 사용될 씬")]
    public string sceneName;

    [Header("타이머 시간 데이터")]
    public TimerData timerData;

    [Header("텍스트 데이터")]
    public TextData textData;

    [System.Serializable]
    public struct TimerData
    {
        [Tooltip("플레이 시간이 무제한 ?")]
        public bool infinityTime;  // 플레이 시간이 무제한인지

        //[Tooltip("시작 전 준비 시간 [ 1 ~ 30 ] (초)")]
        //[Range(0, 30)]
        //public int prepareTime;   // 시작 전 준비 시간 (초)

        //[Tooltip("씬 전환 대기 시간 [ 1 ~ 10 ] (초)")]
        //[Range(0, 10)]
        //public int waitTime;  // 씬 전환 대기 시간 (초)

        [Tooltip("본 게임 시간 [ 1 ~ 15 ] (분)")]
        [Range(0, 15)]
        public int playTime;    // 본 게임 시간 (분)
    }

    [System.Serializable]
    public struct TextData
    {
        [Tooltip("시작 시 사용될 텍스트 내용 / 대기 시간")]
        public List<string> prepareText; // 시작 전 텍스트 내용

        [Tooltip("완료 시 사용될 텍스트 내용 / 대기 시간")]
        public List<string> endText; // 끝나기 전 텍스트 내용
    }
}