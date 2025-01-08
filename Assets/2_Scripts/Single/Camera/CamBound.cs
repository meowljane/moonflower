using UnityEngine;
using Unity.Cinemachine;
using Photon.Pun;
using System.Collections;
using Unity.Cinemachine;

public class CamBound : MonoBehaviour
{
    private PolygonCollider2D bound2D;

    public PlayerManager thePlayer;
    public CinemachineConfiner2D confiner2D;
    public CinemachineCamera cinemachineCamera;

    private void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
    }
    private IEnumerator FindPlayerCoroutine()
    {
        while (thePlayer == null)
        {
            // 모든 PlayerManager 객체를 찾음
            PlayerManager[] players = FindObjectsOfType<PlayerManager>();


            // 로컬 플레이어를 찾음 (PhotonView.IsMine이 true인 플레이어)
            foreach (PlayerManager player in players)
            {
                PhotonView playerPV = player.GetComponent<PhotonView>();
                if (playerPV != null && playerPV.IsMine) // 나 자신의 플레이어인지 확인
                {
                    thePlayer = player;
                    break;
                }
            }

            yield return null; // 다음 프레임까지 대기
        }

        // 여기서 player 컴포넌트를 참조하여 초기화
        var playerComponent = thePlayer.GetComponent<PlayerManager>();
        cinemachineCamera = FindObjectOfType<CinemachineCamera>();

        if (playerComponent != null)
        {
            // 필요한 참조 설정
            cinemachineCamera.Follow = thePlayer.transform;
        }
    }

    public void SetBound(PolygonCollider2D setBound)
    {
        bound2D = setBound;
        confiner2D.BoundingShape2D = bound2D;
    }
}
