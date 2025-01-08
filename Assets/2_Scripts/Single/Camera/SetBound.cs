using UnityEngine;

public class SetBound : MonoBehaviour
{
    private CamBound camBound;
    private PlayerManager playerManager;
    public PolygonCollider2D bound2D;

    void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        camBound = FindObjectOfType<CamBound>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            camBound.SetBound(bound2D);
        }
    }
}
