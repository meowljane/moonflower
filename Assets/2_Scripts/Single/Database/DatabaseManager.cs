using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{

    static public DatabaseManager instance;

    [Header("난이도")]
    [Tooltip("true일때 Hard")]
    public bool isHard = false;

    [Header("초과된 시간")]
    public int seconds = 0;

    [Header("투표 정답 유무")]
    public bool isCorrectVote = false;

    [Header("힌트 확인 유무")]
    public bool isCheckedHint = false;

    [Header("퀴즈 정답 상태")]
    public bool[] quizCorrect = new bool[10];

    [Header("아이템")]
    public List<ItemInfo> itemInfos = new List<ItemInfo>();


    private float elapsedTime = 0f;

    //void Update()
    //{
    //    elapsedTime += Time.deltaTime;

    //    if (elapsedTime >= 1f)
    //    {
    //        elapsedTime -= 1f;
    //        seconds++;
    //    }
    //}

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