using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;
    public InventoryItem itemInSlot;
    public InventoryManager inventoryManager;
    public GameObject inventoryItemPrefab;

    private void Awake()
    {
        Deselect();
    }

    void Update()
    {
        //Get InventoryItemInfo
        if(transform.childCount != 0)
        {
            itemInSlot = GetComponentInChildren<InventoryItem>();
        }
        else
        {
            itemInSlot = null;
        }

        if(!inventoryManager.isOpeningTheInventory)
        {
            if(inventoryManager.currentMouseItem != null)
            {
                for(int i = 0; i < inventoryManager.currentMouseItem.count; i++)
                {
                    inventoryManager.AddItem(inventoryManager.currentMouseItem.item);
                    inventoryManager.currentMouseItem.count--;
                    inventoryManager.currentMouseItem.RefreshCount();
                }
            }
        }
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
        image.color = notSelectedColor;
    }

    public void PickingUpItem(InventoryItem item)
    {
        inventoryManager.currentMouseItem = item;
        inventoryManager.currentMouseItem.parentAfterDrag = item.transform.parent;
        inventoryManager.currentMouseItem.transform.SetParent(inventoryManager.currentMouseItem.transform.root);
    }

    public void PlaceItem(InventoryItem item)
    {
        item = inventoryManager.currentMouseItem;
        item.transform.SetParent(inventoryManager.currentMouseItem.parentAfterDrag);
        inventoryManager.currentMouseItem = null;
    }

    public void SplitItem(InventoryItem item)
    {
        if(item.count > 1)
        {
            Debug.Log(true);
            int halfQuantity = item.count/2;
            item.count -= halfQuantity;
            item.RefreshCount();

            GameObject newItemGO = Instantiate(inventoryItemPrefab, transform);
            inventoryManager.currentMouseItem = newItemGO.GetComponent<InventoryItem>();
            inventoryManager.currentMouseItem.InitialiseItem(item.item);
            inventoryManager.currentMouseItem.count = halfQuantity;
            inventoryManager.currentMouseItem.RefreshCount();
            
            inventoryManager.currentMouseItem.parentAfterDrag = item.transform.parent;
            inventoryManager.currentMouseItem.transform.SetParent(inventoryManager.currentMouseItem.transform.root);
        }
    }

    public void SwapItem()
    {
        //swap count
        int tgII;
        tgII = itemInSlot.count;
        itemInSlot.count = inventoryManager.currentMouseItem.count;
        inventoryManager.currentMouseItem.count = tgII;

        //swap info of item
        Item tg = itemInSlot.item;
        itemInSlot.item = inventoryManager.currentMouseItem.item;
        inventoryManager.currentMouseItem.item = tg;

        //refresh info
        itemInSlot.InitialiseItem(itemInSlot.item);
        inventoryManager.currentMouseItem.InitialiseItem(inventoryManager.currentMouseItem.item);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Select();
        inventoryManager.UpdateTooltips(itemInSlot, true);

        if(inventoryManager.currentMouseItem != null)
        {
            inventoryManager.currentMouseItem.parentAfterDrag = transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Deselect(); 
        inventoryManager.UpdateTooltips((InventoryItem)null, false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(inventoryManager.isOpeningTheInventory)
        {
            if(inventoryManager.currentMouseItem != null)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    //if slot is empty, add all to slot
                    if(itemInSlot == null)
                    {
                        PlaceItem(itemInSlot);
                    }
                    //if it is not, and the slot item is the same as the holding item, then add
                    else if(inventoryManager.currentMouseItem.item == itemInSlot.item)
                    {
                        //if the sum out of MaxStackSize, add to the slot till it full stack
                        if(inventoryManager.currentMouseItem.count + itemInSlot.count > itemInSlot.item.MaxStackSize && itemInSlot.count < itemInSlot.item.MaxStackSize)
                        {
                            int quantity = Mathf.Abs(itemInSlot.item.MaxStackSize - itemInSlot.count);
                            AddItemToSlot(itemInSlot, inventoryManager.currentMouseItem, quantity);
                        }
                        //else, add all
                        else if(inventoryManager.currentMouseItem.count + itemInSlot.count <= itemInSlot.item.MaxStackSize)
                        {
                            AddItemToSlot(itemInSlot, inventoryManager.currentMouseItem, inventoryManager.currentMouseItem.count);
                        }
                        else if(itemInSlot.count == itemInSlot.item.MaxStackSize && inventoryManager.currentMouseItem.count < itemInSlot.item.MaxStackSize)
                        {
                            SwapItem();
                        }
                    }
                    else if(inventoryManager.currentMouseItem.item != itemInSlot.item)
                    {
                        SwapItem();
                    }
                }
                //else if press Right-click and (slot is empty, or slot item is the same as the holding item), add to the slot 
                else if(((itemInSlot == null) || (inventoryManager.currentMouseItem.item.Name == itemInSlot.item.Name)) && Input.GetMouseButtonDown(1))
                {
                    AddItemToSlot(itemInSlot, inventoryManager.currentMouseItem, 1);
                }
                
            }
            //else if not holding item, pick item, or split item
            else if(inventoryManager.currentMouseItem == null && itemInSlot != null)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    PickingUpItem(itemInSlot);
                }
                else if(Input.GetMouseButtonDown(1))
                {
                    SplitItem(itemInSlot);
                }
            }
        }
    }

    public void AddItemToSlot(InventoryItem itemInSlot, InventoryItem itemToAdd, int quantity)
    {
        if(itemInSlot == null)
        {
            GameObject newItemGO = Instantiate(inventoryItemPrefab, transform);
            itemInSlot = newItemGO.GetComponent<InventoryItem>();
            itemInSlot.InitialiseItem(itemToAdd.item);
            itemInSlot = itemToAdd;
            itemToAdd.count--;
            itemToAdd.RefreshCount();
        }
        else if(itemInSlot.count < itemInSlot.item.MaxStackSize)
        {
            itemInSlot.count += quantity;
            itemInSlot.RefreshCount();
            inventoryManager.currentMouseItem.count -= quantity;
            inventoryManager.currentMouseItem.RefreshCount();
        }
    }

    public void AddItemToSlot(InventoryItem itemInSlot, Item item, int quantity)
    {
        if(itemInSlot == null)
        {
            GameObject newItemGO = Instantiate(inventoryItemPrefab, transform);
            itemInSlot = newItemGO.GetComponent<InventoryItem>();
            itemInSlot.InitialiseItem(item);
            itemInSlot.count += quantity - 1;
            itemInSlot.RefreshCount();
        }
        else if(itemInSlot.count < item.MaxStackSize)
        {
            itemInSlot.count += quantity;
            itemInSlot.RefreshCount();
        }
    }
}
