using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionItem : MonoBehaviour
{
    //오브젝트 캐싱
    public GameObject confirmOn;
    public WebGLBtn webglBtn;

    //db관련된 문자열 변수
    public string itemName;

    //게임 중 변하는 불값
    public bool isColliding = false;

    private void Awake()
    {
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
        confirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
    }
    private void Update()
    {
        if (isColliding)
        {
            //confirmOn.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F) || webglBtn.isClick)
            {
                ChangeDb();
                webglBtn.isClick = false;
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

    public void ChangeDb()
    {
        if (itemName != null)
        {
            DatabaseManager databaseManager = FindObjectOfType<DatabaseManager>();
            databaseManager.UpdateItemStatus(itemName, true);
        }
    }
}
