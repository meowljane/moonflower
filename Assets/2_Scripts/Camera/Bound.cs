using UnityEngine;

public class Bound : MonoBehaviour
{
    private CamBound camBound;
    private PolygonCollider2D bound2D;
    
    public bool isAwake = false;

    private void Awake()
    {
        camBound = FindObjectOfType<CamBound>();

        if (isAwake)
        {
            bound2D = GetComponent<PolygonCollider2D>();
            SetBound(bound2D);
        }
    }

    private void SetBound(PolygonCollider2D bound2D)
    {
        camBound.SetBound(bound2D);
    }
}
