using UnityEngine;
using System.Collections.Generic;

public class TimeSet : MonoBehaviour
{
    public List<TimerInfo> timerInfos = new List<TimerInfo>();
}

[System.Serializable]
public class TimerInfo
{
    [Header("�����Ͱ� ���� ��")]
    public string sceneName;

    [Header("Ÿ�̸� �ð� ������")]
    public TimerData timerData;

    [Header("�ؽ�Ʈ ������")]
    public TextData textData;

    [System.Serializable]
    public struct TimerData
    {
        [Tooltip("�÷��� �ð��� ������ ?")]
        public bool infinityTime;  // �÷��� �ð��� ����������

        //[Tooltip("���� �� �غ� �ð� [ 1 ~ 30 ] (��)")]
        //[Range(0, 30)]
        //public int prepareTime;   // ���� �� �غ� �ð� (��)

        //[Tooltip("�� ��ȯ ��� �ð� [ 1 ~ 10 ] (��)")]
        //[Range(0, 10)]
        //public int waitTime;  // �� ��ȯ ��� �ð� (��)

        [Tooltip("�� ���� �ð� [ 1 ~ 15 ] (��)")]
        [Range(0, 15)]
        public int playTime;    // �� ���� �ð� (��)
    }

    [System.Serializable]
    public struct TextData
    {
        [Tooltip("���� �� ���� �ؽ�Ʈ ���� / ��� �ð�")]
        public List<string> prepareText; // ���� �� �ؽ�Ʈ ����

        [Tooltip("�Ϸ� �� ���� �ؽ�Ʈ ���� / ��� �ð�")]
        public List<string> endText; // ������ �� �ؽ�Ʈ ����
    }
}