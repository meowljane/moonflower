using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionBubble : MonoBehaviour
{
    [Header("말풍선 설정")]
    public GameObject balloonPrefab; // 말풍선 프리팹
    public float offsetY = 30f;       // 오브젝트 위 말풍선 위치

    [Header("크기 설정")]
    public float fixedWidth = 120f;   // 말풍선 고정 너비
    public float minHeight = 20f;  // 말풍선 최소 높이
    public float lineHeight = 15f; // 한 줄당 추가되는 높이

    [Header("메시지")]
    [TextArea(1, 10)]
    public string message = "기본 메시지입니다."; // 출력할 메시지
    
    //오브젝트 캐싱받기
    private GameObject currentBalloon;
    private TextMeshPro balloonText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (balloonPrefab != null)
            {
                currentBalloon = Instantiate(balloonPrefab, transform.position, Quaternion.identity);
                currentBalloon.SetActive(false);

                balloonText = currentBalloon.GetComponentInChildren<TextMeshPro>();
            }
            ShowBalloon(message);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HideBalloon();
        }
    }

    public void ShowBalloon(string text)
    {
        if (currentBalloon != null)
        {
            balloonText.text = text;

            Vector3 newPosition = transform.position + new Vector3(0, offsetY, 0);
            currentBalloon.transform.position = newPosition;

            currentBalloon.SetActive(true);
            AdjustBalloonSize();
        }
    }

    public void HideBalloon()
    {
        if (currentBalloon != null)
        {
            Destroy(currentBalloon);
        }
    }

    private void AdjustBalloonSize()
    {
        if (currentBalloon != null)
        {
            SpriteRenderer balloonRenderer = currentBalloon.GetComponent<SpriteRenderer>();
            if (balloonRenderer != null && balloonText != null)
            {
                balloonText.ForceMeshUpdate();
                int lineCount = balloonText.textInfo.lineCount;

                float height = Mathf.Max(minHeight, lineCount * lineHeight);

                balloonRenderer.size = new Vector2(fixedWidth, height);
                balloonText.transform.localPosition = new Vector3(0, height / 2, 0);

            }
        }
    }
}
