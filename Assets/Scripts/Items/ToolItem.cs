using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tool", menuName = "New Item/Tool")]
public class ToolItem : Item
{
    [Header("Tool Menu")]
    public RuntimeAnimatorController toolAnim;
    public float fillLevel;
    public float maxFillLevel;
    public string postfix;
    public bool useBarToShowFillLevel;
    [Header("Fuel")]
    public bool useFuel;
    public FuelItem loadedFuel;
    [Header("Attachments")]
    public bool useAttachments;
}
