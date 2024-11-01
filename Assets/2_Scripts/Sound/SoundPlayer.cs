using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioManager theAudio;
    public bool isAwake;
    public int SoundNum;
    public bool isLoop;
    private void Awake()
    {
        theAudio = FindFirstObjectByType<AudioManager>();

        if(theAudio != null)
        {
            theAudio.TestPlay(SoundNum, isLoop);
        }   
    }
}