using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    [SerializeField] private GameObject inventoryItemPrefab;
    [HideInInspector] public InventoryItem itemInSelectedSlot;
    [SerializeField] private Player player;

    [HideInInspector] public ItemTool tool;
    [HideInInspector] public ItemCrop crop;
    [HideInInspector] public ItemCropSeed cropSeed;

    [Space(10)]
    [HideInInspector] public int selectedSlot = -1;
    [HideInInspector] public bool isOpeningTheInventory = false;

    public ShopManager shopManagerScript;
    public QuestManager questManagerScript;
    public Furnace furnace;

    [HideInInspector] public InventoryItem currentMouseItem;

    [Space(20)]
    public Image toolTips;
    public Text toolTipsItemName;
    public Text toolTipsDescription;

    [Space(20)]
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

    void Start()
    {
        ChangeSelectedSlot(0);
        toolTips.enabled = false;
        toolTipsItemName.enabled = false;
        toolTipsDescription.enabled = false;

        EnableInventoryPage(inventory, inventoryButton);

        for(int i = 0; i < startItem.Count; i++)
        {
            AddItem(startItem[i]);
        }
    }

    void Update()
    {
        OpenTheInventory();
        HighlightCurrentSlot();

    }

    void OpenTheInventory()
    {
        //if none of these is opened, then can open inventory by pressing E key
        if(!pauseMenu.activeInHierarchy && !shopManagerScript.isOpeningTheShop && !questManagerScript.isOpeningTheQuestUI && !furnace.isOpeningTheFurnace)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(!isOpeningTheInventory)
                {
                    //Change the color of all slots to deselected color before closing the inventory, except the selectedSlot
                    for(int i = 0; i< inventorySlots.Length; i++)
                    {
                        if(inventorySlots[i] != inventorySlots[selectedSlot])
                        {
                            inventorySlots[i].Deselect();
                        }
                    }
                    inventoryGroup.SetActive(true);
                    EnableInventoryPage(inventory, inventoryButton);
                    isOpeningTheInventory = true;
                }
                else
                {
                    inventoryGroup.SetActive(false);
                    EnableInventoryPage(inventory, inventoryButton);
                    isOpeningTheInventory = false;
                }
            }
        }
    }

    void HighlightCurrentSlot()
    {
        if(currentMouseItem != null && currentMouseItem.count <= 0)
        {
            currentMouseItem = null;
        }
        else if(currentMouseItem != null)
        {
            currentMouseItem.transform.position = Input.mousePosition;
        }

        //Choosing slots by scrolling mouse
        if(!isOpeningTheInventory || !pauseMenu.activeInHierarchy)
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
            {
                ChangeSelectedSlot(number - 1);
            }
        }
        
        //Get the selected-slot items' info
        itemInSelectedSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();

        ChangeSelectedSlot(selectedSlot);
    }

    void ChangeSelectedSlot(int newValue)
    {
        if(selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        
        if(!pauseMenu.activeInHierarchy && !isOpeningTheInventory)
        {
            if(itemInSelectedSlot != null && !player.isDead)
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
                    default:
                        player.SetTempBox(false);
                        break;
                }
            }
            else if(itemInSelectedSlot == null)
            {
                player.SetTempBox(false);
            }
        }
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

    void HoldingTools(Item item)
    {
        tool = (ItemTool)item;

        switch(tool.toolType)
        {
            case ToolType.Pickaxe:
                player.PickaxeAction();
                break;
            case ToolType.Sword:
                player.SwordAction();
                break;
            case ToolType.Axe:
                player.AxeAction();
                break;
            case ToolType.Shovel:
                player.ShovelAction();
                break;
            case ToolType.Bucket:
                player.BucketAction();
                break;
        }
    }

    void HoldingCrops(Item item)
    {
        crop = (ItemCrop)item;
        player.CropAction();
    }

    void HoldingCropSeeds(Item item)
    {
        cropSeed = (ItemCropSeed)item;
        player.CropSeedAction();
    }

    //Update the tooltips infor when the player points at the items in the inventory
    public void UpdateTooltips(InventoryItem item, bool check)
    {
        if(item != null)
        {
            toolTips.enabled = check;
        }
        else
        {
            toolTips.enabled = false;
        }
        
        if(item != null)
        {
            toolTipsItemName.enabled = true;
            toolTipsItemName.text = item.item.Name;
            toolTipsDescription.enabled = check;
            toolTipsDescription.text = item.item.Descriptions;
        }
        else 
        {
            toolTipsItemName.enabled = false;
            toolTipsDescription.enabled = false;
        }
    }

    //Update tooltips infor when the player points at quest
    public void UpdateTooltips(Item item, bool check)
    {
        //Enable or disable tooltips
        if(item != null)
            toolTips.enabled = check;
        
        //Update info inside tooltips
        if(item != null)
        {
            toolTipsItemName.enabled = check;
            toolTipsItemName.text = item.Name;
        }
        else 
        {
            toolTipsItemName.enabled = false;
        }
    }

    public void OpenInventoryButton()
    {
        EnableInventoryPage(inventory, inventoryButton);
        AudioSource.PlayClipAtPoint(clickSFX, GameObject.Find("Main Camera").transform.position);
    }

    public void OpenUpgradeGroupButton()
    {
        EnableInventoryPage(upgradeGroup, upgradeButton);
        AudioSource.PlayClipAtPoint(clickSFX, GameObject.Find("Main Camera").transform.position);
    }

    public void EnableInventoryPage(GameObject inventoryPage, Button button)
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
