using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{

    static public DatabaseManager instance;

    [Header("���̵�")]
    [Tooltip("true�϶� Hard")]
    public bool isHard = false;

    [Header("�ʰ��� �ð�")]
    public int seconds = 0;

    [Header("��ǥ ���� ����")]
    public bool isCorrectVote = false;

    [Header("��Ʈ Ȯ�� ����")]
    public bool isCheckedHint = false;

    [Header("���� ���� ����")]
    public bool[] quizCorrect = new bool[10];

    [Header("������")]
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
                if (index != -1) // ��ȿ�� �ε������� Ȯ��
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