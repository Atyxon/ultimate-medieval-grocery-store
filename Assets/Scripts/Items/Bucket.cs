using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    public GameObject water;
    public PlayerInventory inv;
    public ToolItem bucket;
    private void OnEnable()
    {
        bucket = (ToolItem)inv.quickbarSlots[inv.selectedSlot].item;
        CheckWaterLevel();
    }
    public void CheckWaterLevel()
    {
        if (bucket.fillLevel > 0)
        {
            water.SetActive(true);
        }
        else
        {
            water.SetActive(false);
        }
    }
}
