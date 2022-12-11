using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTrigger : MonoBehaviour
{
    public PlayerInventory playerInv;
    public CharacterController playerCont;
    public bool playerInZone;
    public float timer;
    public float timerToReturn = 5;
    private void Start()
    {
        playerInv = FindObjectOfType<PlayerInventory>();
        playerCont = FindObjectOfType<CharacterController>();
    }
    private void FixedUpdate()
    {
        if (playerInZone)
        {
            playerInv.colorAdj.saturation.value = Mathf.Lerp((float)playerInv.colorAdj.saturation, -100, 2 * Time.fixedDeltaTime);
            timer += Time.fixedDeltaTime;
            playerInv.outsideZoneText.text = "<color=orange>Go Back To Youre Store\n<size=50>" + (timerToReturn - timer).ToString("0.0");
            if (timerToReturn - timer <= 0)
            {
                playerCont.enabled = false;
                playerInv.gameObject.transform.position = playerInv.sleep.wakePoint.position;
                playerCont.enabled = true;
                timer = 0;
                playerInZone = false;
                playerInv.outsideZoneText.gameObject.SetActive(false);
            }
        }
        else
        {
            playerInv.colorAdj.saturation.value = Mathf.Lerp((float)playerInv.colorAdj.saturation, 0, 2 * Time.fixedDeltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInZone = true;
            playerInv.outsideZoneText.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInZone = false;
            playerInv.outsideZoneText.gameObject.SetActive(false);
            timer = 0;
        }
    }
}
