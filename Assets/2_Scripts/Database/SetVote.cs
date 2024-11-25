using UnityEngine;

public class SetVote : MonoBehaviour
{
    private DatabaseManager theDM;

    [Header("투표성공")]
    public bool setVote;

    private void Awake()
    {
        theDM = FindObjectOfType<DatabaseManager>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            theDM.isCorrectVote = setVote;
        }
    }
}
