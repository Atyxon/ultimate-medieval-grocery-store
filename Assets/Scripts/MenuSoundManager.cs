using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] coinSounds;
    public AudioClip[] clickSound;
    public void PlayCoin()
    {
        int clip = Random.Range(0, coinSounds.Length+1);
        audioSource.clip = coinSounds[clip];
        audioSource.Play();
    }
    public void PlayClick(int click)
    {
        audioSource.clip = clickSound[click];
        audioSource.Play();
    }
}
