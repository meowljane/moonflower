using UnityEngine;

public class SetHint : MonoBehaviour
{
    private DatabaseManager theDM;
    public GameObject[] objectsToDisable;

    private void Awake()
    {
        theDM = FindFirstObjectByType<DatabaseManager>();
        theDM.isCheckedHint = true;
        UpdateObjects();
    }

    private void UpdateObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }
}
