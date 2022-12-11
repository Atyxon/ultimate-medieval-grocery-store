using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fuel", menuName = "New Item/Fuel")]
public class FuelItem : Item
{
    [Header("Fuel Menu")]
    public float fuel;
    public float fuelMax;
    public Color fuelColor;
}
