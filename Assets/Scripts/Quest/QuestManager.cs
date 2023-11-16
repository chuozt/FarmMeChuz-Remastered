using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject questUI;
    public GameObject inventoryGroup;
    public GameObject upgradeGroup;
    public InventoryManager inventoryManagerScript;

    private bool canOpenQuestUI = false;
    public bool isOpeningTheQuestUI = false;
    public GameObject storageWhiteBorder;

    [Space(20)]
    public AudioClip sfxOpenQuest;
    public AudioClip sfxCloseQuest;

    void Start()
    {
        questUI.SetActive(false);
        storageWhiteBorder.SetActive(false);
    }

    void Update()
    {
        if(canOpenQuestUI)
        {
            if(Input.GetKeyDown(KeyCode.F) && !isOpeningTheQuestUI && !inventoryManagerScript.isOpeningTheInventory)
            {
                AudioSource.PlayClipAtPoint(sfxOpenQuest, transform.position);
                questUI.SetActive(true);
                inventoryGroup.SetActive(true);
                inventoryManagerScript.EnableInventoryPage(inventoryManagerScript.inventory, inventoryManagerScript.inventoryButton);
                upgradeGroup.SetActive(false);
                isOpeningTheQuestUI = true;
                inventoryManagerScript.isOpeningTheInventory = true;
                
                //deselect all the slots, except the selected one
                for(int i = 0; i < inventoryManagerScript.inventorySlots.Length; i++)
                {
                    if(inventoryManagerScript.inventorySlots[i] != inventoryManagerScript.inventorySlots[inventoryManagerScript.selectedSlot])
                    {
                        inventoryManagerScript.inventorySlots[i].Deselect();
                    }
                }
            }
            else if(Input.GetKeyDown(KeyCode.F) && isOpeningTheQuestUI)
            {
                AudioSource.PlayClipAtPoint(sfxCloseQuest, transform.position);
                isOpeningTheQuestUI = false;
                inventoryManagerScript.isOpeningTheInventory = false;
                questUI.SetActive(false);
                inventoryGroup.SetActive(false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col) 
    {
        if(col.gameObject.name == "Player")
        {
            canOpenQuestUI = true;
            storageWhiteBorder.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col) 
    {
        if(col.gameObject.name == "Player")
        {
            canOpenQuestUI = false;
            storageWhiteBorder.SetActive(false);
            questUI.SetActive(false);
            inventoryManagerScript.isOpeningTheInventory = false;
            isOpeningTheQuestUI = false;
        }   
    }
}
