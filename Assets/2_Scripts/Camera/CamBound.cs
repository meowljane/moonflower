using UnityEngine;
using Unity.Cinemachine;

public class CamBound : MonoBehaviour
{
    public CinemachineConfiner2D confiner2D;
    private PolygonCollider2D bound2D;

    public void SetBound(PolygonCollider2D setBound)
    {
        bound2D = setBound;
        confiner2D.BoundingShape2D = bound2D;
    }
}
