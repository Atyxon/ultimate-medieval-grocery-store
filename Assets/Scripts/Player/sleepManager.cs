using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sleepManager : MonoBehaviour
{
    public TimeManager timeMng;
    public PlayerInventory playerInv;
    public Transform cam;
    public Animator sleepAnim;
    [Space]
    public MovementController moveControl;
    public CameraController cameraControl;
    public Transform sleepPoint;
    public Transform wakePoint;
    public Vector3 playerSleepRot;
    public Vector3 cameraSleepRot;
    public bool sleeping;
    float timer;
    public void GoSleep()
    {
        moveControl.canMove = false;
        cameraControl.isEnabled = false;
        sleeping = true;
    }
    private void FixedUpdate()
    {
        if (sleeping)
        {
            timer += Time.deltaTime;
            playerInv.gameObject.transform.position = Vector3.Lerp(playerInv.gameObject.transform.position, sleepPoint.position, 3  * Time.fixedDeltaTime);
            playerInv.gameObject.transform.rotation = Quaternion.Lerp(playerInv.gameObject.transform.rotation, Quaternion.Euler(playerSleepRot), 3 * Time.fixedDeltaTime);
            cam.localRotation = Quaternion.Lerp(cam.localRotation, Quaternion.Euler(cameraSleepRot), 3 * Time.fixedDeltaTime);
            if (timer >= 2 && timer < 4)
            {
                sleepAnim.gameObject.SetActive(true);
                sleepAnim.Play("sleep");
            }
            else if (timer >= 4)
            {
                endSleeping();
            }
        }
    }
    public void endSleeping()
    {
        timer = 0;
        playerInv.gameObject.transform.position = wakePoint.position;
        cameraControl.xRotation = 0;
        moveControl.canMove = true;
        cameraControl.isEnabled = true; 
        sleeping = false;
        sleepAnim.gameObject.SetActive(false);
        sleepAnim.gameObject.SetActive(true);
        if (timeMng.time > 1200)
        {
            timeMng.day++;
            timeMng.time = 420;
            playerInv.startAnim.SetActive(false);
            playerInv.startAnim.SetActive(true);
        }
        else if (timeMng.time < 240)
        {
            timeMng.time = 420;
            playerInv.startAnim.SetActive(false);
            playerInv.startAnim.SetActive(true);
        }
    }
}
