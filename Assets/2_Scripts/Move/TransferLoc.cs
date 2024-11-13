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
        // �÷��̾� ��ġ ����
        PlayerMove(thePlayer, target);
        TransferPlayerSet();
    }

    public void PlayerMove(PlayerManager thePlayer, Transform transform)
    {
        thePlayer.transform.position = transform.position;
    }

    public void TransferPlayerSet()
    {
        //�� �̵��Ҷ� �÷��̾ ������ �ٶ󺸵��� �����ϴ� ����
        thePlayer.inputVec.y = -1;
        thePlayer.anim.SetFloat("DirY", thePlayer.inputVec.y);

        //�ִϸ��̼� ���ǹ����� �ٷ� �����ʵ��� Vector�� 0,0�ʱ�ȭ
        thePlayer.inputVec.x = 0;
        thePlayer.inputVec.y = 0;
    }
}
