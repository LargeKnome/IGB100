using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource sfx;

    public static AudioManager i;

    private void Awake()
    {
        i = this;
    }

    public void PlaySFX(AudioClip sound)
    {
        sfx.PlayOneShot(sound);
    }

    public void ChangeBGM(AudioClip music)
    {
        bgm.clip = music;
        bgm.Play();
    }
}
