using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openableHandler : MonoBehaviour
{
    public bool isOpen;
    public bool canOpen;
    public bool autoClose;
    public Animator anim;
    public Transform iconPoint;
    public float closeAfter = 10;
    float timer;
    float cooldownTimer;
    public float cooldown = .5f;
    public AudioClip openSound;
    public AudioSource audioSrc;

    public void interaction()
    {
        if (canOpen && cooldownTimer >= cooldown)
        {
            if (isOpen)
            {
                anim.SetTrigger("close");
                isOpen = false;
                cooldownTimer = 0;
            }
            else
            {
                anim.SetTrigger("open");
                isOpen = true;
                audioSrc.clip = openSound;
                audioSrc.Play();
                cooldownTimer = 0;
            }
        }
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (autoClose && isOpen && canOpen)
        {
            timer += Time.deltaTime;
            if (timer >= closeAfter)
            {
                interaction();
                timer = 0;
            }
        }
    }
}
