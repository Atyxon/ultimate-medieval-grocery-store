using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Seed", menuName = "New Item/Seed")]
public class SeedItem : Item
{
    [Header("Seed Menu")]
    public GameObject plantGhost;
    public RuntimeAnimatorController seedAnim;
    public int quantity;
    public int maxQuantity;
    public GameObject objectToInstantiate;
}
