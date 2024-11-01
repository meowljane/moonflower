using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgm; //�긦 ���� ��ȯ�ɶ����� BgmON���� �����ü��ֵ��� �����ؾ���
    public AudioSource effectSound; //ȿ����
    public VideoPlayer video;

    private bool isBgm = true;
    private bool isClick = true;

    [SerializeField] private float bgmVolume;
    [SerializeField] private float effectSoundVolume;

    /// <summary>
    /// �����̴� ��� ���� �޼���
    /// </summary>
    /// <param name="volume"></param>
    public void SetVideoSound(float volume) //���� ���� ����
    {   
        if (isBgm)
            video.SetDirectAudioVolume(0, volume);
        else
            bgmVolume = volume;
    }

    public void SetMusicVolume(float volume) //����� ���� �����̴�
    {
        if (isBgm)
            bgm.volume = volume;
    }

    public void SetButtonVolume(float volume) //ȿ���� ���� �����̴�
    {
        if (isClick)
            effectSound.volume = volume;
        else
            effectSoundVolume = volume;
    }

    /// <summary>
    /// ����â�� �ִ� Ŭ��(�հ�����ư) ���� �ѹ��� Ʋ���ִ� �뵵
    /// ���߿� ���߹����� �����������(�� ����� �Ǹ� ��Ÿ������ �Ҹ� �ȳ����� �����ؾ���)
    /// </summary>
    public void OnBtn()
    {
        if(isClick)
            effectSound.Play();
    }

    /// <summary>
    /// ��� ���� On Off ��ư �޼���
    /// </summary>
    public void OnBgmMuteVolume()
    {
        if(isBgm)
        {
            bgmVolume = bgm.volume;
            bgm.volume = 0;
            video.SetDirectAudioVolume(0, 0);
            isBgm = false;
        }
        else if(!isBgm)
        {
            bgm.volume = bgmVolume;
            video.SetDirectAudioVolume(0, bgmVolume);
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
            effectSoundVolume = effectSound.volume;
            effectSound.volume = 0;
            isClick = false;
        }
        else
        {
            effectSound.volume = effectSoundVolume;
            isClick = true;
        }
    }
}
