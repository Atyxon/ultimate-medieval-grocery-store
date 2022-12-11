using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class storeManager : MonoBehaviour
{
    public bool isStoreOpen;
    public PlayerInventory inventory;
    [Space]
    public Transform pointToOpenDoors;
    public Transform[] pointsInQueue;
    public CustomerAI[] customersInQueue;
    public openableHandler frontDoors;
    public Transform[] pointsToGoAway;
    public Transform exitPointPhase1;
    [Space]
    public int currentCustomersCount;
    public int maxCustomers;
    public float maxDistance;
    [Space]
    public GameObject messagesContainer;
    public GameObject message;
    public GameObject warningIcon;
    public TextMeshProUGUI messageUI;
    [Space]
    public CustomerAI customer;
    public List<sellableSerializable> goodsOnTable = new List<sellableSerializable>();
    List<bool> itemSatisfaction = new List<bool>();
    List<GameObject> objectsInTrigger = new List<GameObject>();
    Vector3 originSize;
    public bool customerInPlace;
    [Space]
    public goodsPricesSerializable[] prices;
    private void Start()
    {
        originSize = message.transform.localScale;
    }
    public void SetMessagePoistion(Vector3 point)
    {
        Vector3 screenPos = inventory.playerCamera.WorldToScreenPoint(point);
        float distance = Vector3.Distance(inventory.transform.position, point);
        message.transform.localScale = new Vector3(originSize.x / (distance / 2), originSize.y / (distance / 2), originSize.z / (distance / 2));

        if (screenPos.z > 0)
        {
            messagesContainer.SetActive(true);
        }
        else
        {
            messagesContainer.SetActive(false);
        }

        if (customer != null && customerInPlace)
        {
            if (distance >= maxDistance)
            {
                warningIcon.SetActive(true);
                message.SetActive(false);
                warningIcon.transform.position = screenPos;
            }
            else
            {
                warningIcon.SetActive(false);
                message.SetActive(true);
                message.transform.position = screenPos;
            }
        }
        else
        {
            warningIcon.SetActive(false);
            message.SetActive(false);
        }
    }
    public void NewCustomer(int items)
    {
        itemSatisfaction.Clear();
        for (int i = 0; i < items; i++)
        {
            itemSatisfaction.Add(false);
        }
    }
    public void Next()
    {
        for (int i = 1; i < customersInQueue.Length; i++)
        {
            if (customersInQueue[i] != null)
            {
                customersInQueue[i].agent.SetDestination(pointsInQueue[i - 1].position);
                customersInQueue[i - 1] = customersInQueue[i];
                customersInQueue[i] = null;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "sellable")
        {
            objectsInTrigger.Add(other.gameObject);
            sellableObject sellable = other.GetComponent<sellableObject>();
            bool itemFound = false;

            for (int i = 0; i < goodsOnTable.Count; i++)
            {
                if ((int)goodsOnTable[i].sellableType == (int)sellable.item.sellableType)
                {
                    goodsOnTable[i].quantity++;
                    itemFound = true;
                }
            }
            if (!itemFound)
            {
                sellableSerializable obj = new sellableSerializable
                {
                    quantity = sellable.item.quantity,
                    sellableType = sellable.item.sellableType
                };
                goodsOnTable.Add(obj);
            }

            for (int o = 0; o < customer.demandingObjects.Count; o++)
            {
                for (int i = 0; i < goodsOnTable.Count; i++)
                {
                    if (customer.demandingObjects[o].sellableType == goodsOnTable[i].sellableType && customer.demandingObjects[o].quantity <= goodsOnTable[i].quantity)
                    {
                        itemSatisfaction[o] = true;
                    }
                }
            }
            bool satisfied = true;
            for (int i = 0; i < itemSatisfaction.Count; i++)
            {
                if (itemSatisfaction[i] == false)
                {
                    satisfied = false;
                }
            }
            if(satisfied)
            {
                int finalPrice = 0;
                for (int i = 0; i < customer.demandingObjects.Count; i++)
                {
                    finalPrice += customer.demandingObjects[i].quantity * prices[(int)customer.demandingObjects[i].sellableType].priceBase;
                }

                for (int i = 0; i < objectsInTrigger.Count; i++)
                {
                    Destroy(objectsInTrigger[i]);
                }
                objectsInTrigger.Clear();
                goodsOnTable.Clear();
                customer.BoughtGoods();
                inventory.UpdateMoney(finalPrice);
                inventory.MoneyPopUp(finalPrice);
                inventory.levelMng.addExp("Service", 10);
                inventory.levelMng.addExp("Player", 4);
                inventory.levelMng.addExp("Management", finalPrice*2);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "sellable")
        {
            sellableObject sellable = other.GetComponent<sellableObject>();
            objectsInTrigger.Remove(other.gameObject);
            for (int i = 0; i < goodsOnTable.Count; i++)
            {
                if (goodsOnTable[i].sellableType == sellable.item.sellableType)
                {
                    if (goodsOnTable[i].quantity > 1)
                    {
                        goodsOnTable[i].quantity--;
                    }
                    else
                    {
                        goodsOnTable.Remove(goodsOnTable[i]);
                    }
                }
            }
        }
    }
}
