using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLogic : MonoBehaviour
{
    public List<AudioClip> sounds;
    public List<AudioSource> sources;

    public void Awake()
    {
        sources = new List<AudioSource>();
        foreach(AudioClip sound in sounds)
        {
            sources.Add(new AudioSource());
        }
    }

    public void PlayButtonSound(int index)
    {
        // sources[index].play
    }
}
