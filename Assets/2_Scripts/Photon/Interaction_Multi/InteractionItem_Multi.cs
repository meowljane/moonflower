using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static InteractionDialogue_Multi;

public class InteractionItem_Multi : AbstractInteraction
{
    //스크립트 캐싱
    public ItemPopUpManager_Multi itemPopUpManager_Multi;

    //db관련된 문자열 변수
    public List<string> itemNames = new List<string>();

    // Sprite 배열 미리 받아두는곳
    public List<Sprite> spriteData;

    void Awake()
    {
    //    theIPM = FindFirstObjectByType<ItemPopUpManager>();
    //    webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
    //    confirmn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
    }

    void Update()
    {
        UpdateMethod();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        itemPopUpManager_Multi = playerManager_Multi.canvas.GetComponent<ItemPopUpManager_Multi>();
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }

    public override void UpdateMethod()
    {
        if (isColliding && !isActive)
        {
            confirmBtn.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F) || webglBtn.isClick)
            {
                List<Sprite> itemSprites = GetItemDetailSprites();
                if (itemSprites != null && itemSprites.Count > 0)
                {
                    itemPopUpManager_Multi.ShowItem(itemSprites);
                }
                ChangeDb();
                webglBtn.isClick = false;
            }
        }
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
        DatabaseManager databaseManager = FindObjectOfType<DatabaseManager>(); // 나중에 수정해야함
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
