using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioArrayScriptableObject audioArray;

    public void PlaySound()
    {
        AudioSource audioSoucre = gameObject.AddComponent<AudioSource>();
    }
}
