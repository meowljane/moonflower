using UnityEngine;

public class SetHint : MonoBehaviour
{
    private DatabaseManager theDM;
    public GameObject ceiling;

    private void Awake()
    {
        theDM = FindObjectOfType<DatabaseManager>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            theDM.isCheckedHint = true;
            ceiling.SetActive(false);
        }
    }
}
