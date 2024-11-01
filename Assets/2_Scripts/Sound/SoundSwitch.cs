using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundSwitch : MonoBehaviour
{
    public AudioManager theAudio;

    //������Ʈ ĳ��
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
        if (isColliding && !isAwake) //EventCollider�� ��Ƽ� true�� �ž�����
        {
            if (isTouch) //Inspectorâ���� true������ ���� / ��ũ��Ʈ���� ���� �����ϴ� �κ��� ����.
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
