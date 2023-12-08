using System;
using UnityEngine;


/* Enum for Sound types */

public enum SoundType
{
    ButtonClick,
    ChestOpen
}

/* Class for sounds */

[Serializable]
public class Sounds
{
    public SoundType Type;
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume;
}

/* Service for sounds in-game */
public class SoundService : GenericMonoSingleton<SoundService>
{
    private AudioSource source;
    public Sounds[] Sounds;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayClip(SoundType type)
    {
        Sounds sound = Array.Find(Sounds, i => i.Type == type);
        source.volume = sound.Volume;
        source.clip = sound.Clip;
        source.Play();
    }
}
