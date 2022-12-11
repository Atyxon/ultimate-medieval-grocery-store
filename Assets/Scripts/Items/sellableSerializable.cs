using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class sellableSerializable
{
    public int quantity;
    public enum SellableType { carrot, cabbage, corn };
    public SellableType sellableType;
}