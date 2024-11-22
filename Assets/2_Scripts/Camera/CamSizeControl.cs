using UnityEngine;
using Unity.Cinemachine;

public class CamSizeControl : MonoBehaviour
{
    // �÷��̾� �Ŵ����� �޾ƿ� ����
    private PlayerManager playerManager;
    
    // �ó׸ӽ� ī�޶� �����ϱ� ���� ����
    public CinemachineCamera cinemachineCamera;

    [Tooltip("ī�޶� �⺻ ������ 250\nī�޶� �� ������ 115")]
    public int lensSize;
    public int delayTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (playerManager == null)
        {
            playerManager = FindObjectOfType<PlayerManager>();
        }

        if (cinemachineCamera == null)
        {
            cinemachineCamera = FindObjectOfType<CinemachineCamera>();
        }
    }

    public void CamSize()
    {
        cinemachineCamera.Lens.OrthographicSize = lensSize;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Invoke("CamSize", delayTime);
        }
    }
}
