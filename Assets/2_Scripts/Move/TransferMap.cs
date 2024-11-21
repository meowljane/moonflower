using UnityEngine;

public class TransferMap : MonoBehaviour
{
    private PlayerManager playerManager;
    private GameProgress gameProgress;
    void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        gameProgress = FindObjectOfType<GameProgress>();
    }

    /// <summary>
    /// �� �̵��� ���ؼ� ����Ǵ� �޼���(�� �̵��� GameProgress���� �����)
    /// </summary>
    public void Transfer()
    {
        playerManager.isTransfer = true;

        // �ִϸ��̼� ���ǹ����� �ٷ� ���� �ʵ��� Vector �� 0,0 �ʱ�ȭ
        playerManager.inputVec.x = 0;
        playerManager.inputVec.y = 0;

        gameProgress.SkipPlayCoroutine();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Transfer();
            this.gameObject.SetActive(false);
        }
    }
}