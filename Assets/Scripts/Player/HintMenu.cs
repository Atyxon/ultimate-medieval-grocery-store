using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintMenu : MonoBehaviour
{
    public TextMeshProUGUI hintText;
    public PlayerInventory playerInv;
    public string inventoryString;
    public string interactionString;
    private void Update()
    {
        if (playerInv.inventoryIsOpen)
        {
            inventoryString = "Close Inventory   <color=orange>TAB</color> or <color=orange>ESC</color>\n";
        }
        else
        {
            inventoryString = "Open Inventory   <color=orange>TAB</color>\n";
        }
        if (playerInv.objectToInteract != null)
        {
            interactionString = playerInv.interactString + "   <color=orange>E</color>\n";
        }
        else
        {
            interactionString = "";
        }
        hintText.text = inventoryString + interactionString;
    }
}
