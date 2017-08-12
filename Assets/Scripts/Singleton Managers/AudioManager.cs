using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource source;

    public void PlayAudio(AudioClip audioClip)
    {
        source.PlayOneShot(audioClip);
    }
}
