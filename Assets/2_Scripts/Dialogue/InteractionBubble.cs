using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionBubble : MonoBehaviour
{
    [Header("��ǳ�� ����")]
    public GameObject balloonPrefab; // ��ǳ�� ������
    public float offsetY = 30f;       // ������Ʈ �� ��ǳ�� ��ġ

    [Header("ũ�� ����")]
    public float fixedWidth = 120f;   // ��ǳ�� ���� �ʺ�
    public float minHeight = 20f;  // ��ǳ�� �ּ� ����
    public float lineHeight = 15f; // �� �ٴ� �߰��Ǵ� ����

    [Header("�޽���")]
    [TextArea(1, 10)]
    public string message = "�⺻ �޽����Դϴ�."; // ����� �޽���
    
    //������Ʈ ĳ�̹ޱ�
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
