using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionDoor : MonoBehaviour
{
    //��ũ��Ʈ ĳ��
    public PanelManager thePM;

    //Ȱ��ȭ, ��Ȱ��ȭ�� ������Ʈ��
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    // Sprite �迭 �̸� �޾Ƶδ°�
    public List<Sprite> spriteData;

    //������Ʈ ĳ��
    public GameObject confirmOn;
    public WebGLBtn webglBtn;

    //���� �� ���ϴ� �Ұ�
    public bool isColliding = false;
    public bool isTouch = false;


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
                    thePM.ShowPanel(spriteData, true);
                }
                else
                {
                    UpdateObjects();
                }
                webglBtn.isClick = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
        thePM.objectsToDisable = objectsToDisable;
        thePM.objectsToEnable = objectsToEnable;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        confirmOn.SetActive(false);
    }

    public void ClickPanelEvent()
    {
        thePM.ShowPanel(spriteData, false);
    }

    private void UpdateObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}
