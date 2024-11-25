using UnityEngine;

public class SetLevel : MonoBehaviour
{
    private DatabaseManager theDM;

    [Header("���̵�")]
    [Tooltip("true�϶� Hard")]
    public bool setLevel;

    private void Awake()
    {
        theDM = FindObjectOfType<DatabaseManager>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            theDM.isHard = setLevel;
        }
    }
}
