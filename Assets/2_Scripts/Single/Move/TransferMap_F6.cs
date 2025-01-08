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
    /// 씬 이동을 위해서 실행되는 메서드(씬 이동은 GameProgress에서 진행됨)
    /// </summary>
    public void Transfer()
    {
        playerManager.isTransfer = true;

        // 애니메이션 조건문으로 바로 들어가지 않도록 Vector 값 0,0 초기화
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