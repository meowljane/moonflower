using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmoothBlinkingButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText; // �����Ÿ� ��ư
    public Color normalColor = Color.white; // �⺻ ��ư ����
    public Color blinkColor = Color.black;    // �����Ÿ� �� ��ư ����
    public float blinkSpeed = 1.0f;         // �����Ÿ� �ӵ� (���� Ŭ���� ������)

    private Image buttonImage;
    private bool isBlinking = false;

    private void Start()
    {
        // �ڷ�ƾ�� ���� �����Ÿ� ����
        Twinkle();
    }

    public void Twinkle()
    {
        // �ڷ�ƾ�� ���� �����Ÿ� ����
        StartCoroutine(SmoothBlink());
    }

    IEnumerator SmoothBlink()
    {
        isBlinking = true;
        float t = 0f;
        bool isLerpingToBlinkColor = true; // ������ �����Ÿ��� �������� ���ϴ��� Ȯ��

        while (isBlinking)
        {
            // t ���� 0���� 1���� ���� �����ϰų� ����
            t += (isLerpingToBlinkColor ? blinkSpeed : -blinkSpeed) * Time.deltaTime;

            // t ���� 0���� 1�� ����� �ʵ��� ����
            t = Mathf.Clamp01(t);

            // ������ normalColor���� blinkColor�� �ε巴�� ��ȯ
            buttonText.color = Color.Lerp(normalColor, blinkColor, t);

            // t ���� 1�� �����ϸ� �����Ÿ� ������ �ݴ�� �ٲ�
            if (t >= 1f)
            {
                isLerpingToBlinkColor = false;
            }
            // t ���� 0�� �����ϸ� �ٽ� �����Ÿ��� ����
            else if (t <= 0f)
            {
                isLerpingToBlinkColor = true;
            }

            yield return null; // �� �����Ӹ��� ����
        }
    }
}
