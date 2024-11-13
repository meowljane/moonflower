using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferLoc : MonoBehaviour
{
    private PlayerManager thePlayer;
    public Transform target;

    private void Awake()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Transfer();
        }
    }

    public void Transfer()
    {
        // 플레이어 위치 설정
        PlayerMove(thePlayer, target);
        TransferPlayerSet();
    }

    public void PlayerMove(PlayerManager thePlayer, Transform transform)
    {
        thePlayer.transform.position = transform.position;
    }

    public void TransferPlayerSet()
    {
        //씬 이동할때 플레이어가 정면을 바라보도록 설정하는 로직
        thePlayer.inputVec.y = -1;
        thePlayer.anim.SetFloat("DirY", thePlayer.inputVec.y);

        //애니메이션 조건문으로 바로 들어가지않도록 Vector값 0,0초기화
        thePlayer.inputVec.x = 0;
        thePlayer.inputVec.y = 0;
    }
}
