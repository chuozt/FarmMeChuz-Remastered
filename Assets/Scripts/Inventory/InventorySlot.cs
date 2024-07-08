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
    public GameObject inventoryItemPrefab;

    private void Awake() => Deselect();

    void Update()
    {
        UpdateTheInventoryItemInfo();

        //Re-add the holding items to the inventory if toggle it off
        if(!InventoryManager.Instance.isOpeningTheInventory)
        {
            if(InventoryManager.Instance.currentMouseItem != null)
            {
                for(int i = 0; i < InventoryManager.Instance.currentMouseItem.count; i++)
                {
                    InventoryManager.Instance.AddItem(InventoryManager.Instance.currentMouseItem.item);
                    InventoryManager.Instance.currentMouseItem.count--;
                    InventoryManager.Instance.currentMouseItem.RefreshCount();
                }
            }
        }
    }

    public void Select() => image.color = selectedColor;

    public void Deselect() => image.color = notSelectedColor;

    public void UpdateTheInventoryItemInfo()
    {
        //Get InventoryItemInfo
        if(transform.childCount != 0)
            itemInSlot = GetComponentInChildren<InventoryItem>();
        else
            itemInSlot = null;
    }

    public void PickItem(InventoryItem item)
    {
        InventoryManager.Instance.currentMouseItem = item;
        InventoryManager.Instance.currentMouseItem.parentAfterDrag = item.transform.parent;
        InventoryManager.Instance.currentMouseItem.transform.SetParent(InventoryManager.Instance.currentMouseItem.transform.root);
    }

    public void PlaceItem(InventoryItem item)
    {
        item = InventoryManager.Instance.currentMouseItem;
        item.transform.SetParent(InventoryManager.Instance.currentMouseItem.parentAfterDrag);
        InventoryManager.Instance.currentMouseItem = null;
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
            InventoryManager.Instance.currentMouseItem = newItemGO.GetComponent<InventoryItem>();
            InventoryManager.Instance.currentMouseItem.InitialiseItem(item.item);
            InventoryManager.Instance.currentMouseItem.count = halfQuantity;
            InventoryManager.Instance.currentMouseItem.RefreshCount();
            
            InventoryManager.Instance.currentMouseItem.parentAfterDrag = item.transform.parent;
            InventoryManager.Instance.currentMouseItem.transform.SetParent(InventoryManager.Instance.currentMouseItem.transform.root);
        }
    }

    public void SwapItem()
    {
        //swap count
        int tgII;
        tgII = itemInSlot.count;
        itemInSlot.count = InventoryManager.Instance.currentMouseItem.count;
        InventoryManager.Instance.currentMouseItem.count = tgII;

        //swap info of item
        Item tg = itemInSlot.item;
        itemInSlot.item = InventoryManager.Instance.currentMouseItem.item;
        InventoryManager.Instance.currentMouseItem.item = tg;

        //refresh info
        itemInSlot.InitialiseItem(itemInSlot.item);
        InventoryManager.Instance.currentMouseItem.InitialiseItem(InventoryManager.Instance.currentMouseItem.item);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Select();
        InventoryManager.Instance.UpdateTooltips(itemInSlot);

        if(InventoryManager.Instance.currentMouseItem != null)
        {
            InventoryManager.Instance.currentMouseItem.parentAfterDrag = transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Deselect(); 
        InventoryManager.Instance.UpdateTooltips((InventoryItem)null);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(InventoryManager.Instance.isOpeningTheInventory)
        {
            if(InventoryManager.Instance.currentMouseItem != null)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    //if slot is empty, add all to slot
                    if(itemInSlot == null)
                        PlaceItem(itemInSlot);
                    //if it is not, and the slot item is the same as the holding item, then add
                    else if(InventoryManager.Instance.currentMouseItem.item == itemInSlot.item)
                    {
                        //if the sum out of MaxStackSize, add to the slot till it full stack
                        if(InventoryManager.Instance.currentMouseItem.count + itemInSlot.count > itemInSlot.item.MaxStackSize && itemInSlot.count < itemInSlot.item.MaxStackSize)
                        {
                            int quantity = Mathf.Abs(itemInSlot.item.MaxStackSize - itemInSlot.count);
                            AddItemToSlot(itemInSlot, InventoryManager.Instance.currentMouseItem, quantity);
                        }
                        //else, add all
                        else if(InventoryManager.Instance.currentMouseItem.count + itemInSlot.count <= itemInSlot.item.MaxStackSize)
                            AddItemToSlot(itemInSlot, InventoryManager.Instance.currentMouseItem, InventoryManager.Instance.currentMouseItem.count);
                        else if(itemInSlot.count == itemInSlot.item.MaxStackSize && InventoryManager.Instance.currentMouseItem.count < itemInSlot.item.MaxStackSize)
                            SwapItem();
                    }
                    else if(InventoryManager.Instance.currentMouseItem.item != itemInSlot.item)
                        SwapItem();
                }
                //else if press Right-click and (slot is empty, or slot item is the same as the holding item), add to the slot 
                else if(((itemInSlot == null) || (InventoryManager.Instance.currentMouseItem.item.Name == itemInSlot.item.Name)) && Input.GetMouseButtonDown(1))
                    AddItemToSlot(itemInSlot, InventoryManager.Instance.currentMouseItem, 1);
                
            }
            //else if not holding item, pick item, or split item
            else if(InventoryManager.Instance.currentMouseItem == null && itemInSlot != null)
            {
                if(Input.GetMouseButtonDown(0))
                    PickItem(itemInSlot);
                else if(Input.GetMouseButtonDown(1))
                    SplitItem(itemInSlot);
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
            InventoryManager.Instance.currentMouseItem.count -= quantity;
            InventoryManager.Instance.currentMouseItem.RefreshCount();
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
