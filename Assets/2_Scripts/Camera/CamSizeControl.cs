using UnityEngine;
using Unity.Cinemachine;

public class CamSizeControl : MonoBehaviour
{
    // 플레이어 매니저를 받아올 공간
    private PlayerManager playerManager;
    
    // 시네머신 카메라를 참조하기 위한 공간
    public CinemachineCamera cinemachineCamera;

    [Tooltip("카메라 기본 사이즈 250\n카메라 숲 사이즈 115")]
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
