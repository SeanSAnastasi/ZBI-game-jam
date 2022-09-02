using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLogic : MonoBehaviour
{
    public List<AudioClip> sounds;
    public List<AudioSource> sources;

    public void Start()
    {
        foreach(AudioSource source in sources)
        {
            source.clip = sounds[sources.IndexOf(source)];
        }
    }

    public void PlayButtonSound(int index)
    {
        sources[index].Play();
    }
}
