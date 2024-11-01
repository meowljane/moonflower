using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using WebGLSupport;

public class UIInit : MonoBehaviour
{
    //��ũ��Ʈ�� �����޴� ������Ʈ
    private PlayerManager thePlayer;

    //UI �⺻ â
    public GameObject button;

    //�����ϴ� â�� ���
    public WindowSettings[] windowsSettings;
    [System.Serializable]
    public class WindowSettings
    {
        public GameObject window;
    }

    //�ʱ�ȭ��ų â�� ��� 
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

        // button UI�� Ȱ��ȭ ������ �÷��̾� �̵� ���� ����.
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
