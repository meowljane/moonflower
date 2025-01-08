using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgm; //얘를 씬이 전환될때마다 BgmON에서 가져올수있도록 수정해야함
    public AudioSource effectSound; //효과음
    public VideoPlayer video;

    private bool isBgm = true;
    private bool isClick = true;

    [SerializeField] private float bgmVolume;
    [SerializeField] private float effectSoundVolume;

    /// <summary>
    /// 슬라이더 담당 사운드 메서드
    /// </summary>
    /// <param name="volume"></param>
    public void SetVideoSound(float volume) //비디오 사운드 조절
    {   
        if (isBgm)
            video.SetDirectAudioVolume(0, volume);
        else
            bgmVolume = volume;
    }

    public void SetMusicVolume(float volume) //배경음 사운드 슬라이더
    {
        if (isBgm)
            bgm.volume = volume;
    }

    public void SetButtonVolume(float volume) //효과음 사운드 슬라이더
    {
        if (isClick)
            effectSound.volume = volume;
        else
            effectSoundVolume = volume;
    }

    /// <summary>
    /// 설정창에 있는 클릭(손가락버튼) 사운드 한번씩 틀어주는 용도
    /// 나중에 컨펌받을때 지울수도있음(안 지우게 되면 연타했을때 소리 안나도록 조정해야함)
    /// </summary>
    public void OnBtn()
    {
        if(isClick)
            effectSound.Play();
    }

    /// <summary>
    /// 브금 사운드 On Off 버튼 메서드
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
    /// 효과음 사운드 On Off 버튼 메서드
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
