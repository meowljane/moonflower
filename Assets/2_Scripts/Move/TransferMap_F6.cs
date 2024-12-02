using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap_F6 : MonoBehaviour
{
    private PlayerManager playerManager;
    private GameProgress gameProgress;

    public string nextScene;
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

        gameProgress.isF6 = true;

        SceneManager.LoadScene(nextScene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && gameProgress.CheckSceneNum() == 7)
        {
            Transfer();
        }
    }
}