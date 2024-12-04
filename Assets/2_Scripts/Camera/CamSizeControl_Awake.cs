using UnityEngine;
using Unity.Cinemachine;

public class CamSizeControl_Awake : MonoBehaviour
{
    // �÷��̾� �Ŵ����� �޾ƿ� ����
    private PlayerManager playerManager;
    
    // �ó׸ӽ� ī�޶� �����ϱ� ���� ����
    public CinemachineCamera cinemachineCamera;

    [Tooltip("ī�޶� �⺻ ������ 250\nī�޶� �� ������ 115")]
    public int lensSize;

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

        CamSize();
    }

    private void CamSize()
    {
        cinemachineCamera.Lens.OrthographicSize = lensSize;
    }
}
