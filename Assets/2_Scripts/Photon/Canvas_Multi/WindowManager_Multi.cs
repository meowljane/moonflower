using NUnit.Framework;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager_Multi : MonoBehaviour
{
    public PlayerManager_Multi playerManager_Multi;
    public GameObject TextWindow;

    public GameObject button;
    public GameObject[] UIwindows;
    public GameObject[] Elsewindows;

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
            if(playerManager_Multi != null)
            {
                playerManager_Multi.canMove = true;
            }
        }
        else
        {
            playerManager_Multi.canMove = false;
        }
    }

    private void DeactivateUIWindows()
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
