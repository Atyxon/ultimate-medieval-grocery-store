using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFeed : MonoBehaviour
{
    public GameObject feedPrefab;
    public Transform feedParent;
    public void NewFeed(Item item)
    {
        GameObject nf = Instantiate(feedPrefab, feedParent.transform.position, feedParent.transform.rotation);
        nf.transform.parent = feedParent;
        nf.transform.SetSiblingIndex(0);
        nf.transform.localScale = new Vector3(1, 1, 1);
        Destroy(nf, 5);

        ItemFeedObject feedObj = nf.GetComponent<ItemFeedObject>();
        feedObj.icon.sprite = item.itemSprite;

        if (item.GetType() == typeof(SeedItem))
        {
            SeedItem seed = (SeedItem)item;
            feedObj.quantityText.text = "+" + seed.quantity;
        }
    }
}
