using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Furnace : MonoBehaviour
{
    InventoryManager inventoryManager;
    [SerializeField] private GameObject furnaceUI;
    [SerializeField] private InventorySlot fuelSlot;
    [SerializeField] private InventorySlot inputSlot;
    [SerializeField] private InventorySlot outputSlot;
    [SerializeField] private GameObject mineralBubble;
    [SerializeField] private Slider sliderProgress;
    [SerializeField] private Slider sliderFuel;
    [SerializeField] private GameObject furnaceWhiteBorder;

    bool canFurnace = false;
    [HideInInspector] public bool isOpeningTheFurnace;
    float time = 0;
    float fuelLeft = 0;
    bool isHaveFuel = false;

    void Start()
    {
        inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();

        furnaceUI.SetActive(false);
        mineralBubble.SetActive(true);
        sliderProgress.maxValue = 3;
        sliderProgress.value = 0;
        sliderFuel.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        OpenAndCloseFurnace();

        if((fuelSlot.itemInSlot != null || fuelLeft > 0) && inputSlot.itemInSlot != null)
            StartFurnace();
        else
        {
            mineralBubble.SetActive(false);
            time = 0;
            sliderProgress.value = 0;
        }

        sliderFuel.value = fuelLeft;
    }

    void StartFurnace()
    {
        if(inputSlot.itemInSlot.item.ItemType == ItemType.Mineral)
        {
            ItemMineral itemMineral = (ItemMineral)inputSlot.itemInSlot.item;
            if((fuelLeft > 0 || fuelSlot.itemInSlot.item.IsFuelResource) && itemMineral.Output != null)
            {
                if((outputSlot.itemInSlot != null && outputSlot.itemInSlot.count < itemMineral.MaxStackSize && outputSlot.itemInSlot.item == itemMineral.Output) || outputSlot.itemInSlot == null)
                {
                    //Update the current melting mineral
                    mineralBubble.SetActive(true);
                    mineralBubble.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = inputSlot.itemInSlot.item.ItemSprite;

                    if(!isHaveFuel)
                    {
                        fuelSlot.itemInSlot.count--;
                        fuelSlot.itemInSlot.RefreshCount();
                        fuelLeft = fuelSlot.itemInSlot.item.FuelAmount;
                        isHaveFuel = true;
                        sliderFuel.maxValue = fuelLeft;
                    }

                    if(fuelLeft < 0)
                        isHaveFuel = false;
                    
                    //After finishing 1 melting session
                    if(time > 3)
                    {
                        //Decrease the input's number
                        time = 0;
                        inputSlot.itemInSlot.count--;
                        inputSlot.itemInSlot.RefreshCount();

                        //Increase the output's number
                        outputSlot.AddItemToSlot(outputSlot.itemInSlot, itemMineral.Output, 2);
                        outputSlot.UpdateTheInventoryItemInfo();

                        //If the outputs > MaxStackSize, then set the number to MaxStackSize
                        if(outputSlot.itemInSlot != null && outputSlot.itemInSlot.count > outputSlot.itemInSlot.item.MaxStackSize)
                            outputSlot.itemInSlot.count = outputSlot.itemInSlot.item.MaxStackSize;
                    }

                    time += Time.deltaTime;
                    fuelLeft -= Time.deltaTime;
                }
                else
                {
                    mineralBubble.SetActive(false);
                    time = 0;
                }
            }
        }
        
        sliderProgress.value = time;
    }

    void OpenAndCloseFurnace()
    {
        if(canFurnace && Input.GetKeyDown(KeyCode.F))
        {
            if(!isOpeningTheFurnace)
            {
                furnaceUI.SetActive(true);
                isOpeningTheFurnace = true;
                inventoryManager.ToggleTheInventory();
            }
            else
            {
                furnaceUI.SetActive(false);
                isOpeningTheFurnace = false;
                inventoryManager.ToggleTheInventory();
            }
            
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            canFurnace = true;
            furnaceWhiteBorder.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            canFurnace = false;
            isOpeningTheFurnace = false;
            furnaceUI.SetActive(false);
            inventoryManager.inventoryGroup.SetActive(false);
            inventoryManager.isOpeningTheInventory = false;
            furnaceWhiteBorder.SetActive(false);
        }
    }
}
