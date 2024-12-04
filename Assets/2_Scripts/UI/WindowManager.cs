using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    private PlayerManager thePlayer;
    public GameObject button;
    public GameObject TextWindow;

    [System.Serializable]
    public class WindowSettings
    {
        public GameObject window;
    }
    public WindowSettings[] windowsSettings;

    public GameObject[] onWindow;
    public GameObject[] offWindow;

    public GameObject[] testObj;
    public List<bool> testList;
    //public bool isUp = false;
    void Awake()
    {
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
                windowsSettings[0].window.SetActive(true);
                button.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            if (button.activeInHierarchy)
            {
                windowsSettings[1].window.SetActive(true);
                button.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            if (button.activeInHierarchy)
            {
                windowsSettings[2].window.SetActive(true);
                button.SetActive(false);
            }
        }
        foreach (var settings in windowsSettings)
        {
            if (settings.window.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    DeactivateAllWindows();
                    button.SetActive(true);
                }
            }
        }



        if (button.activeInHierarchy)
        {
            thePlayer.canMove = true;
            WindowInit();
        }
        else
        {
            thePlayer.canMove = false;
        }
    }
    public void DeactivateAllWindows()
    {
        foreach (var settings in windowsSettings)
        {
            if (settings.window != null)
            {
                settings.window.SetActive(false);
            }
        }
    }

    public void OpenWindow(GameObject windowToOpen)
    {
        DeactivateAllWindows();
        TextWindow.SetActive(false);
        windowToOpen.SetActive(true);
        button.SetActive(false);
    }

    public void CloseWindow()
    {
        button.SetActive(true);
    }

    public void WindowInit()
    {
        foreach (var windows in onWindow)
        {
            windows.SetActive(true);
        }
        foreach (var windows in offWindow)
        {
            windows.SetActive(false);
        }
    }
}
