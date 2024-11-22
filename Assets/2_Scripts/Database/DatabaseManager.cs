using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{

    static public DatabaseManager instance;

    public List<ItemInfo> itemInfos = new List<ItemInfo>();

    // 게임 진행 시간 기록
    public int minutes = 0;
    public int seconds = 0;

    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 1f)
        {
            elapsedTime -= 1f;
            seconds++;

            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
            }
        }
    }

    public void UpdateItemStatus(string itemName, bool newStatus)
    {
        foreach (ItemInfo itemInfo in itemInfos)
        {
            ItemInfo.ItemData? itemData = itemInfo.items.Find(q => q.itemName == itemName);
            if (itemData.HasValue)
            {
                int index = itemInfo.items.IndexOf(itemData.Value);
                if (index != -1) // 유효한 인덱스인지 확인
                {
                    ItemInfo.ItemData updatedItemData = itemInfo.items[index];
                    updatedItemData.status = newStatus;
                    itemInfo.items[index] = updatedItemData;
                    return;
                }
            }
        }
    }

}

[System.Serializable]
public class ItemInfo
{
    public string roomName;
    public List<ItemData> items = new List<ItemData>();

    [System.Serializable]
    public struct ItemData
    {
        public string itemName;
        public Sprite itemImg;
        public List<Sprite> itemDetailImg;
        public bool status;
    }
}