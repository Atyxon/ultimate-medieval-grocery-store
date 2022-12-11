using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
    public PlayerInventory inventory;

    public void HideEnd()
    {
        inventory.ChangeSelectedSlot(inventory.nextSlot);
    }
}
