using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField] private GameObject questUI;
    [SerializeField] private GameObject storageWhiteBorder;
    [SerializeField] private Scrollbar questScrollbar;

    private bool canOpenQuestUI = false;
    bool isOpeningTheQuestUI = false;
    public bool IsOpeningTheQuestUI => isOpeningTheQuestUI;

    [Space(20)]
    [SerializeField] private AudioClip sfxOpenQuest;
    [SerializeField] private AudioClip sfxCloseQuest;

    void OnEnable() => Player.onPlayerDie += ToggleOffTheQuestUI;
    void OnDisable() => Player.onPlayerDie -= ToggleOffTheQuestUI;

    void Start()
    {
        questUI.SetActive(false);
        storageWhiteBorder.SetActive(false);
    }

    void LateUpdate()
    {
        if(!canOpenQuestUI)
            return;

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(!isOpeningTheQuestUI)
            {
                InventoryManager.Instance.ToggleOnTheInventory();
                ToggleOnTheQuestUI();
                AudioManager.Instance.PlaySFX(sfxOpenQuest);
            }
            else
            {
                InventoryManager.Instance.ToggleOffTheInventory();
                ToggleOffTheQuestUI();
                AudioManager.Instance.PlaySFX(sfxCloseQuest);
            }
        }
    }

    private void ToggleOnTheQuestUI()
    {
        questUI.SetActive(true);
        isOpeningTheQuestUI = true;
        questScrollbar.value = 1;
    }

    private void ToggleOffTheQuestUI()
    {
        questUI.SetActive(false);
        isOpeningTheQuestUI = false;
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
            questUI.SetActive(false);
            isOpeningTheQuestUI = false;
            canOpenQuestUI = false;
            storageWhiteBorder.SetActive(false);
            InventoryManager.Instance.isOpeningTheInventory = false;
        }   
    }
}
