using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class shopManager : MonoBehaviour
{
    public GameObject shopUI;
    public PlayerInventory playerInv;
    public TextMeshProUGUI playerMoneyText;
    [Space]
    public GameObject itemPrefab;
    public GameObject itemsParent;
    public Item[] itemsToInstantiate;
    public List<ItemInShop> items = new List<ItemInShop>();
    [Space]
    public Transform dropPoint;
    private void Start()
    {
        playerMoneyText.text = playerInv.playerMoney + "";
        for (int i = 0; i < itemsToInstantiate.Length; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, itemsParent.transform.position, itemsParent.transform.rotation);
            newItem.transform.parent = itemsParent.transform;
            newItem.transform.localScale = new Vector3(1,1,1);
            ItemInShop itemInShop = newItem.GetComponent<ItemInShop>();
            items.Add(itemInShop);
            itemInShop.item = itemsToInstantiate[i];

        }
    }
    public void Close()
    {
        playerInv.TriggerShop();
    }
    public void FilterItems(int index)
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].gameObject.SetActive(false);
        }

        if (index == 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].gameObject.SetActive(true);
            }
        }
        else if (index == 1)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item.GetType() == typeof(SeedItem))
                {
                    items[i].gameObject.SetActive(true);
                }
            }
        }
        else if (index == 2)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item.GetType() == typeof(ToolItem))
                {
                    items[i].gameObject.SetActive(true);
                }
            }
        }
        else if (index == 3)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item.GetType() == typeof(FuelItem))
                {
                    items[i].gameObject.SetActive(true);
                }
            }
        }
        else if (index == 4)
        {
/*            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item.GetType() == typeof(SeedItem))
                {
                    items[i].gameObject.SetActive(true);
                }
            }*/
        }
    }
}
