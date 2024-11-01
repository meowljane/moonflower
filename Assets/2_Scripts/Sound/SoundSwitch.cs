using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundSwitch : MonoBehaviour
{
    public AudioManager theAudio;

    //오브젝트 캐싱
    public WebGLBtn webglBtn;
    public GameObject confirmOn;

    public int audioNum;

    public bool isAwake = false;
    public bool isLoop = false;
    public bool isOnce = false;
    public bool isTouch = false;
    private bool isColliding = false;

    void Awake()
    {
        theAudio = FindFirstObjectByType<AudioManager>();
        webglBtn = Resources.FindObjectsOfTypeAll<WebGLBtn>().FirstOrDefault();
        confirmOn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(g => g.name == "ConfirmOn");
        if (isAwake)
        {
            PlayObjectSound();
        }
    }

    private void Update()
    {
        if (isColliding && !isAwake) //EventCollider에 닿아서 true가 돼었을때
        {
            if (isTouch) //Inspector창에서 true값으로 고정 / 스크립트에서 따로 조절하는 부분이 없음.
            {
                PlayObjectSound();
            }
            else
            {
                confirmOn.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F) || webglBtn.isClick)
                {
                    PlayObjectSound();
                }
            }
        }
    }

    void PlayObjectSound()
    {
        theAudio.TestPlay(audioNum, isLoop);
        if (isOnce)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        if(confirmOn != null)
        {
            confirmOn.SetActive(false);
        }
    }
}
