using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInShop : MonoBehaviour
{
    public Item item;
    public shopManager shop;
    public Image itemImage;
    public GameObject itemQuantityCounter;
    [Space]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI moneyText;
    [Space]
    public int itemCount = 1;
    public int maxItems;
    public AudioSource audioSrc;
    public AudioClip[] coinClips;
    public AudioClip declineClip;
    public void Start()
    {
        shop = FindObjectOfType<shopManager>();
        itemImage.sprite = item.itemSprite;
        nameText.text = item.itemName;
        countText.text = itemCount + "";
        moneyText.text = (item.priceInShop * itemCount) + "";

        if (item.GetType() == typeof(SeedItem))
        {
            SeedItem seedObject = (SeedItem)item;
            maxItems = seedObject.maxQuantity;
        }
        else
        {
            itemQuantityCounter.SetActive(false);
        }
    }
    public void Plus()
    {
        if (itemCount < maxItems)
        {
            itemCount++;
            countText.text = itemCount + "";
            moneyText.text = (item.priceInShop * itemCount) + "";
        }
    }
    public void Minus()
    {
        if (itemCount > 1)
        {
            itemCount--;
            countText.text = itemCount + "";
            moneyText.text = (item.priceInShop * itemCount) + "";
        }
    }
    public void Buy()
    {
        if (shop.playerInv.playerMoney >= item.priceInShop * itemCount)
        {
            shop.playerInv.playerMoney -= item.priceInShop * itemCount;
            shop.playerMoneyText.text = shop.playerInv.playerMoney + "";
            shop.playerInv.moneyText.text = shop.playerInv.playerMoney + "";

            GameObject droppedItem = Instantiate(item.itemPrefab, shop.dropPoint.position, shop.dropPoint.rotation);
            Item deliveredItem = droppedItem.GetComponent<ItemPickable>().instanceItem;
            audioSrc.clip = coinClips[Random.Range(0, coinClips.Length)];
            audioSrc.Play();

            if (item.GetType() == typeof(SeedItem))
            {
                SeedItem instanceItem = (SeedItem)deliveredItem;
                instanceItem.quantity = itemCount;
            }
        }
        else
        {
            audioSrc.clip = declineClip;
            audioSrc.Play();
        }
    }
}
