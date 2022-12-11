using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Options")]
    public slotSerializable[] slots;
    public slotSerializable[] quickbarSlots;
    public int selectedSlot;
    public bool isSlotSelected;
    public Color selectedColor;
    public Color nonSelectedColor;
    public int playerMoney;
    [Space(20)]
    public Color hpColor;
    public Color lowHpColor;
    [Space]
    public InventorySlot clickedSlot;
    public bool inventoryIsOpen;
    public Animator uiAnim;
    [Header("Selected Seed Info Panel")]
    public GameObject selectedItemInfoSeed;
    public TextMeshProUGUI SelectedSeedDescription;
    public TextMeshProUGUI SelectedSeedName;
    public Image selectedSeedImage;
    public Image selectedSeedImageToSplit;
    [Header("Selected Tool Info Panel")]
    public GameObject selectedItemInfoTool;
    public TextMeshProUGUI SelectedToolDescription;
    public TextMeshProUGUI SelectedToolName;
    public Image selectedToolImage;
    public slotSerializable fuelSlot;
    public GameObject fuelPanel;
    public GameObject attachmentsPanel;
    [Header("Selected Misc Info Panel")]
    public GameObject selectedMiscInfoTool;
    public TextMeshProUGUI SelectedMiscDescription;
    public TextMeshProUGUI SelectedMiscName;
    public Image selectedMiscImage;
    [Space]
    public AnimationCurve slotSizeAnim;
    [HideInInspector]
    public float timer;
    public float timeMultiplier;
    [Space]
    public GameObject objectToInteract;
    public InteractInfo interInfo;
    public float rayDistance;
    public Sprite[] interactionSprites;
    public Image fillImage;
    public LayerMask interLayerMask;
    public GameObject pickUpParticle;
    [Space]
    [Header("Misc")]
    public TextMeshProUGUI outsideZoneText;
    public pauseMenuManager pauseMenu;
    public sleepManager sleep;
    public GameObject cantSleepWarning;
    public levelManager levelMng;
    public GameObject startAnim;
    public Volume volume;
    public GameObject[] objectsInHand;
    public GameObject shopUI;
    public TimeManager timeManager;
    public GameObject moneyPopup;
    public TextMeshProUGUI moneyPopupText;
    public TextMeshProUGUI moneyText;
    public float grabForce;
    public GameObject heldObject;
    public Transform holdPosition;
    public GameObject feedParent;
    public ItemFeed itemFeed;
    public GameObject playerArms;
    public Animator playerArmsAnim;
    public CameraController cameraController;
    public Camera playerCamera;
    public Controlls ctrls;
    public Sprite emptySprite;
    public int nextSlot;
    bool isHolding;
    public bool isShopUIOpen;
    DepthOfField depthOfField;
    public ColorAdjustments colorAdj;
    public ItemDropManager idm;
    public AudioSource audioSrc;
    public AudioClip[] pickupClips;
    public string interactString;

    RectTransform rt;
    bool charUpd;
    private void Awake()
    {
        playerArms.SetActive(false);
        startAnim.SetActive(true);
        volume.profile.TryGet<DepthOfField>(out depthOfField);
        volume.profile.TryGet<ColorAdjustments>(out colorAdj);
        moneyText.text = playerMoney + "";
        rt = interInfo.transform.gameObject.GetComponent<RectTransform>();
        ctrls = new Controlls();
        CloseAllItemInfoPanels();
        for (int i = 0; i < quickbarSlots.Length; i++)
        {
            if (isSlotSelected && i != selectedSlot)
            {
                quickbarSlots[i].invSlot.backGround.color = nonSelectedColor;
                quickbarSlots[i].invSlot.sprite.color = nonSelectedColor;
            }
            else 
            {
                quickbarSlots[i].invSlot.backGround.color = nonSelectedColor;
                quickbarSlots[i].invSlot.sprite.color = nonSelectedColor;
            }
        }
    }
    private void OnEnable()
    {
        ctrls.Enable();
    }
    private void OnDisable()
    {
        ctrls.Disable();
    }
    // UPDATING MONEY
    public void UpdateMoney(int value)
    {
        playerMoney += value;
        moneyText.text = playerMoney + "";
    }
    public void MoneyPopUp(int value)
    {
        moneyPopup.SetActive(false);
        moneyPopup.SetActive(true);
        moneyPopupText.text = "+" + value;
    }
    public void AddItem(Item item)
    {
        if (item.GetType() == typeof(SeedItem))
        {
            bool stillExist = true;
            SeedItem seedObject = (SeedItem)item;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null && stillExist)
                {
                    if (slots[i].item.GetType() == typeof(SeedItem) && slots[i].item.itemName == item.itemName)
                    {
                        SeedItem seedObjectSlot = (SeedItem)slots[i].item;
                        int a = seedObject.quantity;
                        int b = seedObjectSlot.quantity;

                        if (b < seedObjectSlot.maxQuantity)
                        {
                            if (b + a <= seedObjectSlot.maxQuantity)
                            {
                                seedObjectSlot.quantity += a;
                                seedObject.quantity = 0;
                                Destroy(seedObject);
                                stillExist = false;
                                slots[i].invSlot.quantityText.text = seedObjectSlot.quantity + "x";
                                break;
                            }
                            else if (b + a > seedObjectSlot.maxQuantity)
                            {
                                int diffrence = seedObjectSlot.maxQuantity - seedObjectSlot.quantity;
                                seedObjectSlot.quantity = seedObjectSlot.maxQuantity;
                                seedObject.quantity -= diffrence;
                                slots[i].invSlot.quantityText.text = seedObjectSlot.quantity + "x";
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null && stillExist)
                {
                    slots[i].item = item;
                    slots[i].invSlot.sprite.sprite = item.itemSprite;
                    slots[i].invSlot.quantityText.text = seedObject.quantity + "";
                    UpdateSlot(i, 0);
                    break;
                }
                else if (slots[i].item != null && i == slots.Length - 1)
                {
                    GameObject droppedItem = Instantiate(seedObject.itemPrefab, idm.dropPoint.position, idm.dropPoint.rotation);
                    Item Ditem = droppedItem.GetComponent<ItemPickable>().instanceItem;
                    Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
                    rb.AddRelativeForce(Vector3.forward * 100);

                    if (Ditem.GetType() == typeof(SeedItem))
                    {
                        SeedItem instanceItem = (SeedItem)Ditem;
                        instanceItem.quantity = seedObject.quantity;
                    }
                }
            }
        }
        else if (item.GetType() == typeof(ToolItem))
        {
            bool stillExist = true;
            ToolItem toolObject = (ToolItem)item;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null && stillExist)
                {
                    slots[i].item = item;
                    slots[i].invSlot.sprite.sprite = item.itemSprite;
                    slots[i].invSlot.quantityText.text = "";
                    UpdateSlot(i, 0);
                    break;
                }
                else if (slots[i].item != null && i == slots.Length - 1)
                {
                    GameObject droppedItem = Instantiate(toolObject.itemPrefab, idm.dropPoint.position, idm.dropPoint.rotation);
                    Item Ditem = droppedItem.GetComponent<ItemPickable>().instanceItem;
                    Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
                    rb.AddRelativeForce(Vector3.forward * 100);

                    if (Ditem.GetType() == typeof(ToolItem))
                    {
                        ToolItem instanceItem = (ToolItem)Ditem;
                        // assign fuel
                    }
                }
            }
        }
        else if(item.GetType() == typeof(FuelItem))
        {
            FuelItem fuelObject = (FuelItem)item;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    slots[i].item = item;
                    slots[i].invSlot.sprite.sprite = item.itemSprite;
                    slots[i].invSlot.quantityText.text = "";
                    UpdateSlot(i, 0);
                    break;
                }
                else if (slots[i].item != null && i == slots.Length - 1)
                {
                    GameObject droppedItem = Instantiate(fuelObject.itemPrefab, idm.dropPoint.position, idm.dropPoint.rotation);
                    Item Ditem = droppedItem.GetComponent<ItemPickable>().instanceItem;
                    Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
                    rb.AddRelativeForce(Vector3.forward * 100);

                    if (Ditem.GetType() == typeof(FuelItem))
                    {
                        FuelItem instanceItem = (FuelItem)Ditem;
                    }
                }
            }
        }
    }
    //OPENING SHOP

    public void TriggerShop()
    {
        if (!pauseMenu.isMenuOppened)
        {
            if (isShopUIOpen)
            {
                isShopUIOpen = false;
                shopUI.SetActive(false);
                cameraController.isEnabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                depthOfField.active = false;
            }
            else
            {
                isShopUIOpen = true;
                shopUI.SetActive(true);
                cameraController.isEnabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                depthOfField.active = true;
            }
        }
    }

    //UPDATING SLOT
    public void UpdateSlot(int index, int slotType)
    {
        //INVENTORY SLOT
        if (slotType == 0)
        {
            slots[index].invSlot.UpdateSlot(slots[index].item);
        }
        //QUICKBAR SLOT
        else if (slotType == 1)
        {
            quickbarSlots[index].invSlot.UpdateSlot(quickbarSlots[index].item);
        }
    }
    private void Update()
    {
        //HOLDING ITEM

        if (heldObject != null)
        {
            isHolding = true;
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            float hold = ctrls.Player.Interact.ReadValue<float>();
            if (hold >= .7)
            {
                rb.velocity = (holdPosition.position - heldObject.transform.position) * grabForce;
                rb.angularVelocity = Vector3.zero;
                heldObject.transform.rotation = Quaternion.Lerp(heldObject.transform.rotation, Quaternion.Euler(0,0,0), Time.deltaTime * 3);
            }
            else
            {
                int y = 1;
                rb.angularVelocity = new Vector3(Random.Range(-y, y), Random.Range(-y, y), Random.Range(-y, y));
                rb.velocity /= 2;
                heldObject = null;
                isHolding = false;
            }
        }

        //INTERACTIONS

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayDistance, interLayerMask) && !isHolding)
        {
            if (hit.transform.gameObject.tag == "Pickable")
            {
                interactString = "Pick Up Item";
                uiAnim.SetBool("Interaction", true);
                objectToInteract = hit.transform.gameObject;
                if (!inventoryIsOpen)
                {
                    interInfo.gameObject.SetActive(true);
                    interInfo.image.sprite = interactionSprites[0];
                    interInfo.transform.position = playerCamera.WorldToScreenPoint(objectToInteract.transform.position);

                    if (ctrls.Player.Interact.triggered && objectToInteract != null)
                    {
                        Item newItem = objectToInteract.GetComponent<ItemPickable>().instanceItem;
                        GameObject newParticle = Instantiate(pickUpParticle, objectToInteract.transform.position, Quaternion.Euler(0, 0, 0));
                        Destroy(newParticle, 4);
                        Destroy(objectToInteract);

                        itemFeed.NewFeed(newItem);
                        AddItem(newItem);
                        audioSrc.clip = pickupClips[Random.Range(0, pickupClips.Length)];
                        audioSrc.Play();
                    }
                }
                else
                {
                    interInfo.gameObject.SetActive(false);
                }
            }
            else if (hit.transform.gameObject.tag == "openable")
            {
                uiAnim.SetBool("Interaction", true);
                objectToInteract = hit.transform.gameObject;
                if (!inventoryIsOpen)
                {
                    openableHandler opHandler = hit.transform.gameObject.GetComponent<openableHandler>();
                    if(opHandler.isOpen)
                        interactString = "Close";
                    else
                        interactString = "Open";
                    interInfo.gameObject.SetActive(true);
                    interInfo.image.sprite = interactionSprites[1];
                    interInfo.transform.position = playerCamera.WorldToScreenPoint(opHandler.iconPoint.transform.position);

                    if (ctrls.Player.Interact.triggered && objectToInteract != null)
                    {
                        opHandler.interaction();
                    }
                }
                else
                {
                    interInfo.gameObject.SetActive(false);
                }
            }
            else if (hit.transform.gameObject.tag == "crop")
            {
                interactString = "Crop Plant";
                uiAnim.SetBool("Interaction", true);
                objectToInteract = hit.transform.gameObject;
                if (!inventoryIsOpen)
                {
                    interInfo.gameObject.SetActive(true);
                    interInfo.image.sprite = interactionSprites[2];
                    interInfo.transform.position = playerCamera.WorldToScreenPoint(objectToInteract.transform.position);

                    float hold = ctrls.Player.Interact.ReadValue<float>();

                    if (hold > .8)
                    {
                        fillImage.fillAmount += Time.deltaTime * 2;
                        if (fillImage.fillAmount == 1)
                        {
                            plantCrop crop = hit.transform.gameObject.GetComponent<plantCrop>();
                            crop.Cropped();
                            levelMng.addExp("Planting", 10);
                            levelMng.addExp("Player", 3);
                            fillImage.fillAmount = 0;
                        }
                    }
                    else
                    {
                        fillImage.fillAmount = 0;
                    }
                }
                else
                {
                    interInfo.gameObject.SetActive(false);
                }
            }
            else if (hit.transform.gameObject.tag == "dragable")
            {
                interactString = "Drag";
                uiAnim.SetBool("Interaction", true);
                objectToInteract = hit.transform.gameObject;
                if (!inventoryIsOpen)
                {
                    interInfo.gameObject.SetActive(true);
                    interInfo.image.sprite = interactionSprites[2];
                    interInfo.transform.position = playerCamera.WorldToScreenPoint(objectToInteract.transform.position);

                    float hold = ctrls.Player.Interact.ReadValue<float>();

                    if (hold > .8)
                    {
                        heldObject = objectToInteract;
                    }
                }
                else
                {
                    interInfo.gameObject.SetActive(false);
                }
            }
            else if (hit.transform.gameObject.tag == "shop")
            {
                uiAnim.SetBool("Interaction", true);
                objectToInteract = hit.transform.gameObject;
                if (!inventoryIsOpen)
                {
                    shopPoint shop = hit.transform.gameObject.GetComponent<shopPoint>();
                    if(isShopUIOpen)
                        interactString = "Close Shop";
                    else
                        interactString = "Open Shop";
                    interInfo.gameObject.SetActive(true);
                    interInfo.image.sprite = interactionSprites[3];
                    interInfo.transform.position = playerCamera.WorldToScreenPoint(shop.iconPoint.transform.position);

                    if (ctrls.Player.Interact.triggered && objectToInteract != null)
                    {
                        TriggerShop();
                    }
                }
                else
                {
                    interInfo.gameObject.SetActive(false);
                }
            }
            else if (hit.transform.gameObject.tag == "bed")
            {
                interactString = "Sleep";
                uiAnim.SetBool("Interaction", true);
                objectToInteract = hit.transform.gameObject;
                if (!inventoryIsOpen)
                {
                    GameObject bed = hit.transform.gameObject;
                    interInfo.gameObject.SetActive(true);
                    interInfo.image.sprite = interactionSprites[4];
                    interInfo.transform.position = playerCamera.WorldToScreenPoint(bed.transform.position);

                    if (ctrls.Player.Interact.triggered && objectToInteract != null)
                    {
                        if (timeManager.time > 1200 || timeManager.time < 240)
                            sleep.GoSleep();
                        else
                        {
                            cantSleepWarning.SetActive(false);
                            cantSleepWarning.SetActive(true);
                        }
                    }
                }
                else
                {
                    interInfo.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            interactString = "";
            objectToInteract = null;
            uiAnim.SetBool("Interaction", false);
            interInfo.gameObject.SetActive(false);
            fillImage.fillAmount = 0;
        }
        //OPENING PAUSE MENU

        if (ctrls.Player.Escape.triggered)
        {
            if (inventoryIsOpen || isShopUIOpen)
            {
                isShopUIOpen = false;
                shopUI.SetActive(false);
                cameraController.isEnabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                depthOfField.active = false;

                uiAnim.SetBool("Open Inventory", false);
                inventoryIsOpen = false;

                cameraController.isEnabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                depthOfField.active = false;
                for (int i = 0; i < quickbarSlots.Length; i++)
                {
                    quickbarSlots[i].invSlot.backGround.color = nonSelectedColor;
                    quickbarSlots[i].invSlot.sprite.color = nonSelectedColor;
                }
                if (isSlotSelected)
                {
                    quickbarSlots[selectedSlot].invSlot.backGround.color = selectedColor;
                    quickbarSlots[selectedSlot].invSlot.sprite.color = selectedColor;
                }
            }
            else
            {
                if (pauseMenu.isMenuOppened)
                {
                    pauseMenu.Resume();
                }
                else
                {
                    pauseMenu.OpenMenu();
                }
            }
        }

        //OPENING INVENTORY

        if (ctrls.Player.Inventory.triggered && !pauseMenu.isMenuOppened)
        {
            if (inventoryIsOpen)
            {
                uiAnim.SetBool("Open Inventory", false);
                inventoryIsOpen = false;

                cameraController.isEnabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                depthOfField.active = false;
                for (int i = 0; i < quickbarSlots.Length; i++)
                {
                    quickbarSlots[i].invSlot.backGround.color = nonSelectedColor;
                    quickbarSlots[i].invSlot.sprite.color = nonSelectedColor;
                }
                if (isSlotSelected)
                {
                    quickbarSlots[selectedSlot].invSlot.backGround.color = selectedColor;
                    quickbarSlots[selectedSlot].invSlot.sprite.color = selectedColor;
                }
            }
            else if (!inventoryIsOpen && !isShopUIOpen)
            {
                uiAnim.SetBool("Open Inventory", true);
                inventoryIsOpen = true;
                depthOfField.active = true;
                selectedItemInfoSeed.SetActive(false);
                selectedItemInfoTool.SetActive(false);
                moneyText.text = playerMoney + "";

                cameraController.isEnabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                for (int i = 0; i < quickbarSlots.Length; i++)
                {
                    quickbarSlots[i].invSlot.backGround.color = selectedColor;
                    quickbarSlots[i].invSlot.sprite.color = selectedColor;
                }
            }
        }

        //QUICKBAR SLOTS HANDLER

        if (ctrls.Player.Slot1.triggered && !inventoryIsOpen)
        {
            if (!isSlotSelected || quickbarSlots[selectedSlot].item == null)
            {
                ChangeSelectedSlot(0);
            }
            else
            {
                nextSlot = 0;
                playerArmsAnim.SetTrigger("Hide");
            }
        }
        else if(ctrls.Player.Slot2.triggered && !inventoryIsOpen)
        {
            if (!isSlotSelected || quickbarSlots[selectedSlot].item == null)
                ChangeSelectedSlot(1);
            else
            {
                nextSlot = 1;
                playerArmsAnim.SetTrigger("Hide");
            }
        }
        else if(ctrls.Player.Slot3.triggered && !inventoryIsOpen)
        {
            if (!isSlotSelected || quickbarSlots[selectedSlot].item == null)
                ChangeSelectedSlot(2);
            else
            {
                nextSlot = 2;
                playerArmsAnim.SetTrigger("Hide");
            }
        }
        else if(ctrls.Player.Slot4.triggered && !inventoryIsOpen)
        {
            if (!isSlotSelected || quickbarSlots[selectedSlot].item == null)
                ChangeSelectedSlot(3);
            else
            {
                nextSlot = 3;
                playerArmsAnim.SetTrigger("Hide");
            }
        }
        else if(ctrls.Player.Slot5.triggered && !inventoryIsOpen)
        {
            if (!isSlotSelected || quickbarSlots[selectedSlot].item == null)
                ChangeSelectedSlot(4);
            else
            {
                nextSlot = 4;
                playerArmsAnim.SetTrigger("Hide");
            }
        }
    }
    public void ChangeSelectedSlot(int slotIndex)
    {
        bool canChangeSlot = true;
        for (int i = 0; i < objectsInHand.Length; i++)
        {
            objectsInHand[i].SetActive(false);
        }

        if (canChangeSlot)
        {
            if (!inventoryIsOpen)
            {
                quickbarSlots[selectedSlot].invSlot.backGround.color = nonSelectedColor;
                quickbarSlots[selectedSlot].invSlot.sprite.color = nonSelectedColor;
                if (clickedSlot != null)
                {
                    float size = slotSizeAnim.Evaluate(0);
                    clickedSlot.transform.localScale = new Vector3(size, size, size);
                }

                clickedSlot = quickbarSlots[slotIndex].invSlot;
                timer = 0;
            }

            if (isSlotSelected)
            {
                if (selectedSlot == slotIndex)
                {
                    isSlotSelected = false;
                }
                else
                {
                    selectedSlot = slotIndex;
                    quickbarSlots[slotIndex].invSlot.backGround.color = selectedColor;
                    quickbarSlots[slotIndex].invSlot.sprite.color = selectedColor;
                }
            }
            else
            {
                selectedSlot = slotIndex;
                isSlotSelected = true;

                quickbarSlots[slotIndex].invSlot.backGround.color = selectedColor;
                quickbarSlots[slotIndex].invSlot.sprite.color = selectedColor;
            }

            if (quickbarSlots[selectedSlot].item != null && isSlotSelected)
            {
                if (quickbarSlots[selectedSlot].item.GetType() == typeof(SeedItem))
                {
                    SeedItem seedObject = (SeedItem)quickbarSlots[selectedSlot].item;
                    playerArms.SetActive(true);
                    playerArmsAnim.runtimeAnimatorController = seedObject.seedAnim;
                    playerArmsAnim.SetTrigger("Draw");
                }
                else if (quickbarSlots[selectedSlot].item.GetType() == typeof(ToolItem))
                {
                    ToolItem toolObject = (ToolItem)quickbarSlots[selectedSlot].item;
                    playerArms.SetActive(true);
                    playerArmsAnim.runtimeAnimatorController = toolObject.toolAnim;
                    playerArmsAnim.SetTrigger("Draw");
                }
            }
            else
            {
                playerArms.SetActive(false);
            }
        }
        else
        {
            isSlotSelected = false;
            playerArms.SetActive(false);
        }

        for (int i = 0; i < objectsInHand.Length; i++)
        {
            if (objectsInHand[i] != null && objectsInHand[i].name == quickbarSlots[selectedSlot].item.itemName)
            {
                objectsInHand[i].SetActive(true);
                if (quickbarSlots[selectedSlot].item.GetType() == typeof(ToolItem))
                {
                    ToolItem toolObject = (ToolItem)quickbarSlots[selectedSlot].item;
                    if (toolObject.useFuel)
                    {
                        Tool toolObjectInHand = objectsInHand[i].GetComponent<Tool>();
                        if(toolObject.loadedFuel != null)
                            toolObjectInHand.SelectFuel(toolObject.loadedFuel.itemName);
                        else
                            toolObjectInHand.HideFuel();
                    }
                }
            }
        }
    }
    private void FixedUpdate()
    {
        CurveHandler();
    }
    public void UpdateSelectedItemWindow(DragDrop slot)
    {
        if (slot != clickedSlot)
        {
            if (clickedSlot != null)
            {
                float size = slotSizeAnim.Evaluate(0);
                clickedSlot.transform.localScale = new Vector3(size, size, size);
            }

            clickedSlot = slot.slot;
            //IF INV SLOT
            if (slot.slotType == DragDrop.SlotType.Inventory)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i].invSlot == slot.slot)
                    {
                        if (slots[i].item != null)
                        {
                            if (slots[i].item.GetType() == typeof(SeedItem))
                            {
                                CloseAllItemInfoPanels();
                                selectedItemInfoSeed.SetActive(true);
                                selectedSeedImage.sprite = slots[i].item.itemSprite;
                                selectedSeedImageToSplit.sprite = slots[i].item.itemSprite;
                                SelectedSeedDescription.text = slots[i].item.itemDesc;
                                SelectedSeedName.text = slots[i].item.itemName;
                            }
                            else if (slots[i].item.GetType() == typeof(ToolItem))
                            {
                                CloseAllItemInfoPanels();
                                selectedItemInfoTool.SetActive(true);
                                selectedToolImage.sprite = slots[i].item.itemSprite;
                                SelectedToolDescription.text = slots[i].item.itemDesc;
                                SelectedToolName.text = slots[i].item.itemName;
                                ToolItem tool = (ToolItem)slots[i].item;
                                if (tool.useFuel)
                                {
                                    fuelPanel.SetActive(true);
                                    if (tool.loadedFuel != null)
                                    {
                                        fuelSlot.item = tool.loadedFuel;
                                        fuelSlot.invSlot.quantityText.text = (int)tool.loadedFuel.fuel + "";
                                        fuelSlot.invSlot.sprite.sprite = tool.loadedFuel.itemSprite;
                                    }
                                    else
                                    {
                                        fuelSlot.item = null;
                                        fuelSlot.invSlot.quantityText.text = "";
                                        fuelSlot.invSlot.sprite.sprite = emptySprite;
                                    }
                                }
                                else
                                {
                                    fuelPanel.SetActive(false);
                                }

                                if (tool.useAttachments)
                                {
                                    attachmentsPanel.SetActive(true);
                                }
                                else
                                {
                                    attachmentsPanel.SetActive(false);
                                }
                            }
                            else if (slots[i].item.GetType() == typeof(FuelItem))
                            {
                                CloseAllItemInfoPanels();
                                selectedMiscInfoTool.SetActive(true);
                                selectedMiscImage.sprite = slots[i].item.itemSprite;
                                SelectedMiscDescription.text = slots[i].item.itemDesc;
                                SelectedMiscName.text = slots[i].item.itemName;
                            }
                        }
                        else
                        {
                            CloseAllItemInfoPanels();
                        }
                    }
                }
            }
            //IF QCB SLOT
            else if (slot.slotType == DragDrop.SlotType.Quickbar)
            {
                for (int i = 0; i < quickbarSlots.Length; i++)
                {
                    if (quickbarSlots[i].invSlot == slot.slot)
                    {
                        if (quickbarSlots[i].item != null)
                        {
                            if (quickbarSlots[i].item.GetType() == typeof(SeedItem))
                            {
                                CloseAllItemInfoPanels();
                                selectedItemInfoSeed.SetActive(true);
                                selectedSeedImage.sprite = quickbarSlots[i].item.itemSprite;
                                selectedSeedImageToSplit.sprite = quickbarSlots[i].item.itemSprite;
                                SelectedSeedDescription.text = quickbarSlots[i].item.itemDesc;
                                SelectedSeedName.text = quickbarSlots[i].item.itemName;
                            }
                            else if (quickbarSlots[i].item.GetType() == typeof(ToolItem))
                            {
                                CloseAllItemInfoPanels();
                                selectedItemInfoTool.SetActive(true);
                                selectedToolImage.sprite = quickbarSlots[i].item.itemSprite;
                                SelectedToolDescription.text = quickbarSlots[i].item.itemDesc;
                                SelectedToolName.text = quickbarSlots[i].item.itemName;
                                ToolItem tool = (ToolItem)quickbarSlots[i].item;
                                if (tool.useFuel)
                                {
                                    fuelPanel.SetActive(true);
                                    if (tool.loadedFuel != null)
                                    {
                                        fuelSlot.item = tool.loadedFuel;
                                        fuelSlot.invSlot.quantityText.text = (int)tool.loadedFuel.fuel + "";
                                        fuelSlot.invSlot.sprite.sprite = tool.loadedFuel.itemSprite;
                                    }
                                    else
                                    {
                                        fuelSlot.item = null;
                                        fuelSlot.invSlot.quantityText.text = "";
                                        fuelSlot.invSlot.sprite.sprite = emptySprite;
                                    }
                                }
                                else
                                {
                                    fuelPanel.SetActive(false);
                                }

                                if (tool.useAttachments)
                                {
                                    attachmentsPanel.SetActive(true);
                                }
                                else
                                {
                                    attachmentsPanel.SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            CloseAllItemInfoPanels();
                        }
                    }
                }
            }
        }
    }
    public void CloseAllItemInfoPanels()
    {
        selectedItemInfoTool.SetActive(false);
        selectedItemInfoSeed.SetActive(false);
        selectedMiscInfoTool.SetActive(false);
    }
    public void CurveHandler()
    {
        timer += Time.fixedDeltaTime * timeMultiplier;
        float size = slotSizeAnim.Evaluate(timer);
        if (clickedSlot != null)
            clickedSlot.transform.localScale = new Vector3(size,size,size);
    }
    public void Builded()
    {
        if (quickbarSlots[selectedSlot].item.GetType() == typeof(SeedItem))
        {
            SeedItem seedObject = (SeedItem)quickbarSlots[selectedSlot].item;
            seedObject.quantity--;
            if (seedObject.quantity == 0)
            {
                quickbarSlots[selectedSlot].item = null;
                nextSlot = selectedSlot;
                playerArmsAnim.SetTrigger("Hide");
            }
            UpdateSlot(selectedSlot, 1);
        }
    }
}
