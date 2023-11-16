using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryManager inventoryManagerScript;
    public Item item;
    public int num;
    public int count = 0;
    public Text countText;
    public bool isDone = false;
    public Image blurImage;
    public QuestLevel questLevelScript;
    public int questIndex;
    private Feedback feedback;
    private CameraShake cameraShake;

    [Space(20)]
    public AudioClip sfx;

    void Start()
    {
        questLevelScript = this.transform.parent.parent.GetComponent<QuestLevel>();
        inventoryManagerScript = GameObject.Find("_InventoryManager").GetComponent<InventoryManager>();  
        feedback = GameObject.Find("Feedback").GetComponent<Feedback>();
        cameraShake = GameObject.Find("CM vcam1").GetComponent<CameraShake>();
        blurImage.enabled = false;
    }

    public void FinishQuest()
    {
        if(!isDone)
        {
            bool isHaveCrop = false;

            for(int i = 0; i < inventoryManagerScript.inventorySlots.Length; i++)
            {
                InventorySlot slot = inventoryManagerScript.inventorySlots[i];
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
            AudioSource.PlayClipAtPoint(sfx, GameObject.Find("Player").transform.position, 3);
            for(int i = 0; i < inventoryManagerScript.inventorySlots.Length; i++)
            {
                InventorySlot slot = inventoryManagerScript.inventorySlots[i];
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
        inventoryManagerScript.UpdateTooltips(item, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryManagerScript.UpdateTooltips(item, false);
    }

}
