using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioArray", menuName = "Scriptable Objects/AudioArray", order =1)]
public class AudioArrayScriptableObject : ScriptableObject
{
    public AudioClip[] audioClips;

    public AudioClip GetRandomClip()
    {
        int randomIndex = Random.Range(0, audioClips.Length - 1);
        return audioClips[randomIndex];
    }

    public AudioClip GetClipAtIndex(int index)
    {
        return audioClips[index];
    }
}
