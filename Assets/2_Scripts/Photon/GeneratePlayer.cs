using Photon.Pun;
using UnityEngine;

public class GeneratePlayer : MonoBehaviour
{
    public Transform startPoint;
    void Start()
    {
        PhotonNetwork.Instantiate("Player", startPoint.position, Quaternion.identity, 0 );
        Debug.Log("플레이어 생성");
    }
}
