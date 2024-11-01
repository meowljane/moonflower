using UnityEngine;

public class BGMSelect : MonoBehaviour
{
    public AudioManager theAudioManager;

    public int bgmNum;

    private void Awake()
    {
        theAudioManager = FindFirstObjectByType<AudioManager>();
    }

    private void Start()
    {
        theAudioManager.SelectMapSound(bgmNum);
    }

    private void OnEnable()
    {
        theAudioManager = FindFirstObjectByType<AudioManager>();
        theAudioManager.SelectMapSound(bgmNum);
    }
}
