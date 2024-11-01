using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;



public class AudioManager : MonoBehaviour
{
    static public AudioManager instance;

    public PlayerManager thePlayer; //플레이어의 씬 카운트를 부르기 위한 변수

    public AudioSource bgmAudioSource;
    public AudioSource effectAudioSource;

    public AudioClip[] bgmClips;
    public AudioClip[] effectClips;

    //윤성이사운드 변수
    //public bool isSound = true;
    private bool isBgm = true;
    private bool isClick = true;
    
    private float bgmVolume;
    private float effectSoundVolume;

    /// <summary>
    /// 각 씬에 알맞은 사운드 출력하는 메서드
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
    /// 효과음 출력해주는 메서드
    /// </summary>
    /// <param name="num"></param>
    public void TestPlay(int num , bool isLoop)
    {
        Debug.Log("임시 엔터사운드 출력");
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
        Debug.Log("사운드 정지");

        effectAudioSource.Stop();
    }
    /// <summary>
    /// 윤성이 사운드 조절 메서드
    /// </summary>
    /// <param name="volume"></param>

    public void SetMusicVolume(float volume) //배경음 사운드 슬라이더
    {
        if (isBgm)
            bgmAudioSource.volume = volume;
    }

    public void SetButtonVolume(float volume) //효과음 사운드 슬라이더
    {
        if (isClick)
            effectAudioSource.volume = volume;
        else
            effectSoundVolume = volume;
    }

    /// <summary>
    /// 브금 사운드 On Off 버튼 메서드
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
    /// 효과음 사운드 On Off 버튼 메서드
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
    /// F25 마지막 씬에서 영상 출력되면 VidPlayer에서 BGM을 꺼주게 하는 메서드
    /// </summary>
    public void OffBgmSound()
    {
        bgmAudioSource.Stop();
    }
}