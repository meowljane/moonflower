using UnityEngine;

public class isSeenHint : MonoBehaviour
{
    private DatabaseManager theDM;

    //오브젝트 캐싱
    public GameObject setHintObj;

    private void Awake()
    {
        theDM = FindFirstObjectByType<DatabaseManager>();
        if (theDM.isCheckedHint)
        {
            setHintObj.gameObject.SetActive(true);
        }
    }
}
