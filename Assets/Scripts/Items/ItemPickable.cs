using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickable : MonoBehaviour
{
    public Item originalItem;
    [Space]
    public Item instanceItem;
    private void Awake()
    {
        gameObject.name = originalItem.itemName;
        instanceItem = Instantiate(originalItem);
        if (instanceItem.GetType() == typeof(SeedItem))
        {
            SeedItem seedItem = (SeedItem)instanceItem;
            seedItem.quantity = Random.Range(1, 5);
        }
    }
}
