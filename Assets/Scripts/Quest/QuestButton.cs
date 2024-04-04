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
    QuestLevel questLevel;

    [Space(20)]
    public AudioClip sfx;

    void Start()
    {
        questLevel = transform.parent.parent.GetComponent<QuestLevel>();
        blurImage.enabled = false;
    }

    public void FinishQuest()
    {
        if(!isDone)
        {
            bool isHaveCrop = false;

            for(int i = 0; i < InventoryManager.Instance.inventorySlots.Length; i++)
            {
                InventorySlot slot = InventoryManager.Instance.inventorySlots[i];
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
                        continue;
                }
            }

            if(!isHaveCrop || count < num)
            {
                Feedback.Instance.StartCoroutine(Feedback.Instance.FeedbackTrigger("You don't have enough materials to put in the storage!"));
                CameraShake.Instance.ShakeCamera();
            }
        }
        
        if(isDone)
        {
            blurImage.enabled = true;
            GetComponent<Button>().interactable = false;
            AudioManager.Instance.PlaySFX(sfx);
            
            for(int i = 0; i < InventoryManager.Instance.inventorySlots.Length; i++)
            {
                InventorySlot slot = InventoryManager.Instance.inventorySlots[i];
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
            questLevel.quest.IsDone[questIndex] = true;
            questLevel.CheckIfQuestLevelIsDone();
            count = 0;
            item = null;
            this.gameObject.GetComponent<Button>().enabled = false;
        }

        count = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.Instance.UpdateTooltips(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.UpdateTooltips((Item)null);
    }

}
