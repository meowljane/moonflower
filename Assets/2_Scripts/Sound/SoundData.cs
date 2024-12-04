using System.Collections.Generic;
using UnityEngine;

public class SoundData : MonoBehaviour
{
    [Tooltip("시작 시 재생될 사운드")]
    public AudioClip startSound;

    [Tooltip("씬에서 재생될 BGM")]
    public AudioClip BGM;

    [Tooltip("종료 시 재생될 사운드")]
    public AudioClip endSound;
}