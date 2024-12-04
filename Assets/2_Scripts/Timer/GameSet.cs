using UnityEngine;
using System.Collections.Generic;

public class GameSet : MonoBehaviour
{
    public List<GameInfos> gameProgress = new List<GameInfos>();
}

[System.Serializable]
public class GameInfos
{
    [Header("�����Ͱ� ���� ��")]
    public string sceneName;

    [Header("Ÿ�̸� �ð� ������")]
    public TimerData timerData;

    [Header("�ؽ�Ʈ ������")]
    public TextData textData;

    // Timer ���� ������
    [System.Serializable]
    public struct TimerData
    {
        [Tooltip("�÷��� �ð��� ������ ?")]
        public bool infinityTime;  // �÷��� �ð��� ����������

        [Tooltip("�ð� �ʰ��� ����� ���ΰ� ?")]
        public bool overTime;  // ���� Ÿ���� ����� ������

        [Tooltip("�� ���� �ð� [ 1 ~ 15 ] (��)")]
        [Range(0, 15)]
        public int playTime;    // �� ���� �ð� (��)
    }

    // ���� �Ǵ� ���� �ؽ�Ʈ ������
    [System.Serializable]
    public struct TextData
    {
        [Tooltip("���� �� ���� �ؽ�Ʈ ���� / ��� �ð�")]
        public List<string> prepareText; // ���� �� �ؽ�Ʈ ����

        [Tooltip("�Ϸ� �� ���� �ؽ�Ʈ ���� / ��� �ð�")]
        public List<string> endText; // ������ �� �ؽ�Ʈ ����
    }
}