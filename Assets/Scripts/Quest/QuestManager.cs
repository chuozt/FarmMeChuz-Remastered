using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : Singleton<QuestManager>
{
    public GameObject questUI;
    public GameObject storageWhiteBorder;
    [SerializeField] private Scrollbar questScrollbar;

    private bool canOpenQuestUI = false;
    [HideInInspector] public bool isOpeningTheQuestUI = false;

    [Space(20)]
    public AudioClip sfxOpenQuest;
    public AudioClip sfxCloseQuest;

    void OnEnable() => Player.onPlayerDie += ToggleOffTheQuestUI;
    void OnDisable() => Player.onPlayerDie -= ToggleOffTheQuestUI;

    void Start()
    {
        questUI.SetActive(false);
        storageWhiteBorder.SetActive(false);
    }

    void Update()
    {
        if(!canOpenQuestUI)
            return;

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(!isOpeningTheQuestUI)
            {
                InventoryManager.Instance.ToggleOnTheInventory();
                ToggleOnTheQuestUI();
            }
            else
            {
                InventoryManager.Instance.ToggleOffTheInventory();
                ToggleOffTheQuestUI();
            }
        }
    }

    private void ToggleOnTheQuestUI()
    {
        questUI.SetActive(true);
        isOpeningTheQuestUI = true;
        questScrollbar.value = 1;
        AudioManager.Instance.PlaySFX(sfxOpenQuest);
    }

    private void ToggleOffTheQuestUI()
    {
        questUI.SetActive(false);
        isOpeningTheQuestUI = false;
        AudioManager.Instance.PlaySFX(sfxCloseQuest);
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
