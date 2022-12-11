using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropManager : MonoBehaviour, IDropHandler
{
    public PlayerInventory inventory;
    public Transform dropPoint;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DragDrop draggedItem = eventData.pointerDrag.GetComponent<DragDrop>();
            if (draggedItem.slot == inventory.clickedSlot)
            {
                inventory.selectedItemInfoSeed.SetActive(false);
                inventory.selectedItemInfoTool.SetActive(false);
            }

            bool dropped = false;
            //INV ITEM DROPPED
            if (!dropped)
            {
                for (int i = 0; i < draggedItem.slot.inventorySys.slots.Length; i++)
                {
                    if (draggedItem.slot.inventorySys.slots[i].invSlot == draggedItem.slot)
                    {
                        GameObject droppedItem = Instantiate(draggedItem.slot.inventorySys.slots[i].item.itemPrefab, dropPoint.position, dropPoint.rotation);
                        Item item = droppedItem.GetComponent<ItemPickable>().instanceItem;

                        if (draggedItem.slot.inventorySys.slots[i].item.GetType() == typeof(SeedItem))
                        {
                            SeedItem instanceItem = (SeedItem)item;
                            SeedItem removedItem = (SeedItem)draggedItem.slot.inventorySys.slots[i].item;
                            Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
                            rb.AddRelativeForce(Vector3.forward * 100);

                            instanceItem.quantity = removedItem.quantity;
                        }
                        draggedItem.slot.inventorySys.slots[i].item = null;
                        draggedItem.slot.inventorySys.UpdateSlot(i, 0);
                    }
                }
            }
            //QCB ITEM DROPPED
            if (!dropped)
            {
                for (int i = 0; i < draggedItem.slot.inventorySys.quickbarSlots.Length; i++)
                {
                    if (draggedItem.slot.inventorySys.quickbarSlots[i].invSlot == draggedItem.slot)
                    {
                        GameObject droppedItem = Instantiate(draggedItem.slot.inventorySys.quickbarSlots[i].item.itemPrefab, dropPoint.position, dropPoint.rotation);
                        Item item = droppedItem.GetComponent<ItemPickable>().instanceItem;

                        if (draggedItem.slot.inventorySys.quickbarSlots[i].item.GetType() == typeof(SeedItem))
                        {
                            SeedItem instanceItem = (SeedItem)item;
                            SeedItem removedItem = (SeedItem)draggedItem.slot.inventorySys.quickbarSlots[i].item;

                            instanceItem.quantity = removedItem.quantity;
                        }
                        draggedItem.slot.inventorySys.quickbarSlots[i].item = null;
                        draggedItem.slot.inventorySys.UpdateSlot(i, 1);

                        if (draggedItem.slot.inventorySys.selectedSlot == i)
                        {
                            draggedItem.slot.inventorySys.ChangeSelectedSlot(i);
                        }
                    }
                }
            }
        }
    }
}
