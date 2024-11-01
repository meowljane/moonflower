using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager instance;

    // Singleton 패턴 적용
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    public List<PersonInfo> personInfos = new List<PersonInfo>();

    public List<QuestInfo> questInfos = new List<QuestInfo>();

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

    public void UpdateQuestStatus(string questName, QuestStatus newStatus)
    {
        QuestInfo quest = questInfos.Find(q => q.questName == questName);
        if (quest != null)
        {
            quest.status = newStatus;
        }
        else
        {
            Debug.LogWarning("Quest with name " + questName + " not found in database.");
        }
    }

    public void UpdateItemStatus(string itemName, ItemStatus newStatus)
    {
        ItemInfo item = itemInfos.Find(q => q.itemName == itemName);
        if (item != null)
        {
            item.status = newStatus;
        }
        else
        {
            Debug.LogWarning("Quest with name " + itemName + " not found in database.");
        }
    }

    public void UpdatePersonStatus(string personName, bool newStatus)
    {
        PersonInfo person = personInfos.Find(q => q.name == personName);
        if (person != null)
        {
            person.isActive = newStatus;
        }
        else
        {
            Debug.LogWarning("Quest with name " + person + " not found in database.");
        }
    }
}

// 인물 정보
[System.Serializable]
public class PersonInfo
{
    public string name;
    public bool isActive; 

    public PersonInfo(string _name, bool _isActive)
    {
        name = _name;
        isActive = _isActive;
    }
}

// 퀘스트 상태 
public enum QuestStatus
{
    NotSeen,    // 안봤음
    Seen,       // 봤음
    HintSeen    // 힌트봤음
}

// 퀘스트 정보 
[System.Serializable]
public class QuestInfo
{
    public string questName;
    public QuestStatus status;

    public QuestInfo(string _questName, QuestStatus _status)
    {
        questName = _questName;
        status = _status;
    }
}

public enum ItemStatus
{
    NotHave,    // 안얻음
    Have,       // 얻음
    Used    // 사용함
}

[System.Serializable]
public class ItemInfo
{
    public string itemName;
    public ItemStatus status;

    public ItemInfo(string _itemName, ItemStatus _status)
    {
        itemName = _itemName;
        status = _status;
    }
}
