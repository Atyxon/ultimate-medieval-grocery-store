    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public InventorySlot thisSlot;
    public Image sprite;
    public Image backGround;
    public PlayerInventory inventorySys;
    public DragDrop slotSprite;
    [Space]
    public TextMeshProUGUI quantityText;
    public GameObject hpBar;
    public RectTransform hp;
    public Image hpCol;
    private void Start()
    {
        thisSlot = GetComponent<InventorySlot>();
    }
    public void UpdateSlot(Item item)
    {
        if (item != null)
        {
            sprite.sprite = item.itemSprite;
            //IF ITEM IS SEED
            if (item.GetType() == typeof(SeedItem))
            {
                SeedItem seedObject = (SeedItem)item;
                quantityText.text = seedObject.quantity + "x";
                hpBar.SetActive(false);
                sprite.color = inventorySys.hpColor;
            }
            //IF ITEM IS TOOL
            else if (item.GetType() == typeof(ToolItem))
            {
                ToolItem toolObject = (ToolItem)item;
                if (!toolObject.useBarToShowFillLevel)
                {
                    quantityText.text = toolObject.fillLevel + toolObject.postfix;
                    hpBar.SetActive(false);
                }
                else
                {
                    quantityText.text = "";
                    hpBar.SetActive(true);
                    RectTransform hpBarColor = hpCol.GetComponent<RectTransform>();
                    if (toolObject.loadedFuel != null)
                    {
                        hpBarColor.sizeDelta = new Vector2(340 * ((float)toolObject.loadedFuel.fuel / (float)toolObject.loadedFuel.fuelMax), 66.12f);
                    }
                    else
                    {
                        hpBarColor.sizeDelta = new Vector2(0, 66.12f);
                    }
                }
                sprite.color = inventorySys.hpColor;
            }
            //IF ITEM IS FUEL
            if (item.GetType() == typeof(FuelItem))
            {
                FuelItem fuelObject = (FuelItem)item;
                quantityText.text = (int)fuelObject.fuel + "";
                hpBar.SetActive(false);
                sprite.color = inventorySys.hpColor;
            }
        }
        else
        {
            sprite.sprite = inventorySys.emptySprite;
            quantityText.text = "";
            hpBar.SetActive(false);
        }
    }
}
