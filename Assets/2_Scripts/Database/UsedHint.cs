using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedHint : MonoBehaviour
{
    public string questName;

    public void UseHint()
    {
        UpdateQuestStatusInDatabase(questName, QuestStatus.HintSeen);
    }
    private void UpdateQuestStatusInDatabase(string questName, QuestStatus newStatus)
    {
        DatabaseManager databaseManager = FindFirstObjectByType<DatabaseManager>();
        if (databaseManager != null)
        {
            databaseManager.UpdateQuestStatus(questName, newStatus);
        }
        else
        {
            Debug.LogWarning("DatabaseManager not found in scene.");
        }
    }
}
