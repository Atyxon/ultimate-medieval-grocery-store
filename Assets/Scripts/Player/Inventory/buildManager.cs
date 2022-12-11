using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildManager : MonoBehaviour
{
    public levelManager levelManager;
    public GameObject ghostHolder;
    public ghost[] ghosts;
    public PlayerInventory inventory;
    public int rayDistance;
    public LayerMask groundLayerMask;
    bool isGhostSpawned;
    public float buildCooldown;
    float timer;
    int ghostIndex;

    private void Update()
    {
        if (inventory.isSlotSelected)
        {
            if(timer < buildCooldown)
                timer += Time.deltaTime;
            if (inventory.quickbarSlots[inventory.selectedSlot].item != null && inventory.quickbarSlots[inventory.selectedSlot].item.GetType() == typeof(SeedItem))
            {
                SeedItem seedObject = (SeedItem)inventory.quickbarSlots[inventory.selectedSlot].item;
                RaycastHit hit;
                if (Physics.Raycast(inventory.playerCamera.transform.position, inventory.playerCamera.transform.forward, out hit, rayDistance, groundLayerMask) && timer >= buildCooldown)
                {
                    if (!isGhostSpawned)
                    {
                        for (int i = 0; i < ghosts.Length; i++)
                        {
                            if (ghosts[i].name == seedObject.plantGhost.name)
                            {
                                ghosts[i].gameObject.SetActive(true);
                                isGhostSpawned = true;
                                ghostIndex = i;
                            }
                        }
                    }
                    else
                    {
                        if (inventory.ctrls.Player.LMB.triggered && ghosts[ghostIndex].canBuild)
                        {
                            GameObject plantedObj = Instantiate(seedObject.objectToInstantiate, ghostHolder.transform.position, ghostHolder.transform.rotation);
                            plantedObj.transform.localRotation = Quaternion.Euler(plantedObj.transform.localRotation.x, Random.Range(0, 360), plantedObj.transform.localRotation.y);
                            inventory.playerArmsAnim.SetTrigger("plant");
                            timer = 0;
                            inventory.Builded();
                            levelManager.addExp("Building", 5);
                            levelManager.addExp("Planting", 10);
                        }
                    }
                    ghostHolder.transform.position = hit.point;
                }
                else
                {
                    for (int i = 0; i < ghosts.Length; i++)
                    {
                        ghosts[i].gameObject.SetActive(false);
                    }
                    isGhostSpawned = false;
                }
            }
            else
            {
                for (int i = 0; i < ghosts.Length; i++)
                {
                    ghosts[i].gameObject.SetActive(false);
                }
                isGhostSpawned = false;
            }
        }
        else
        {
            for (int i = 0; i < ghosts.Length; i++)
            {
                ghosts[i].gameObject.SetActive(false);
            }
            isGhostSpawned = false;
        }
    }
}
