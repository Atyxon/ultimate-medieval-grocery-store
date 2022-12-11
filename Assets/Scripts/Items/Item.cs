using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Default Item Menu")]
    public string itemName;
    public int tiemId;
    public GameObject itemPrefab;
    [TextArea]
    public string itemDesc;
    public Sprite itemSprite;
    [Space]
    public int priceInShop;
}
