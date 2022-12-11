using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ambientSoundManager : MonoBehaviour
{
    public AudioClip[] ambientSounds;
    public AudioSource audioSrc;
    public bool playerInside;
    private void FixedUpdate()
    {
        if (playerInside)
        {
            audioSrc.volume = Mathf.MoveTowards(audioSrc.volume, .2f, Time.fixedDeltaTime);
        }
        else
        {
            audioSrc.volume = Mathf.MoveTowards(audioSrc.volume, 1f, Time.fixedDeltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInside = false;
        }
    }
}
