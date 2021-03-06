﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicholas Easterby - EAS12337350
//Switches the music in game depending on whether an invasion is occurring

public class SoundControl : MonoBehaviour
{
    public AudioClip calmMusic;
    public AudioClip tenseMusic;

    AudioSource sound;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void SwitchMusic(bool music)
    {
        if(music) { sound.clip = tenseMusic; }
        else{ sound.clip = calmMusic; }
        sound.Play();
    }

    public void Mute()
    {
        sound.mute = !sound.mute;
    }
}
