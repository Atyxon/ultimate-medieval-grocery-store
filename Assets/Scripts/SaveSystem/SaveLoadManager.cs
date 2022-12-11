using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public CharacterController playerCont;
    public TimeManager timeMng;
    public GameObject firstTimePlayBonus;
    [Space]
    public LoadSaveTrigger loadMng;
    [Space]
    public Item[] itemList;
    public GameObject[] sellablePrefabList;
    public GameObject[] buildingPrefabList;
    [Space]
    public List<ItemPickable> pickableList = new List<ItemPickable>();
    public List<sellableObject> sellableList = new List<sellableObject>();
    public List<GameObject> buildingList = new List<GameObject>();
    public void Start()
    {
        loadMng = FindObjectOfType<LoadSaveTrigger>();
        if (loadMng != null)
        {
            loadMng.LoadData();
            if (loadMng.loadSaveBool)
            {
                LoadSave();
                firstTimePlayBonus.SetActive(false);
            }
            else
            {
                Save();
                firstTimePlayBonus.SetActive(true);
            }
        }
    }
    public void Save()
    {
        sellableObject[] sellableArray = FindObjectsOfType<sellableObject>();
        ItemPickable[] pickableArray = FindObjectsOfType<ItemPickable>();

        plant[] plantArray = FindObjectsOfType<plant>();
        sellableList.Clear();
        pickableList.Clear();
        buildingList.Clear();
        for (int i = 0; i < pickableArray.Length; i++)
        {
            pickableList.Add(pickableArray[i]);
        }
        for (int i = 0; i < sellableArray.Length; i++)
        {
            sellableList.Add(sellableArray[i]);
        }
        for (int i = 0; i < plantArray.Length; i++)
        {
            buildingList.Add(plantArray[i].gameObject);
        }
        SaveSystem.Save(loadMng.saveName, playerInventory, timeMng, this);
        loadMng.SaveData((int)timeMng.time, timeMng.day, playerInventory.levelMng.levels[0].levelInt);
    }
    public void LoadSave()
    {
        GameSave data = SaveSystem.LoadSave(loadMng.saveName);

        playerInventory.playerMoney = data.money;
        playerCont.enabled = false;
        playerInventory.gameObject.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
        playerInventory.gameObject.transform.rotation = Quaternion.Euler(0,data.playerYrotation,0);
        playerCont.enabled = true;

        for (int i = 0; i < playerInventory.levelMng.levels.Length; i++)
        {
            playerInventory.levelMng.levels[i].levelInt = data.level[i];
            playerInventory.levelMng.levels[i].exp = data.exp[i];
            playerInventory.levelMng.levels[i].expMax = data.expMax[i];
        }
        playerInventory.levelMng.initLevels();

        timeMng.time = data.time;
        timeMng.day = data.day;

        for (int i = 0; i < playerInventory.slots.Length; i++)
        {
            if (!data.slotIsEmpty[i])
            {
                Item loadedItem = Instantiate(itemList[data.slotItemId[i]]);
                if (loadedItem.GetType() == typeof(SeedItem))
                {
                    SeedItem loadedSeedItem = (SeedItem)loadedItem;
                    loadedSeedItem.quantity = data.slotItemQuantity[i];
                }

                playerInventory.slots[i].item = loadedItem;
                playerInventory.UpdateSlot(i, 0);
            }
        }
        for (int i = 0; i < playerInventory.quickbarSlots.Length; i++)
        {
            if (!data.quickbarSlotIsEmpty[i])
            {
                Item loadedItem = Instantiate(itemList[data.quickbarSlotItemId[i]]);
                if (loadedItem.GetType() == typeof(SeedItem))
                {
                    SeedItem loadedSeedItem = (SeedItem)loadedItem;
                    loadedSeedItem.quantity = data.quickbarSlotItemQuantity[i];
                }

                playerInventory.quickbarSlots[i].item = loadedItem;
                playerInventory.UpdateSlot(i, 1);
            }
        }

        //WORLD ITEMS
        for (int i = 0; i < data.pickableItemId.Count; i++)
        {
            for (int z = 0; z < itemList.Length; z++)
            {
                if (data.pickableItemId[i] == itemList[z].tiemId)
                {
                    GameObject item = Instantiate(itemList[z].itemPrefab);
                    item.transform.position = new Vector3(data.pickableItemPositionX[i], data.pickableItemPositionY[i], data.pickableItemPositionZ[i]);
                    item.transform.rotation = Quaternion.Euler(data.pickableItemRotationX[i], data.pickableItemRotationY[i], data.pickableItemRotationZ[i]);

                    if (itemList[z].GetType() == typeof(SeedItem))
                    {
                        ItemPickable newItem = item.GetComponent<ItemPickable>();
                        SeedItem seedItem = (SeedItem)newItem.instanceItem;
                        seedItem.quantity = data.pickableItemQuantity[i];
                    }
                    break;
                }
            }
        }
        for (int i = 0; i < data.sellableItemName.Count; i++)
        {
            for (int z = 0; z < sellablePrefabList.Length; z++)
            {
                if (data.sellableItemName[i] == sellablePrefabList[z].name)
                {
                    GameObject sellableItem = Instantiate(sellablePrefabList[z]);
                    sellableObjectParent sellParent = sellableItem.GetComponent<sellableObjectParent>();
                    sellableItem.transform.position = new Vector3(data.sellableItemPositionX[i], data.sellableItemPositionY[i], data.sellableItemPositionZ[i]);
                    sellableItem.transform.rotation = Quaternion.Euler(data.sellableItemRotationX[i], data.sellableItemRotationY[i], data.sellableItemRotationZ[i]);
                    sellParent.childObj.EnableColliders();
                }
            }
        }
        for (int i = 0; i < data.buildingName.Count; i++)
        {
            for (int z = 0; z < buildingPrefabList.Length; z++)
            {
                if (data.buildingName[i] == buildingPrefabList[z].name)
                {
                    GameObject buildingItem = Instantiate(buildingPrefabList[z]);
                    buildingItem.transform.position = new Vector3(data.buildingPositionX[i], data.buildingPositionY[i], data.buildingPositionZ[i]);
                }
            }
        }
    }
}
