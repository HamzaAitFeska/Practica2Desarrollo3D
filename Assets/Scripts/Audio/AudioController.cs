using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioMixer ambient, effects;
    public AudioSource weaponShootBlue, weaponShootOrange, portalInvalidSurface, pickupObject, pickupObjectLoop, dropObject, turretAlert, turretDead;
    public AudioSource[] portalEnter;
    public AudioSource[] turretAlarmVoice;

    public static AudioController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void Play(AudioSource audio)
    {
        audio.Play();
    }

    public void PlayOneShot(AudioSource audio)
    {
        audio.PlayOneShot(audio.clip);
    }

    public void Stop(AudioSource audio)
    {
        audio.Stop();

    }

    public void Pause(AudioSource audio)
    {
        audio.Pause();
    }
}
