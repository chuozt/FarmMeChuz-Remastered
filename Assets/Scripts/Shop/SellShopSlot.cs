using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellShopSlot : ShopSlot_ParentClass
{
    public void SellItem()
    {
        //Sell one item
        if(inventoryManager.DecreaseItem(itemData) && itemData.IsUnlocked && !Input.GetKey(KeyCode.LeftShift))
        {
            playerCoinText.text = (int.Parse(playerCoinText.text) + itemData.Value).ToString();
            AudioSource.PlayClipAtPoint(sfx, GameObject.Find("Main Camera").transform.position);
        }
        //Sell all method
        else if(itemData.IsUnlocked && Input.GetKey(KeyCode.LeftShift))     
        {
            for(int i = 0; i < inventoryManager.inventorySlots.Length; i++)
            {
                if(inventoryManager.inventorySlots[i].itemInSlot != null &&
                 itemData == inventoryManager.inventorySlots[i].itemInSlot.item)
                {
                    playerCoinText.text = (int.Parse(playerCoinText.text) + itemData.Value * (inventoryManager.inventorySlots[i].itemInSlot.count + 1)).ToString();
                    inventoryManager.inventorySlots[i].itemInSlot.count = 0;
                    inventoryManager.inventorySlots[i].itemInSlot.RefreshCount();
                    audioManager.PlaySFX(sfx);
                }
            }
        }
        //Send error feedback
        else
        {
            feedback.StartCoroutine(feedback.FeedbackTrigger("You have no item to sell!"));
            cameraShake.ShakeCamera();
        }
    }
}
