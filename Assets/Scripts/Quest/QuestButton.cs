using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    [HideInInspector] public Item item;
    [HideInInspector] public int num;
    [HideInInspector] public int count = 0;
    public Text countText;
    [HideInInspector] public bool isDone = false;
    public Image blurImage;
    
    public int questIndex;

    //Managers
    QuestLevel questLevelScript;
    InventoryManager inventoryManager;
    Feedback feedback;
    CameraShake cameraShake;
    AudioManager audioManager;

    [Space(20)]
    public AudioClip sfx;

    void Start()
    {
        questLevelScript = transform.parent.parent.GetComponent<QuestLevel>();
        inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();  
        feedback = GameObject.FindGameObjectWithTag("FeedbackText").GetComponent<Feedback>();
        cameraShake = GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<CameraShake>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        blurImage.enabled = false;
    }

    public void FinishQuest()
    {
        if(!isDone)
        {
            bool isHaveCrop = false;

            for(int i = 0; i < inventoryManager.inventorySlots.Length; i++)
            {
                InventorySlot slot = inventoryManager.inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if(itemInSlot != null && itemInSlot.item == item && count < num)
                {
                    isHaveCrop = true;
                    count += itemInSlot.count;

                    if(count >= num)
                    {
                        countText.text = num.ToString() + "/" + num.ToString();
                        isDone = true;
                        count = num;
                        break;
                    }
                    else
                    {
                        feedback.StartCoroutine(feedback.FeedbackTrigger("You don't have enough crops to put in the storage!"));
                        cameraShake.ShakeCamera();
                    }
                }
            }

            if(!isHaveCrop)
            {
                feedback.StartCoroutine(feedback.FeedbackTrigger("You don't have enough crops to put in the storage!"));
                cameraShake.ShakeCamera();
            }
        }
        
        if(isDone)
        {
            blurImage.enabled = true;
            audioManager.PlaySFX(sfx);
            
            for(int i = 0; i < inventoryManager.inventorySlots.Length; i++)
            {
                InventorySlot slot = inventoryManager.inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if(itemInSlot != null && itemInSlot.item == item && itemInSlot.count < count)
                {
                    count -= itemInSlot.count;
                    itemInSlot.count -= itemInSlot.count;
                    itemInSlot.RefreshCount();
                    continue;
                }
                else if(itemInSlot != null && itemInSlot.item == item && itemInSlot.count >= count)
                {
                    itemInSlot.count -= count;
                    itemInSlot.RefreshCount();
                    break;
                }
            }
            questLevelScript.quest.IsDone[questIndex] = true;
            questLevelScript.CheckIfQuestLevelIsDone();
            count = 0;
            item = null;
            this.gameObject.GetComponent<Button>().enabled = false;
        }

        count = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryManager.UpdateTooltips(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryManager.UpdateTooltips(item);
    }

}
