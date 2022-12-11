using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playDoorsClosedSound : MonoBehaviour
{
    public AudioSource audioSrc;
    public void PlayClosedDoorSound()
    {
        audioSrc.Play();
    }
}
