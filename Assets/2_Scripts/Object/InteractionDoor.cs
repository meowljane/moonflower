using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionDoor : MonoBehaviour
{
    //스크립트 캐싱
    public PanelManager thePM;

    //활성화, 비활성화할 오브젝트들
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

    // Sprite 배열 미리 받아두는곳
    public List<Sprite> spriteData;

    //오브젝트 캐싱
    public GameObject confirmOn;
    public WebGLBtn webglBtn;

    //게임 중 변하는 불값
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
