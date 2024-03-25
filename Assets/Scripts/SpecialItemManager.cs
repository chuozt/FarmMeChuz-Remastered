using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpecialItemManager : Singleton<SpecialItemManager>
{
    [SerializeField] private GameObject specialPanel;
    [SerializeField] private Text specialItemText;

    private Item specialItem = null;
    private bool isOpeningTheSpecialPanel = false;
    public bool IsOpeningTheSpecialPanel => isOpeningTheSpecialPanel;

    public static event Action onUseHeartOfMotherland;

    void Start() => specialPanel.SetActive(false);

    public void ToggleOnTheSpecialPanel(Item _specialItem)
    {
        Player.Instance.SetInteractingState(false);
        isOpeningTheSpecialPanel = true;
        specialPanel.SetActive(true);
        Time.timeScale = 0;
        specialItem = _specialItem;
        specialItemText.text = specialItem.Name;
    }

    public void YesButton()
    {
        if(specialItem.Name == "Heart of Motherland")
        {
            Player.Instance.SetInteractingState(false);
            onUseHeartOfMotherland?.Invoke();
            InventoryManager.Instance.DecreaseItem(specialItem);
            StartCoroutine(Feedback.Instance.ShowLetter("You have used the HEART OF MOTHERLAND" + "\n\n" + "Trees are blessed with your kindness..."));
        }

        NoButton();
    }

    public void NoButton()
    {
        Player.Instance.SetInteractingState(true);
        isOpeningTheSpecialPanel = false;
        specialPanel.SetActive(false);
        Time.timeScale = 1;
        specialItem = null;
    }
}
