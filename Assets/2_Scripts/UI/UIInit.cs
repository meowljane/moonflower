using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using WebGLSupport;

public class UIInit : MonoBehaviour
{
    //스크립트로 참조받는 오브젝트
    private PlayerManager thePlayer;

    //UI 기본 창
    public GameObject button;

    //꺼야하는 창들 목록
    public WindowSettings[] windowsSettings;
    [System.Serializable]
    public class WindowSettings
    {
        public GameObject window;
    }

    //초기화시킬 창들 목록 
    public WindowInit[] changeWindow;
    [System.Serializable]
    public class WindowInit
    {
        public GameObject window;
        public bool isActive = false;
    }

    void Awake()
    {
        thePlayer = FindFirstObjectByType<PlayerManager>();
    }
    private void Update()
    {

        // button UI의 활성화 유무로 플레이어 이동 여부 제어.
        if (button.activeInHierarchy)
        {
            thePlayer.canMove = true;

            if (Input.GetKeyDown(KeyCode.F2))
            {
                windowsSettings[0].window.SetActive(true);
                button.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.F8))
            {
                windowsSettings[1].window.SetActive(true);
                button.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                windowsSettings[2].window.SetActive(true);
                button.SetActive(false);
            }
        }

        else
        {
            thePlayer.canMove = false;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeactivateAllWindows();
            }
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
        button.SetActive(true);
    }

}
