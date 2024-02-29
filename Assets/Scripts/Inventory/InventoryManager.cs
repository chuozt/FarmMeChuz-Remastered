using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryManager : Singleton<InventoryManager>
{
    public InventorySlot[] inventorySlots;
    [SerializeField] private GameObject inventoryItemPrefab;
    [HideInInspector] public InventoryItem itemInSelectedSlot;
    private ItemTool itemTool;
    private ItemCrop itemCrop;
    private ItemCropSeed itemCropSeed;
    private Item itemSpecialItem;

    public ItemTool ItemTool => itemTool;
    public ItemCrop ItemCrop => itemCrop;
    public ItemCropSeed ItemCropSeed => itemCropSeed;
    public Item ItemSpecialItem => itemSpecialItem;

    [Space(10)]
    [HideInInspector] public int selectedSlot = -1;
    [HideInInspector] public bool isOpeningTheInventory = false;
    
    [Space(20), Header("- Tooltips Components -")]
    public Image toolTips;
    public Text toolTipsItemName;
    public Text toolTipsDescription;
    [HideInInspector] public InventoryItem currentMouseItem;

    [Space(20), Header("- UI Components -")]
    public GameObject pauseMenu;
    public GameObject inventoryGroup;
    public GameObject inventory;
    public GameObject upgradeGroup;
    public Button inventoryButton;
    public Button upgradeButton;
    [SerializeField] private Color enableButtonColor;
    [SerializeField] private Color disableButtonColor;
    [SerializeField] private AudioClip clickSFX;

    public List<Item> startItem;

    void OnEnable() => Player.onPlayerDie += ToggleOffTheInventory;
    void OnDisable() => Player.onPlayerDie -= ToggleOffTheInventory;

    void Start()
    {
        ChangeSelectedSlot(0);
        DisableTooltips();
        EnablePage(inventory, inventoryButton);

        //Starter pack
        for(int i = 0; i < startItem.Count; i++)
            AddItem(startItem[i]);
    }

    void Update()
    {
        HighlightCurrentSlot();
        UseHoldingItem();

        if(CheckCanOpenTheInventory())
        {
            if(!isOpeningTheInventory)
                ToggleOnTheInventory();
            else
                ToggleOffTheInventory();
        }
    }

    bool CheckCanOpenTheInventory()
    {
        //if none of these is opened, then can open inventory by pressing E key
        if(pauseMenu.activeInHierarchy || ShopManager.Instance.isOpeningTheShop || QuestManager.Instance.isOpeningTheQuestUI || Furnace.Instance.isOpeningTheFurnace)
            return false;

        if(Input.GetKeyDown(KeyCode.E))
            return true;

        return false;
    }

    public void ToggleOnTheInventory()
    {
        //Change the color of all slots to deselected color before closing the inventory, except the selectedSlot
        for(int i = 0; i< inventorySlots.Length; i++)
        {
            if(inventorySlots[i] != inventorySlots[selectedSlot])
                inventorySlots[i].Deselect();
        }

        isOpeningTheInventory = true;
        inventoryGroup.SetActive(true);
        EnablePage(inventory, inventoryButton);
        Player.Instance.CanMove = false;
    }

    public void ToggleOffTheInventory()
    {
        isOpeningTheInventory = false;
        inventoryGroup.SetActive(false);
        EnablePage(inventory, inventoryButton);
        Player.Instance.CanMove = true;
    }

    void HighlightCurrentSlot()
    {
        if(currentMouseItem != null && currentMouseItem.count <= 0)
            currentMouseItem = null;
        else if(currentMouseItem != null)
            currentMouseItem.transform.position = Input.mousePosition;

        //Choosing slots by scrolling mouse
        if(!isOpeningTheInventory && !pauseMenu.activeInHierarchy)
        {
            Vector2 mouseScroll = Input.mouseScrollDelta;
            if(mouseScroll.y < 0)
            {
                if(selectedSlot < 6)
                    ChangeSelectedSlot(selectedSlot + 1);
                else
                    ChangeSelectedSlot(0);
            }
            else if(mouseScroll.y > 0)
            {
                if(selectedSlot > 0)
                    ChangeSelectedSlot(selectedSlot - 1);
                else 
                    ChangeSelectedSlot(6);
            }
        }
        
        //Choosing slots with numbers on keyboard
        if(Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);

            if(isNumber && number > 0 && number < 8)
                ChangeSelectedSlot(number - 1);
        }

        //Get the selected-slot items' info
        ChangeSelectedSlot(selectedSlot);
    }

    void ChangeSelectedSlot(int newValue)
    {
        if(selectedSlot >= 0)
            inventorySlots[selectedSlot].Deselect();

        selectedSlot = newValue;
        inventorySlots[selectedSlot].Select();
        itemInSelectedSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();
    }

    void UseHoldingItem()
    {
        if(!pauseMenu.activeInHierarchy && !isOpeningTheInventory)
        {
            if(itemInSelectedSlot != null && !Player.Instance.IsDead)
            {
                switch(itemInSelectedSlot.item.ItemType)
                {
                    case ItemType.Tools:
                        HoldingTools(itemInSelectedSlot.item);
                        break;
                    case ItemType.Crop:
                        HoldingCrops(itemInSelectedSlot.item);
                        break;
                    case ItemType.CropSeed:
                        HoldingCropSeeds(itemInSelectedSlot.item);
                        break;
                    case ItemType.SpecialItems:
                        HoldingSpecialItems(itemInSelectedSlot.item);
                        break;
                    default:
                        Player.Instance.SetTempBox(false);
                        break;
                }
            }
            else if(itemInSelectedSlot == null)
                Player.Instance.SetTempBox(false);
        }
    }

    void HoldingTools(Item item)
    {
        itemTool = (ItemTool)item;

        switch(itemTool.toolType)
        {
            case ToolType.Pickaxe:
                Player.Instance.PickaxeAction();
                break;
            case ToolType.Sword:
                Player.Instance.SwordAction();
                break;
            case ToolType.Axe:
                Player.Instance.AxeAction();
                break;
            case ToolType.Shovel:
                Player.Instance.ShovelAction();
                break;
            case ToolType.Bucket:
                Player.Instance.BucketAction();
                break;
        }
    }

    void HoldingCrops(Item item)
    {
        itemCrop = (ItemCrop)item;
        Player.Instance.CropAction(itemCrop);
    }

    void HoldingCropSeeds(Item item)
    {
        itemCropSeed = (ItemCropSeed)item;
        Player.Instance.CropSeedAction(itemCropSeed);
    }

    void HoldingSpecialItems(Item item)
    {
        itemSpecialItem = item;
        Player.Instance.SpecialItemAction(itemSpecialItem);
    }

    public bool AddItem(Item item)
    {
        //if same type, and not full-stack, then add to the stack
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            slot.itemInSlot = itemInSlot;
            if(itemInSlot != null && itemInSlot.item == item && itemInSlot.count < item.MaxStackSize && item.IsStackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        
        //if not same type, add to new slot
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            slot.itemInSlot = itemInSlot;
            if(itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    public bool DecreaseItem(Item item)
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null && itemInSlot.item == item && itemInSlot.count > 0)
            {
                itemInSlot.count--;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    //Update the tooltips infor when the Player.Instance points at the items in the inventory
    public void UpdateTooltips(InventoryItem item)
    {
        if(item != null)
        {
            //Update item's name and descriptions
            toolTipsItemName.text = item.item.Name;
            toolTipsDescription.text = item.item.Descriptions;
            EnableTooltips();
        }
        else 
            DisableTooltips();
    }

    //Update tooltips infor when the Player.Instance points at quest
    public void UpdateTooltips(Item item)
    {
        //Update info inside tooltips
        if(item != null)
        {
            toolTips.enabled = true;
            toolTipsItemName.enabled = true;
            toolTipsItemName.text = item.Name;
        }
        else 
        {
            toolTips.enabled = false;
            toolTipsItemName.enabled = false;
        }
            
    }
    void EnableTooltips()
    {
        toolTips.enabled = true;
        toolTipsItemName.enabled = true;
        toolTipsDescription.enabled = true;
    }

    void DisableTooltips()
    {
        toolTips.enabled = false;
        toolTipsItemName.enabled = false;
        toolTipsDescription.enabled = false;
    }

    public void OpenInventoryButton()
    {
        EnablePage(inventory, inventoryButton);
        AudioManager.Instance.PlaySFX(clickSFX);
    }

    public void OpenUpgradeGroupButton()
    {
        EnablePage(upgradeGroup, upgradeButton);
        AudioManager.Instance.PlaySFX(clickSFX);
    }

    public void EnablePage(GameObject inventoryPage, Button button)
    {
        inventory.SetActive(false);
        upgradeGroup.SetActive(false);
        inventoryPage.SetActive(true);

        inventoryButton.image.color = disableButtonColor;
        upgradeButton.image.color = disableButtonColor;
        button.image.color = enableButtonColor;     
    }

    public void TrashCan()
    {
        //if currentMouseItem has any item, delete the item on the mouse
        if(currentMouseItem != null && currentMouseItem.item.ItemType != ItemType.Tools)
        {
            currentMouseItem.count = 0; 
            currentMouseItem.RefreshCount();
            Destroy(currentMouseItem);
        }
    }
}
