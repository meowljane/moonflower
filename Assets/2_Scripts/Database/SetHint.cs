using UnityEngine;

public class SetHint : MonoBehaviour
{
    private DatabaseManager theDM;
    public GameObject ceiling;

    private void Awake()
    {
        theDM = FindFirstObjectByType<DatabaseManager>();
        theDM.isCheckedHint = true;
    }
}
