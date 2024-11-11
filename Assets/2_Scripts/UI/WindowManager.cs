using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    private PlayerManager thePlayer;
    public GameObject button;
    public GameObject DialogueWindow;
    public GameObject TextWindow;

    [System.Serializable]
    public class WindowSettings
    {
        public GameObject window;
    }
    public WindowSettings[] windowsSettings;


    public GameObject[] onWindow;
    public GameObject[] offWindow;

    //public bool isUp = false;
    void Awake()
    {
        thePlayer = FindFirstObjectByType<PlayerManager>();
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
                thePlayer.canMove = true;
                settings.window.SetActive(false);
            }
        }
    }

    public void OpenWindow(GameObject windowToOpen)
    {
        DeactivateAllWindows();
        button.SetActive(false);
        windowToOpen.SetActive(true);
    }

    public void CloseWindow()
    {
        button.SetActive(true);
        DialogueWindow.SetActive(false);
        TextWindow.SetActive(false);
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
