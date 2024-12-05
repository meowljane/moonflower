using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static InteractionDialogue;

public class InteractionItem : MonoBehaviour
{
    //��ũ��Ʈ ĳ��
    public ItemPopUpManager theIPM;

    //������Ʈ ĳ��
    public GameObject confirmOn;
    public WebGLBtn webglBtn;

    //db���õ� ���ڿ� ����
    public List<string> itemNames = new List<string>();

    //���� �� ���ϴ� �Ұ�
    public bool isColliding = false;

    // Sprite �迭 �̸� �޾Ƶδ°�
    public List<Sprite> spriteData;

    private void Awake()
    {
        theIPM = FindFirstObjectByType<ItemPopUpManager>();
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
        confirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
    }
    private void Update()
    {
        if (isColliding && !theIPM.isTalking)
        {
            confirmOn.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F) || webglBtn.isClick)
            {
                List<Sprite> itemSprites = GetItemDetailSprites();
                if (itemSprites != null && itemSprites.Count > 0)
                {
                    theIPM.ShowItem(itemSprites);
                }
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
        if (itemNames != null && itemNames.Count > 0)
        {
            DatabaseManager databaseManager = FindObjectOfType<DatabaseManager>();
            foreach (string name in itemNames)
            {
                databaseManager.UpdateItemStatus(name, true);
            }
        }
    }

 private List<Sprite> GetItemDetailSprites()
    {
        DatabaseManager databaseManager = FindObjectOfType<DatabaseManager>();
        List<Sprite> sprites = new List<Sprite>();

        foreach (string name in itemNames)
        {
            foreach (ItemInfo itemInfo in databaseManager.itemInfos)
            {
                var itemData = itemInfo.items.Find(q => q.itemName == name);
                if (itemData.itemDetailImg != null)
                {
                    sprites.AddRange(itemData.itemDetailImg);
                }
            }
        }

        return sprites;
    }
}