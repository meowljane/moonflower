using System.Linq;
using UnityEngine;

public class InteractionBubble : MonoBehaviour
{
    //��ũ��Ʈ ĳ��
    private TextManager theTM;

    //������Ʈ ĳ��
    public WebGLBtn webglBtn;
    public GameObject confirmOn;

    //����� �ؽ�Ʈ
    [TextArea]
    public string sentences;

    //��ȭâ �ܷ� �ð� ��Ÿ���� float��
    public float closeCount = 2.0f;

    //�ν����ͷ� �������ִ� �Ұ�
    public bool isAlways = true;

    //���� �� ���ϴ� �Ұ�
    private bool isColliding = false;
    private bool isActive = false;

    void Awake()
    {
        theTM = FindFirstObjectByType<TextManager>();
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
        confirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
    }


    private void Update()
    {
        if (isColliding && !isActive) //EventCollider�� ��Ƽ� true�� �ž�����
        {

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
    }

    void CloseTMText()
    {
        theTM.CloseText();
        isActive = false;
    }
}
