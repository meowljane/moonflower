using NUnit.Framework;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    public PlayerManager thePlayer;
    public GameObject TextWindow;

    public GameObject button;
    public GameObject[] UIwindows;
    public GameObject[] Elsewindows;

    void Awake()
    {
        //StartCoroutine(FindPlayerCoroutine());
        thePlayer = FindFirstObjectByType<PlayerManager>();
        //불값 list를 만들어주고
        //확인해야하는 오브젝트 리스트의 길이 만큼 반복문 돌리고
        //그 값을 list에 앞에서부터 차곡차곡 넣어주면 끝
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (button.activeInHierarchy)
            {
                UIwindows[0].SetActive(true);
                button.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            if (button.activeInHierarchy)
            {
                UIwindows[1].SetActive(true);
                button.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            if (button.activeInHierarchy)
            {
                UIwindows[2].SetActive(true);
                button.SetActive(false);
            }
        }
        foreach (var settings in UIwindows)
        {
            if (settings.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    DeactivateUIWindows();
                    button.SetActive(true);
                }
            }
        }

        if (button.activeInHierarchy)
        {
            if(thePlayer != null)
            {
                thePlayer.canMove = true;
            }
        }
        else
        {
            thePlayer.canMove = false;
        }
    }

    //public IEnumerator FindPlayerCoroutine()
    //{
    //    while (thePlayer == null)
    //    {
    //        // 모든 PlayerManager 객체를 찾음
    //        PlayerManager[] players = FindObjectsOfType<PlayerManager>();

    //        // 로컬 플레이어를 찾음 (PhotonView.IsMine이 true인 플레이어)
    //        foreach (PlayerManager player in players)
    //        {
    //            PhotonView playerPV = player.GetComponent<PhotonView>();
    //            if (playerPV != null && playerPV.IsMine) // 나 자신의 플레이어인지 확인
    //            {
    //                thePlayer = player;
    //                break;
    //            }
    //        }

    //        yield return null; // 다음 프레임까지 대기
    //    }
    //}
    public void DeactivateUIWindows()
    {
        foreach (var settings in UIwindows)
        {
            if (settings != null)
            {
                settings.SetActive(false);
            }
        }
    }

    public void DeactivateTotalWindows()
    {
        foreach (var settings in UIwindows)
        {
            if (settings != null)
            {
                settings.SetActive(false);
            }
        }
        foreach (var settings in Elsewindows)
        {
            if (settings != null)
            {
                settings.SetActive(false);
            }
        }
    }

    public void OpenWindow(GameObject windowToOpen)
    {
        DeactivateTotalWindows();
        TextWindow.SetActive(false);
        windowToOpen.SetActive(true);
        button.SetActive(false);
    }

    public void CloseWindow()
    {
        button.SetActive(true);
    }
}
