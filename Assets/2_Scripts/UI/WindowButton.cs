using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class WindowButton : MonoBehaviour
{
    public GameObject[] globalActivateOnEnable;
    public GameObject[] globalDeactivateOnDisable;

    public GameObject dialogueManager;
    public GameObject questTextManager;

    private PlayerManager thePlayer;
    //public AudioManager theAudio;
    public Animator anim;
    public GameObject button;
    [System.Serializable]
    public class WindowSettings
    {
        public GameObject window;
    }
    [System.Serializable]
    public class WindowChange
    {
        public GameObject window;
        public Sprite detailImage;
    }
    public WindowSettings[] windowsSettings;
    public WindowChange[] changeWindow;

    //public bool isUp = false;
    void Awake()
    {
        thePlayer = FindFirstObjectByType<PlayerManager>();
        //theAudio = FindObjectOfType<AudioManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (button.activeInHierarchy)
            {
                windowsSettings[1].window.SetActive(true);
                button.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivateAllWindows();
        }
        if (button.activeInHierarchy)
        {
            thePlayer.canMove = true;
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

            button.SetActive(true);
        }
    }

    public void InitializeWindows()
    {
        foreach (var window in globalActivateOnEnable)
        {
            if (window != null)
            {
                if (!dialogueManager.activeSelf && !questTextManager.activeSelf)
                {

                    window.SetActive(true);
                }
            }
        }
    }

    private void DeinitializeWindows()
    {
        foreach (var window in globalDeactivateOnDisable)
        {
            if (window != null)
            {
                window.SetActive(false);
            }
        }
    }


    public void OnWindow()
    {
        foreach (var settings in windowsSettings)
        {
            if (settings.window != null)
            {
                settings.window.SetActive(true);
            }
        }
    }

    public void OffWindow()
    {
        foreach (var settings in windowsSettings)
        {
            if (settings.window != null)
            {
                settings.window.SetActive(false);
            }
        }
    }

    public void ToggleWindow(int index)
    {
        //isUp = !isUp;
        if (index >= 0 && index < windowsSettings.Length && windowsSettings[index].window != null)
        {
            bool isActive = windowsSettings[index].window.activeSelf;

            if (this.gameObject.tag == "CloseBtn")// || (index == 0 && !isUp))
            {
                Debug.Log("꾸물꾸물서윤성");
                //버튼을 누를때 fasle로 초기화
                //F8_False();
                anim.SetTrigger("Phone_Close");
                //anim.SetTrigger("Bag_Close");
                //버튼 누르고 0.5초뒤에 꺼지도록 딜레이주는 조건
                Invoke("DeactivateAllWindows", 0.4f);
                if (thePlayer.canMove)
                {
                    windowsSettings[index].window.SetActive(!isActive);
                }
            }

            else
            {
                DeactivateAllWindows();
                windowsSettings[index].window.SetActive(!isActive);
            }


            if (isActive)
            {
                thePlayer.canMove = true;
            }
            else
            {
                thePlayer.canMove = false;
            }
            InitializeWindows();
            DeinitializeWindows();
        }
        else
        {
            Debug.LogWarning("창없음");
        }
    }

    public void ChangeWindow(int index)
    {
        if (index >= 0 && index < changeWindow.Length && changeWindow[index].window != null)
        {
            Image Image = changeWindow[index].window.GetComponent<Image>();
            if (Image != null)
            {
                Debug.Log("디테일 이미지 출력");
                Image.sprite = changeWindow[index].detailImage;
            }
            else
            {
                Debug.LogWarning("SpriteRenderer없");
            }
        }
        else
        {
            Debug.LogWarning("창없음");
        }
    }
}
