using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioStartEnd;
    public AudioSource audioBgm;

    public GameObject soundData;
    public SoundData data;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetSoundData();
    }

    public void GetSoundData()
    {
        soundData = GameObject.Find("SoundData");
        data = soundData.GetComponent<SoundData>();

        audioStartEnd.clip = data.startSound;
        audioBgm.clip = data.BGM;
        audioBgm.Play();
        audioBgm.Pause();
    }

    public void PlaySound(string sound)
    {
        switch(sound)
        {
            case "START":
                if (data.startSound == null)
                {
                    return;
                }

                audioStartEnd.Play();
                break;

            case "BGM":
                if (data.BGM == null)
                {
                    return;
                }

                audioBgm.Play();
                break;

            case "END":
                if (data.endSound == null)
                {
                    return;
                }

                audioStartEnd.clip = data.endSound;
                audioStartEnd.Play();
                break;
        }
    }

    public void StopSound(string sound)
    {
        switch (sound)
        {
            case "BGM":
                if (data.BGM == null)
                {
                    return;
                }

                audioBgm.Stop();
                break;

            default:
                if (data.startSound == null && data.endSound == null)
                {
                    return;
                }

                audioStartEnd.Stop();
                break;
        }
    }
}