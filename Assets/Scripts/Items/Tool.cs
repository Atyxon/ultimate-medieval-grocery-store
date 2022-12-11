using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public PlayerInventory playerInv;
    public GameObject[] fuelObjects;
    public FuelItem fuelInSelectedTool;
    private void OnEnable()
    {
/*        print("hi");
        ToolItem toolObject = (ToolItem)playerInv.quickbarSlots[playerInv.selectedSlot].item;
        if (toolObject.useFuel)
        {
            if (toolObject.loadedFuel != null)
            {
                SelectFuel(toolObject.loadedFuel.itemName);
            }
            else
            {
                HideFuel();
            }
        }*/
    }
    public void SelectFuel(string fuelName)
    {
        HideFuel();
        for (int i = 0; i < fuelObjects.Length; i++)
        {
            if (fuelObjects[i].name == fuelName)
            {
                fuelObjects[i].SetActive(true);
                ToolItem selectedTool = (ToolItem)playerInv.quickbarSlots[playerInv.selectedSlot].item;
                fuelInSelectedTool = selectedTool.loadedFuel;
                SendMessage("NewFuel");
                break;
            }
        }
    }
    public void HideFuel()
    {
        for (int i = 0; i < fuelObjects.Length; i++)
        {
            fuelObjects[i].SetActive(false);
        }
    }
}
