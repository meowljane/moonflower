using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

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

                audioSource.clip = data.startSound;
                audioSource.Play();
                break;

            case "BGM":
                if (data.BGM == null)
                {
                    return;
                }

                audioSource.clip = data.BGM;
                audioSource.Play();
                break;

            case "END":
                if (data.endSound == null)
                {
                    return;
                }

                audioSource.clip = data.endSound;
                audioSource.Play();
                break;
        }

        audioSource.Play();
    }
}