using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemStateManager : MonoBehaviour
{  

    private void OnEnable()
    {
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        DatabaseManager databaseManager = DatabaseManager.instance;

        foreach (ItemInfo itemInfo in databaseManager.itemInfos)
        {
            if (itemInfo != null)
            {
                // Count items with status == true
                int trueStatusCount = itemInfo.items.Count(item => item.status);

                // Log the room name and the count of items with status == true
                Debug.Log($"Room: {itemInfo.roomName} has {trueStatusCount} item(s) with status set to true.");
            }
        }
    }
}
