using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpeedManager : MonoBehaviour
{
    public PlayerManager thePlayer;
    public float SetSpeed = 0f;
    public WindowManager theWM;
    private void Awake()
    {
        thePlayer = FindFirstObjectByType<PlayerManager>();
        thePlayer.playerSpeed = SetSpeed;
        //팝업초기화
        theWM = FindFirstObjectByType<WindowManager>();
        theWM.CloseWindow();
    }
}
