using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;



public class AudioManager : MonoBehaviour
{
    static public AudioManager instance;

    public PlayerManager thePlayer; //�÷��̾��� �� ī��Ʈ�� �θ��� ���� ����

    public AudioSource bgmAudioSource;
    public AudioSource effectAudioSource;

    public AudioClip[] bgmClips;
    public AudioClip[] effectClips;

    //�����̻��� ����
    //public bool isSound = true;
    private bool isBgm = true;
    private bool isClick = true;
    
    private float bgmVolume;
    private float effectSoundVolume;

    /// <summary>
    /// �� ���� �˸��� ���� ����ϴ� �޼���
    /// </summary>
#region
    public void SelectSceneSound(int num)
    {
        if (thePlayer.isTransfer)
        {
            bgmAudioSource.clip = bgmClips[num]; //monologue
            bgmAudioSource.Play();
            thePlayer.isTransfer = false;
        }
    }
    #endregion

    public void SelectMapSound(int num)
    {
        bgmAudioSource.clip = bgmClips[num];
        bgmAudioSource.Play();
        thePlayer.isTransfer = false;
    }

    /// <summary>
    /// ȿ���� ������ִ� �޼���
    /// </summary>
    /// <param name="num"></param>
    public void TestPlay(int num , bool isLoop)
    {
        Debug.Log("�ӽ� ���ͻ��� ���");
        effectAudioSource.clip = effectClips[num];
        effectAudioSource.loop = isLoop;

        effectAudioSource.Play();
    }

    public void OnePlay(int num)
    {
        effectAudioSource.clip = bgmClips[num];
        effectAudioSource.Play();
    }

    public void TestStop()
    {
        Debug.Log("���� ����");

        effectAudioSource.Stop();
    }
    /// <summary>
    /// ������ ���� ���� �޼���
    /// </summary>
    /// <param name="volume"></param>

    public void SetMusicVolume(float volume) //����� ���� �����̴�
    {
        if (isBgm)
            bgmAudioSource.volume = volume;
    }

    public void SetButtonVolume(float volume) //ȿ���� ���� �����̴�
    {
        if (isClick)
            effectAudioSource.volume = volume;
        else
            effectSoundVolume = volume;
    }

    /// <summary>
    /// ��� ���� On Off ��ư �޼���
    /// </summary>
    public void OnBgmMuteVolume()
    {
        if (isBgm)
        {
            bgmVolume = bgmAudioSource.volume;
            bgmAudioSource.volume = 0;
            isBgm = false;
        }
        else if (!isBgm)
        {
            bgmAudioSource.volume = bgmVolume;
            isBgm = true;
        }
    }

    /// <summary>
    /// ȿ���� ���� On Off ��ư �޼���
    /// </summary>
    public void OnMusicMuteVolume()
    {
        if (isClick)
        {
            effectSoundVolume = effectAudioSource.volume;
            effectAudioSource.volume = 0;
            isClick = false;
        }
        else
        {
            effectAudioSource.volume = effectSoundVolume;
            isClick = true;
        }
    }

    /// <summary>
    /// F25 ������ ������ ���� ��µǸ� VidPlayer���� BGM�� ���ְ� �ϴ� �޼���
    /// </summary>
    public void OffBgmSound()
    {
        bgmAudioSource.Stop();
    }
}