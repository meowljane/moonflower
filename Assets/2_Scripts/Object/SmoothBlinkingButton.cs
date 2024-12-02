using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmoothBlinkingButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText; // 깜빡거릴 버튼
    public Color normalColor = Color.white; // 기본 버튼 색상
    public Color blinkColor = Color.black;    // 깜빡거림 시 버튼 색상
    public float blinkSpeed = 1.0f;         // 깜빡거림 속도 (값이 클수록 빠르게)

    private Image buttonImage;
    private bool isBlinking = false;

    private void Start()
    {
        // 코루틴을 통해 깜빡거림 시작
        Twinkle();
    }

    public void Twinkle()
    {
        // 코루틴을 통해 깜빡거림 시작
        StartCoroutine(SmoothBlink());
    }

    IEnumerator SmoothBlink()
    {
        isBlinking = true;
        float t = 0f;
        bool isLerpingToBlinkColor = true; // 색상이 깜빡거리는 색상으로 향하는지 확인

        while (isBlinking)
        {
            // t 값이 0에서 1까지 점차 증가하거나 감소
            t += (isLerpingToBlinkColor ? blinkSpeed : -blinkSpeed) * Time.deltaTime;

            // t 값이 0에서 1을 벗어나지 않도록 제한
            t = Mathf.Clamp01(t);

            // 색상을 normalColor에서 blinkColor로 부드럽게 전환
            buttonText.color = Color.Lerp(normalColor, blinkColor, t);

            // t 값이 1에 도달하면 깜빡거림 방향을 반대로 바꿈
            if (t >= 1f)
            {
                isLerpingToBlinkColor = false;
            }
            // t 값이 0에 도달하면 다시 깜빡거림을 시작
            else if (t <= 0f)
            {
                isLerpingToBlinkColor = true;
            }

            yield return null; // 매 프레임마다 실행
        }
    }
}
