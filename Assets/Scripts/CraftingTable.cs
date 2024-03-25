using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftingTable : Singleton<CraftingTable>
{
    [SerializeField] private GameObject craftingTableWhiteBorder;
    [SerializeField] private GameObject craftingTableUI;
    [SerializeField] private Image craftingSlot_ItemImage1, craftingSlot_ItemImage2, output_ItemImage;
    [SerializeField] private Text craftingText1, craftingText2;
    [SerializeField] private AudioClip sfxShowRecipe, sfxCraft;
    
    private Item craftingItem = null;
    private Item neededMaterial1 = null, neededMaterial2 = null;
    private int numberOfMaterial1 = 0, numberOfMaterial2 = 0;
    private bool canUseCraftingTable = false;
    private bool isOpeningCraftingTable = false;
    public bool IsOpeningCraftingTable => isOpeningCraftingTable;
    Color originalTextColor;

    void OnEnable() => Player.onPlayerDie += ToggleOffTheCraftingTableUI;
    void OnDisable() => Player.onPlayerDie -= ToggleOffTheCraftingTableUI;

    void Start()
    {
        originalTextColor = craftingText1.color;
        craftingTableUI.SetActive(false);
        craftingTableWhiteBorder.SetActive(false);
    }

    void Awake()
    {
        ToggleOffTheRecipeRow();
    }

    void LateUpdate()
    {
        if(!canUseCraftingTable)
            return;
        
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(!isOpeningCraftingTable)
            {
                // StartCoroutine(Feedback.Instance.FeedbackTrigger("This mechanic will be updated in the Full Version!"));
                // CameraShake.Instance.ShakeCamera();
                InventoryManager.Instance.ToggleOnTheInventory();
                ToggleOnTheCraftingTableUI();
            }
            else
            {
                InventoryManager.Instance.ToggleOffTheInventory();
                ToggleOffTheCraftingTableUI();
            }
        }

        if(craftingItem == null)
            return;
        
        if(craftingItem.CraftingRecipes.Count == 1)
        {
            numberOfMaterial1 = 0;
            for(int i = 0; i < InventoryManager.Instance.inventorySlots.Length; i++)
            {
                if(InventoryManager.Instance.inventorySlots[i].itemInSlot == null)
                    continue;

                if(neededMaterial1 == InventoryManager.Instance.inventorySlots[i].itemInSlot.item)
                    numberOfMaterial1 += InventoryManager.Instance.inventorySlots[i].itemInSlot.count;
            }
            craftingText1.text = numberOfMaterial1 + "/" + craftingItem.CraftingRecipes[0].NumberOfNeededMaterial.ToString();
        }
        else if(craftingItem.CraftingRecipes.Count == 2)
        {
            numberOfMaterial1 = 0;
            numberOfMaterial2 = 0;
            for(int i = 0; i < InventoryManager.Instance.inventorySlots.Length; i++)
            {
                if(InventoryManager.Instance.inventorySlots[i].itemInSlot == null)
                    continue;

                if(neededMaterial1 == InventoryManager.Instance.inventorySlots[i].itemInSlot.item)
                    numberOfMaterial1 += InventoryManager.Instance.inventorySlots[i].itemInSlot.count;
                else if(neededMaterial2 == InventoryManager.Instance.inventorySlots[i].itemInSlot.item)
                    numberOfMaterial2 += InventoryManager.Instance.inventorySlots[i].itemInSlot.count;
            }
            craftingText1.text = numberOfMaterial1 + "/" + craftingItem.CraftingRecipes[0].NumberOfNeededMaterial.ToString();
            craftingText2.text = numberOfMaterial2 + "/" +craftingItem.CraftingRecipes[1].NumberOfNeededMaterial.ToString();

            if(numberOfMaterial2 < craftingItem.CraftingRecipes[1].NumberOfNeededMaterial)
                craftingText2.color = Color.red;
            else
                craftingText2.color = originalTextColor;
        }

        if(numberOfMaterial1 < craftingItem.CraftingRecipes[0].NumberOfNeededMaterial)
            craftingText1.color = Color.red;
        else
            craftingText1.color = originalTextColor;
    }

    void ToggleOnTheCraftingTableUI()
    {
        isOpeningCraftingTable = true;
        craftingTableUI.SetActive(true);
    }

    void ToggleOffTheCraftingTableUI()
    {
        isOpeningCraftingTable = false;
        craftingTableUI.SetActive(false);
        ToggleOffTheRecipeRow();
    }

    void ToggleOnTheRecipeRow()
    {
        craftingSlot_ItemImage1.enabled = true;
        craftingSlot_ItemImage2.enabled = true;
        craftingText1.enabled = true;
        craftingText2.enabled = true;
        output_ItemImage.enabled = false;
    }

    void ToggleOffTheRecipeRow()
    {
        neededMaterial1 = null;
        neededMaterial2 = null;
        craftingSlot_ItemImage1.enabled = false;
        craftingSlot_ItemImage2.enabled = false;
        craftingText1.enabled = false;
        craftingText2.enabled = false;
        craftingItem = null;
        output_ItemImage.sprite = null;
    }

    public void ShowRecipe()
    {
        if(!isOpeningCraftingTable)
            return;

        craftingItem = EventSystem.current.currentSelectedGameObject.GetComponent<CraftingButton>().CraftingItem;
        output_ItemImage.sprite = craftingItem.ItemSprite;
        output_ItemImage.enabled = true;
        AudioManager.Instance.PlaySFX(sfxShowRecipe);
        
        if(craftingItem.CraftingRecipes.Count == 1)
        {
            neededMaterial1 = craftingItem.CraftingRecipes[0].NeededMaterial;
            craftingSlot_ItemImage1.sprite = craftingItem.CraftingRecipes[0].NeededMaterial.ItemSprite;

            craftingSlot_ItemImage1.enabled = true;
            craftingSlot_ItemImage2.enabled = false;
            craftingText1.enabled = true;
            craftingText2.enabled = false;
        }
        else if(craftingItem.CraftingRecipes.Count == 2)
        {
            neededMaterial1 = craftingItem.CraftingRecipes[0].NeededMaterial;
            neededMaterial2 = craftingItem.CraftingRecipes[1].NeededMaterial;
            craftingSlot_ItemImage1.sprite = craftingItem.CraftingRecipes[0].NeededMaterial.ItemSprite;
            craftingSlot_ItemImage2.sprite = craftingItem.CraftingRecipes[1].NeededMaterial.ItemSprite;

            craftingSlot_ItemImage1.enabled = true;
            craftingSlot_ItemImage2.enabled = true;
            craftingText1.enabled = true;
            craftingText2.enabled = true;
        }
    }

    public void CraftButton()
    {
        if(craftingItem == null)
            return;

        //If the recipe only has 1 needed material
        if(craftingItem.CraftingRecipes.Count == 1)
        {
            if(InventoryManager.Instance.IsFullInventory(craftingItem))
            {
                StartCoroutine(Feedback.Instance.FeedbackTrigger("Your inventory is full!"));
                CameraShake.Instance.ShakeCamera();
            }
            
            if(numberOfMaterial1 >= craftingItem.CraftingRecipes[0].NumberOfNeededMaterial)
            {
                for(int i = 0; i < craftingItem.CraftingRecipes[0].NumberOfNeededMaterial; i++)
                    InventoryManager.Instance.DecreaseItem(neededMaterial1);
                
                InventoryManager.Instance.AddItem(craftingItem);
                AudioManager.Instance.PlaySFX(sfxCraft);
            }
            else
            {
                StartCoroutine(Feedback.Instance.FeedbackTrigger("You don't have enough materials!"));
                CameraShake.Instance.ShakeCamera();
            }
        }
        //If the recipe has 2 needed materials
        else if(craftingItem.CraftingRecipes.Count == 2)
        {
            if(InventoryManager.Instance.IsFullInventory(craftingItem))
            {
                StartCoroutine(Feedback.Instance.FeedbackTrigger("Your inventory is full!"));
                CameraShake.Instance.ShakeCamera();
            }

            if(numberOfMaterial1 >= craftingItem.CraftingRecipes[0].NumberOfNeededMaterial &&
               numberOfMaterial2 >= craftingItem.CraftingRecipes[1].NumberOfNeededMaterial)
            {
                for(int i = 0; i < craftingItem.CraftingRecipes[0].NumberOfNeededMaterial; i++)
                    InventoryManager.Instance.DecreaseItem(neededMaterial1);

                for(int i = 0; i < craftingItem.CraftingRecipes[1].NumberOfNeededMaterial; i++)
                    InventoryManager.Instance.DecreaseItem(neededMaterial2);

                InventoryManager.Instance.AddItem(craftingItem);
                AudioManager.Instance.PlaySFX(sfxCraft);
            }
            else
            {
                StartCoroutine(Feedback.Instance.FeedbackTrigger("You don't have enough materials!"));
                CameraShake.Instance.ShakeCamera();
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            canUseCraftingTable = true;
            craftingTableWhiteBorder.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            canUseCraftingTable = false;
            craftingTableWhiteBorder.SetActive(false);
        }
    }
}
