using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DragDrop : MonoBehaviour, IDropHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public InventorySlot slot;
    public bool isQuickbarSlot;
    public int indexInArray;
    public enum SlotType { Inventory, Quickbar, Tool_Fuel, Tool_Attachment};
    public SlotType slotType;

    [SerializeField] Canvas canvas;
    private RectTransform rt;
    private CanvasGroup canvGrp;
    float originSize; 
    private void Awake()
    {
        originSize = transform.localScale.x;
        indexInArray = slot.transform.GetSiblingIndex();
        rt = GetComponent<RectTransform>();
        canvGrp = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.localScale = new Vector3(originSize,originSize,originSize);
        if (!isQuickbarSlot)
        {
            transform.parent = slot.transform.parent.parent.parent;
        }
        else
        {
            transform.parent = slot.transform.parent.parent;
        }
        canvGrp.blocksRaycasts = false;
        transform.SetSiblingIndex(100);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvGrp.blocksRaycasts = true;
        transform.parent = slot.transform;
        rt.localPosition = Vector3.zero;
        transform.localScale = new Vector3(originSize, originSize, originSize);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DragDrop draggedItem = eventData.pointerDrag.GetComponent<DragDrop>();
            if (draggedItem.slot.sprite.sprite != draggedItem.slot.inventorySys.emptySprite)
            {
                //INV >> INV
                if (draggedItem.slotType == SlotType.Inventory && slotType == SlotType.Inventory)
                {
                    if (slot.inventorySys.slots[indexInArray].item != null && draggedItem.slot.inventorySys.slots[draggedItem.indexInArray].item.tiemId == slot.inventorySys.slots[indexInArray].item.tiemId)
                    {
                        if (draggedItem.slot.inventorySys.slots[draggedItem.indexInArray].item.GetType() == typeof(SeedItem))
                        {
                            SeedItem seedDragged = (SeedItem)draggedItem.slot.inventorySys.slots[draggedItem.indexInArray].item;
                            SeedItem seedDroppedIn = (SeedItem)slot.inventorySys.slots[indexInArray].item;
                            int a = seedDragged.quantity;
                            int b = seedDroppedIn.quantity;

                            if (b < seedDroppedIn.maxQuantity)
                            {
                                if (b + a <= seedDroppedIn.maxQuantity)
                                {
                                    seedDroppedIn.quantity += a;
                                    seedDragged.quantity = 0;
                                    Destroy(seedDragged);
                                    draggedItem.slot.inventorySys.slots[draggedItem.indexInArray].item = null;
                                }
                                else if (b + a > seedDroppedIn.maxQuantity)
                                {
                                    int diffrence = seedDroppedIn.maxQuantity - seedDroppedIn.quantity;
                                    seedDroppedIn.quantity = seedDroppedIn.maxQuantity;
                                    seedDragged.quantity -= diffrence;
                                }
                            }
                        }
                    }
                    else
                    {
                        Item newItem = slot.inventorySys.slots[draggedItem.indexInArray].item;

                        slot.inventorySys.slots[draggedItem.indexInArray].item = slot.inventorySys.slots[indexInArray].item;
                        slot.inventorySys.slots[indexInArray].item = newItem;
                    }
                    slot.inventorySys.UpdateSlot(draggedItem.indexInArray, 0);
                    slot.inventorySys.UpdateSlot(indexInArray, 0);
                }
                //INV >> QCB
                else if (draggedItem.slotType == SlotType.Inventory && slotType == SlotType.Quickbar)
                {
                    if (slot.inventorySys.quickbarSlots[indexInArray].item != null && draggedItem.slot.inventorySys.slots[draggedItem.indexInArray].item.tiemId == slot.inventorySys.quickbarSlots[indexInArray].item.tiemId)
                    {
                        if (draggedItem.slot.inventorySys.slots[draggedItem.indexInArray].item.GetType() == typeof(SeedItem))
                        {
                            SeedItem seedDragged = (SeedItem)draggedItem.slot.inventorySys.slots[draggedItem.indexInArray].item;
                            SeedItem seedDroppedIn = (SeedItem)slot.inventorySys.quickbarSlots[indexInArray].item;
                            int a = seedDragged.quantity;
                            int b = seedDroppedIn.quantity;

                            if (b < seedDroppedIn.maxQuantity)
                            {
                                if (b + a <= seedDroppedIn.maxQuantity)
                                {
                                    seedDroppedIn.quantity += a;
                                    seedDragged.quantity = 0;
                                    Destroy(seedDragged);
                                    draggedItem.slot.inventorySys.slots[draggedItem.indexInArray].item = null;
                                }
                                else if (b + a > seedDroppedIn.maxQuantity)
                                {
                                    int diffrence = seedDroppedIn.maxQuantity - seedDroppedIn.quantity;
                                    seedDroppedIn.quantity = seedDroppedIn.maxQuantity;
                                    seedDragged.quantity -= diffrence;
                                }
                            }
                        }
                    }
                    else
                    {
                        Item newItem = slot.inventorySys.slots[draggedItem.indexInArray].item;
                        if (newItem.GetType() == typeof(SeedItem))
                        {

                            if (draggedItem.slot.inventorySys.selectedSlot == indexInArray && draggedItem.slot.inventorySys.isSlotSelected)
                            {
                                draggedItem.slot.inventorySys.ChangeSelectedSlot(indexInArray);
                            }
                            slot.inventorySys.slots[draggedItem.indexInArray].item = slot.inventorySys.quickbarSlots[indexInArray].item;
                            slot.inventorySys.quickbarSlots[indexInArray].item = newItem;
                        }
                        else if (newItem.GetType() == typeof(ToolItem))
                        {
                            if (draggedItem.slot.inventorySys.selectedSlot == indexInArray && draggedItem.slot.inventorySys.isSlotSelected)
                            {
                                draggedItem.slot.inventorySys.ChangeSelectedSlot(indexInArray);
                            }
                            slot.inventorySys.slots[draggedItem.indexInArray].item = slot.inventorySys.quickbarSlots[indexInArray].item;
                            slot.inventorySys.quickbarSlots[indexInArray].item = newItem;
                        }
                    }
                    slot.inventorySys.UpdateSlot(draggedItem.indexInArray, 0);
                    slot.inventorySys.UpdateSlot(indexInArray, 1);
                }
                //QCB >> QCB
                else if (draggedItem.slotType == SlotType.Quickbar && slotType == SlotType.Quickbar)
                {
                    if (slot.inventorySys.quickbarSlots[indexInArray].item != null && draggedItem.slot.inventorySys.quickbarSlots[draggedItem.indexInArray].item.tiemId == slot.inventorySys.quickbarSlots[indexInArray].item.tiemId)
                    {
                        if (draggedItem.slot.inventorySys.quickbarSlots[draggedItem.indexInArray].item.GetType() == typeof(SeedItem))
                        {
                            SeedItem seedDragged = (SeedItem)draggedItem.slot.inventorySys.quickbarSlots[draggedItem.indexInArray].item;
                            SeedItem seedDroppedIn = (SeedItem)slot.inventorySys.quickbarSlots[indexInArray].item;
                            int a = seedDragged.quantity;
                            int b = seedDroppedIn.quantity;

                            if (b < seedDroppedIn.maxQuantity)
                            {
                                if (b + a <= seedDroppedIn.maxQuantity)
                                {
                                    seedDroppedIn.quantity += a;
                                    seedDragged.quantity = 0;
                                    Destroy(seedDragged);
                                    draggedItem.slot.inventorySys.quickbarSlots[draggedItem.indexInArray].item = null;
                                }
                                else if (b + a > seedDroppedIn.maxQuantity)
                                {
                                    int diffrence = seedDroppedIn.maxQuantity - seedDroppedIn.quantity;
                                    seedDroppedIn.quantity = seedDroppedIn.maxQuantity;
                                    seedDragged.quantity -= diffrence;
                                }
                            }
                        }
                    }
                    else
                    {
                        Item newItem = slot.inventorySys.quickbarSlots[draggedItem.indexInArray].item;

                        slot.inventorySys.quickbarSlots[draggedItem.indexInArray].item = slot.inventorySys.quickbarSlots[indexInArray].item;
                        slot.inventorySys.quickbarSlots[indexInArray].item = newItem;
                    }

                    slot.inventorySys.UpdateSlot(draggedItem.indexInArray, 1);
                    slot.inventorySys.UpdateSlot(indexInArray, 1);
                    draggedItem.slot.inventorySys.ChangeSelectedSlot(draggedItem.indexInArray);
                }
                //QCB >> INV
                else if (draggedItem.slotType == SlotType.Quickbar && slotType == SlotType.Inventory)
                {
                    if (slot.inventorySys.slots[indexInArray].item != null && draggedItem.slot.inventorySys.quickbarSlots[draggedItem.indexInArray].item.tiemId == slot.inventorySys.slots[indexInArray].item.tiemId)
                    {
                        if (draggedItem.slot.inventorySys.quickbarSlots[draggedItem.indexInArray].item.GetType() == typeof(SeedItem))
                        {
                            SeedItem seedDragged = (SeedItem)draggedItem.slot.inventorySys.quickbarSlots[draggedItem.indexInArray].item;
                            SeedItem seedDroppedIn = (SeedItem)slot.inventorySys.slots[indexInArray].item;
                            int a = seedDragged.quantity;
                            int b = seedDroppedIn.quantity;

                            if (b < seedDroppedIn.maxQuantity)
                            {
                                if (b + a <= seedDroppedIn.maxQuantity)
                                {
                                    seedDroppedIn.quantity += a;
                                    seedDragged.quantity = 0;
                                    Destroy(seedDragged);
                                    draggedItem.slot.inventorySys.quickbarSlots[draggedItem.indexInArray].item = null;
                                }
                                else if (b + a > seedDroppedIn.maxQuantity)
                                {
                                    int diffrence = seedDroppedIn.maxQuantity - seedDroppedIn.quantity;
                                    seedDroppedIn.quantity = seedDroppedIn.maxQuantity;
                                    seedDragged.quantity -= diffrence;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (slot.inventorySys.slots[indexInArray].item == null || slot.inventorySys.slots[indexInArray].item.GetType() == typeof(SeedItem))
                        {
                            Item newItem = slot.inventorySys.quickbarSlots[draggedItem.indexInArray].item;

                            slot.inventorySys.quickbarSlots[draggedItem.indexInArray].item = slot.inventorySys.slots[indexInArray].item;
                            slot.inventorySys.slots[indexInArray].item = newItem;

                            if (draggedItem.slot.inventorySys.selectedSlot == draggedItem.indexInArray && draggedItem.slot.inventorySys.isSlotSelected)
                                draggedItem.slot.inventorySys.ChangeSelectedSlot(draggedItem.indexInArray);
                        }
                    }
                    slot.inventorySys.UpdateSlot(draggedItem.indexInArray, 1);
                    slot.inventorySys.UpdateSlot(indexInArray, 0);
                }

                //FUEL AND ATTACHMENTS

                //INV >> FUEL
                else if (draggedItem.slotType == SlotType.Inventory && slotType == SlotType.Tool_Fuel)
                {
                    if (draggedItem.slot.inventorySys.slots[draggedItem.indexInArray].item.GetType() == typeof(FuelItem))
                    {
                        InventorySlot selectedSlot = slot.inventorySys.clickedSlot;
                        if (selectedSlot.slotSprite.slotType == SlotType.Inventory)
                        {
                            if (slot.inventorySys.slots[selectedSlot.slotSprite.indexInArray].item.GetType() == typeof(ToolItem))
                            {
                                FuelItem newFuel = (FuelItem)slot.inventorySys.slots[draggedItem.indexInArray].item;
                                slot.inventorySys.slots[draggedItem.indexInArray].item = slot.inventorySys.fuelSlot.item;
                                ToolItem tool = (ToolItem)slot.inventorySys.slots[selectedSlot.slotSprite.indexInArray].item;
                                tool.loadedFuel = newFuel;
                                slot.inventorySys.UpdateSelectedItemWindow(slot.inventorySys.clickedSlot.slotSprite);
                            }

                            slot.inventorySys.UpdateSlot(draggedItem.indexInArray, 0);
                            slot.inventorySys.UpdateSlot(slot.inventorySys.clickedSlot.slotSprite.indexInArray, 0);
                        }
                        else if (selectedSlot.slotSprite.slotType == SlotType.Quickbar)
                        {
                            if (slot.inventorySys.quickbarSlots[selectedSlot.slotSprite.indexInArray].item.GetType() == typeof(ToolItem))
                            {
                                FuelItem newFuel = (FuelItem)slot.inventorySys.slots[draggedItem.indexInArray].item;
                                slot.inventorySys.slots[draggedItem.indexInArray].item = slot.inventorySys.fuelSlot.item;
                                ToolItem tool = (ToolItem)slot.inventorySys.quickbarSlots[selectedSlot.slotSprite.indexInArray].item;
                                tool.loadedFuel = newFuel;
                                slot.inventorySys.UpdateSelectedItemWindow(slot.inventorySys.clickedSlot.slotSprite);
                            }

                            slot.inventorySys.UpdateSlot(draggedItem.indexInArray, 0);
                            slot.inventorySys.UpdateSlot(slot.inventorySys.clickedSlot.slotSprite.indexInArray, 1);
                            CheckToolFuel();
                        }
                    }
                }
                //FUEL >> INV
                else if (draggedItem.slotType == SlotType.Tool_Fuel && slotType == SlotType.Inventory)
                {
                    for (int i = 0; i < slot.inventorySys.slots.Length; i++)
                    {
                        if (slot.inventorySys.clickedSlot == slot.inventorySys.slots[slot.inventorySys.clickedSlot.slotSprite.indexInArray].invSlot)
                        {
                            if (slot.inventorySys.slots[indexInArray].item == null)
                            {
                                ToolItem tool = (ToolItem)slot.inventorySys.slots[slot.inventorySys.clickedSlot.slotSprite.indexInArray].item;
                                Item newItem = tool.loadedFuel;
                                slot.inventorySys.slots[indexInArray].item = newItem;
                                tool.loadedFuel = null;
                                slot.inventorySys.UpdateSelectedItemWindow(slot.inventorySys.clickedSlot.slotSprite);

                                slot.inventorySys.UpdateSlot(indexInArray, 0);
                                slot.inventorySys.UpdateSlot(slot.inventorySys.clickedSlot.slotSprite.indexInArray, 0);
                                break;
                            }
                            else
                            {
                                if (slot.inventorySys.slots[indexInArray].item.GetType() == typeof(FuelItem))
                                {
                                    ToolItem tool = (ToolItem)slot.inventorySys.slots[slot.inventorySys.clickedSlot.slotSprite.indexInArray].item;
                                    Item newItem = tool.loadedFuel;
                                    tool.loadedFuel = (FuelItem)slot.inventorySys.slots[indexInArray].item;
                                    slot.inventorySys.slots[indexInArray].item = newItem;
                                    slot.inventorySys.UpdateSelectedItemWindow(slot.inventorySys.clickedSlot.slotSprite);

                                    slot.inventorySys.UpdateSlot(indexInArray, 0);
                                    slot.inventorySys.UpdateSlot(slot.inventorySys.clickedSlot.slotSprite.indexInArray, 0);
                                    break;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < slot.inventorySys.quickbarSlots.Length; i++)
                    {
                        if (slot.inventorySys.clickedSlot == slot.inventorySys.quickbarSlots[slot.inventorySys.clickedSlot.slotSprite.indexInArray].invSlot)
                        {
                            if (slot.inventorySys.slots[indexInArray].item == null)
                            {
                                ToolItem tool = (ToolItem)slot.inventorySys.quickbarSlots[slot.inventorySys.clickedSlot.slotSprite.indexInArray].item;
                                Item newItem = tool.loadedFuel;
                                slot.inventorySys.slots[indexInArray].item = newItem;
                                tool.loadedFuel = null;
                                slot.inventorySys.UpdateSelectedItemWindow(slot.inventorySys.clickedSlot.slotSprite);

                                slot.inventorySys.UpdateSlot(indexInArray, 0);
                                slot.inventorySys.UpdateSlot(slot.inventorySys.clickedSlot.slotSprite.indexInArray, 1);
                                CheckToolFuel();
                                break;
                            }
                            else
                            {
                                if (slot.inventorySys.slots[indexInArray].item.GetType() == typeof(FuelItem))
                                {
                                    ToolItem tool = (ToolItem)slot.inventorySys.quickbarSlots[slot.inventorySys.clickedSlot.slotSprite.indexInArray].item;
                                    print(tool);
                                    Item newItem = tool.loadedFuel;
                                    tool.loadedFuel = (FuelItem)slot.inventorySys.slots[indexInArray].item;
                                    slot.inventorySys.slots[indexInArray].item = newItem;
                                    slot.inventorySys.UpdateSelectedItemWindow(slot.inventorySys.clickedSlot.slotSprite);

                                    slot.inventorySys.UpdateSlot(indexInArray, 0);
                                    slot.inventorySys.UpdateSlot(slot.inventorySys.clickedSlot.slotSprite.indexInArray, 1);
                                    CheckToolFuel();
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (draggedItem.slot == slot.inventorySys.clickedSlot)
            {
                slot.inventorySys.CloseAllItemInfoPanels();
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (slotType != SlotType.Tool_Attachment && slotType != SlotType.Tool_Fuel)
        {
            DragDrop thisSlot = this;
            slot.inventorySys.UpdateSelectedItemWindow(thisSlot);
            slot.inventorySys.timer = 0;
        }
    }
    public void CheckToolFuel()
    {
        for (int i = 0; i < slot.inventorySys.objectsInHand.Length; i++)
        {
            if (slot.inventorySys.objectsInHand[i] != null && slot.inventorySys.objectsInHand[i].name == slot.inventorySys.quickbarSlots[slot.inventorySys.selectedSlot].item.itemName)
            {
                slot.inventorySys.objectsInHand[i].SetActive(true);
                if (slot.inventorySys.quickbarSlots[slot.inventorySys.selectedSlot].item.GetType() == typeof(ToolItem))
                {
                    ToolItem toolObject = (ToolItem)slot.inventorySys.quickbarSlots[slot.inventorySys.selectedSlot].item;
                    if (toolObject.useFuel)
                    {
                        Tool toolObjectInHand = slot.inventorySys.objectsInHand[i].GetComponent<Tool>();
                        if (toolObject.loadedFuel != null)
                            toolObjectInHand.SelectFuel(toolObject.loadedFuel.itemName);
                        else
                            toolObjectInHand.HideFuel();
                    }
                }
            }
        }
    }
}

