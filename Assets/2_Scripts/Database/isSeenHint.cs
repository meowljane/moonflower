using UnityEngine;

public class isSeenHint : MonoBehaviour
{
    private DatabaseManager theDM;

    //������Ʈ ĳ��
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
