using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionText : MonoBehaviour
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
    public bool isTouch = true;

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
            if (isTouch) //Inspectorâ���� true������ ���� / ��ũ��Ʈ���� ���� �����ϴ� �κ��� ����.
            {
                ActiveText(sentences);
            }
            else
            {
                confirmOn.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F) || webglBtn.isClick)
                {
                    ActiveText(sentences);
                }
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
    }
    void ActiveText(string sentences)
    {
        theTM.ShowText(sentences);
        isActive = true;
        Invoke("CloseTMText", closeCount);
    }
    void CloseTMText()
    {
        theTM.CloseText();
        isActive = false;
    }
}
