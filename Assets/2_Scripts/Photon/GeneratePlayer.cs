using Photon.Pun;
using UnityEngine;

public class GeneratePlayer : MonoBehaviour
{
    public Transform startPoint;
    public FloatingJoystick joyStick;
    public GameObject webGLbtn;
    public Canvas canvas;
    public PhotonView PV;
    private GameObject Player;

    void Awake()
    {
        Player = PhotonNetwork.Instantiate("Player_Multi", startPoint.position, Quaternion.identity, 0);
        PlayerManager_Multi playerManager_Multi = Player.GetComponent<PlayerManager_Multi>();
        WindowManager_Multi windowManager_Multi = canvas.GetComponent<WindowManager_Multi>();
        playerManager_Multi.joystick = joyStick;
        playerManager_Multi.webglBtn = webGLbtn;
        playerManager_Multi.canvas = canvas;
        windowManager_Multi.playerManager_Multi = playerManager_Multi;
        Debug.Log("플레이어 생성");
    }
}
