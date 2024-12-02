using System.Linq;
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
    public float showTime = 2f; // �޼��� ��Ÿ�� �ð�

    //������Ʈ ĳ�̹ޱ�
    private GameObject currentBalloon;
    private TextMeshPro balloonText;
    public GameObject confirmOn;
    public WebGLBtn webglBtn;

    //���� �� ���ϴ� �Ұ�
    public bool isColliding = false;
    public bool isActive = false;
    public bool isAwake = false;

    public void Awake()
    {
        confirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
        if (isAwake)
        {
            ShowBalloon(message);
        }
    }
    private void Update()
    {
        if (isColliding && !isAwake && !isActive)
        {
            confirmOn.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F) || webglBtn.isClick)
            {
                ShowBalloon(message);
                Invoke("HideBalloon", showTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        confirmOn.SetActive(false);
        CancelInvoke("HideBalloon");
        HideBalloon();
    }

    public void ShowBalloon(string text)
    {
        if (balloonPrefab != null)
        {
            currentBalloon = Instantiate(balloonPrefab, transform.position, Quaternion.identity);

            balloonText = currentBalloon.GetComponentInChildren<TextMeshPro>(); balloonText.text = text;

            Vector3 newPosition = transform.position + new Vector3(0, offsetY, 0);
            currentBalloon.transform.position = newPosition;

            AdjustBalloonSize();
        }

        isActive = true;
        confirmOn.SetActive(false);
    }

    public void HideBalloon()
    {
        if (currentBalloon != null)
        {
            isActive = false;
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
