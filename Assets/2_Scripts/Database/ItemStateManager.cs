using UnityEngine;
using UnityEngine.UI;

public class ItemStateManager : MonoBehaviour
{
    public string itemName;
    public Sprite inactiveSprite;
    public Sprite activeSprite; 

    public GameObject usedObj;   

    private void OnEnable()
    {
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        DatabaseManager databaseManager = DatabaseManager.instance;
        foreach (ItemInfo itemInfo in DatabaseManager.instance.itemInfos)
        {
            ItemInfo.ItemData? itemData = itemInfo.items.Find(q => q.itemName == itemName);

            if (itemInfo != null)
            {

            }
        }
    }
}
