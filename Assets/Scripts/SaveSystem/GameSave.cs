using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSave
{
    public int money;
    public float[] position;
    public float playerYrotation;

    public int[] level;
    public int[] exp;
    public int[] expMax;

    public float time;
    public int day;

    public List<string> sellableItemName = new List<string>();
    public List<float> sellableItemPositionX = new List<float>();
    public List<float> sellableItemPositionY = new List<float>();
    public List<float> sellableItemPositionZ = new List<float>();
    public List<float> sellableItemRotationX = new List<float>();
    public List<float> sellableItemRotationY = new List<float>();
    public List<float> sellableItemRotationZ = new List<float>();

    public List<int> pickableItemId = new List<int>();
    public List<float> pickableItemPositionX = new List<float>();
    public List<float> pickableItemPositionY = new List<float>();
    public List<float> pickableItemPositionZ = new List<float>();
    public List<float> pickableItemRotationX = new List<float>();
    public List<float> pickableItemRotationY = new List<float>();
    public List<float> pickableItemRotationZ = new List<float>();
    public List<int> pickableItemQuantity = new List<int>();

    public List<string> buildingName = new List<string>();
    public List<float> buildingPositionX = new List<float>();
    public List<float> buildingPositionY = new List<float>();
    public List<float> buildingPositionZ = new List<float>();

    public bool[] slotIsEmpty;
    public int[] slotItemId;
    public int[] slotItemQuantity;
    public bool[] quickbarSlotIsEmpty;
    public int[] quickbarSlotItemId;
    public int[] quickbarSlotItemQuantity;
    public GameSave(PlayerInventory playerInventory, TimeManager timeManager, SaveLoadManager saveLoadMng)
    {
        //PLAYER
        money = playerInventory.playerMoney;
        position = new float[3];
        position[0] = playerInventory.transform.position.x;
        position[1] = playerInventory.transform.position.y;
        position[2] = playerInventory.transform.position.z;
        playerYrotation = playerInventory.transform.rotation.eulerAngles.y;

        //LEVELS
        level = new int[5];
        exp = new int[5];
        expMax = new int[5];
        for (int i = 0; i < playerInventory.levelMng.levels.Length; i++)
        {
            level[i] = playerInventory.levelMng.levels[i].levelInt;
            exp[i] = playerInventory.levelMng.levels[i].exp;
            expMax[i] = playerInventory.levelMng.levels[i].expMax;
        }

        //WEATHER & TIME
        time = timeManager.time;
        day = timeManager.day;

        //SELLABLE ITEMS

        for (int i = 0; i < saveLoadMng.sellableList.Count; i++)
        {
            if (saveLoadMng.sellableList[i] != null)
            {
                sellableItemName.Add(saveLoadMng.sellableList[i].parent.name);
                sellableItemPositionX.Add(saveLoadMng.sellableList[i].parent.transform.position.x);
                sellableItemPositionY.Add(saveLoadMng.sellableList[i].parent.transform.position.y);
                sellableItemPositionZ.Add(saveLoadMng.sellableList[i].parent.transform.position.z);

                sellableItemRotationX.Add(saveLoadMng.sellableList[i].transform.rotation.eulerAngles.x);
                sellableItemRotationY.Add(saveLoadMng.sellableList[i].transform.rotation.eulerAngles.y);
                sellableItemRotationZ.Add(saveLoadMng.sellableList[i].transform.rotation.eulerAngles.z);
            }
        }

        //PICKABLE ITEMS
        for (int i = 0; i < saveLoadMng.pickableList.Count; i++)
        {
            if (saveLoadMng.pickableList[i] != null)
            {
                pickableItemId.Add(saveLoadMng.pickableList[i].instanceItem.tiemId);
                pickableItemPositionX.Add(saveLoadMng.pickableList[i].transform.position.x);
                pickableItemPositionY.Add(saveLoadMng.pickableList[i].transform.position.y);
                pickableItemPositionZ.Add(saveLoadMng.pickableList[i].transform.position.z);

                pickableItemRotationX.Add(saveLoadMng.pickableList[i].transform.rotation.eulerAngles.x);
                pickableItemRotationY.Add(saveLoadMng.pickableList[i].transform.rotation.eulerAngles.y);
                pickableItemRotationZ.Add(saveLoadMng.pickableList[i].transform.rotation.eulerAngles.z);

                if (saveLoadMng.pickableList[i].instanceItem.GetType() == typeof(SeedItem))
                {
                    SeedItem item = (SeedItem)saveLoadMng.pickableList[i].instanceItem;
                    pickableItemQuantity.Add(item.quantity);
                }
                else
                {
                    pickableItemQuantity.Add(0);
                }
            }
        }

        //BUILDINGS
        for (int i = 0; i < saveLoadMng.buildingList.Count; i++)
        {
            if (saveLoadMng.buildingList[i] != null)
            {
                buildingName.Add(saveLoadMng.buildingList[i].name);
                buildingPositionX.Add(saveLoadMng.buildingList[i].transform.position.x);
                buildingPositionY.Add(saveLoadMng.buildingList[i].transform.position.y);
                buildingPositionZ.Add(saveLoadMng.buildingList[i].transform.position.z);
            }
        }

        //INVENTORY
        slotIsEmpty = new bool[30];
        slotItemId = new int[30];
        slotItemQuantity = new int[30];

        quickbarSlotIsEmpty = new bool[6];
        quickbarSlotItemId = new int[6];
        quickbarSlotItemQuantity = new int[6];

        for (int i = 0; i < playerInventory.slots.Length; i++)
        {
            if (playerInventory.slots[i].item != null)
            {
                slotItemId[i] = playerInventory.slots[i].item.tiemId;
                if (playerInventory.slots[i].item.GetType() == typeof(SeedItem))
                {
                    SeedItem seedItemToSave = (SeedItem)playerInventory.slots[i].item;
                    slotItemQuantity[i] = seedItemToSave.quantity;
                }
            }
            else
            {
                slotIsEmpty[i] = true;
            }
        }

        for (int i = 0; i < playerInventory.quickbarSlots.Length; i++)
        {
            if (playerInventory.quickbarSlots[i].item != null)
            {
                quickbarSlotItemId[i] = playerInventory.quickbarSlots[i].item.tiemId;
                if (playerInventory.quickbarSlots[i].item.GetType() == typeof(SeedItem))
                {
                    SeedItem seedItemToSave = (SeedItem)playerInventory.quickbarSlots[i].item;
                    quickbarSlotItemQuantity[i] = seedItemToSave.quantity;
                }
            }
            else
            {
                quickbarSlotIsEmpty[i] = true;
            }
        }
    }
}
