using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionPanel : MonoBehaviour
{    
    //��ũ��Ʈ ĳ��
    public PanelManager thePM;

    //������Ʈ ĳ��
    public GameObject confirmOn;
    public WebGLBtn webglBtn;

    //���� �� ���ϴ� �Ұ�
    public bool isColliding = false;
    public bool isTouch = false;

    // Sprite �迭 �̸� �޾Ƶδ°�
    public List<Sprite> spriteData;

    private void Awake()
    {
        thePM = FindFirstObjectByType<PanelManager>();
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
        confirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
    }

    private void Update()
    {
        if (isColliding && !thePM.isTalking)
        {
            confirmOn.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F) || webglBtn.isClick)
            {
                if (spriteData != null && spriteData.Count > 0)
                {
                    thePM.ShowPanel(spriteData);
                }
                webglBtn.isClick = false;
            }
            if (isTouch)
            {
                thePM.ShowPanel(spriteData);
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

}
