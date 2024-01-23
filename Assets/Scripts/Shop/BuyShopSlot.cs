using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;

public class BuyShopSlot : ShopSlot_ParentClass
{
    public void BuyItem()
    {
        //if has enough money
        if(int.Parse(playerCoinText.text) >= itemData.Cost && itemData.IsUnlocked)
        {
            inventoryManager.AddItem(itemData);
            playerCoinText.text = (int.Parse(playerCoinText.text) - itemData.Cost).ToString();
            audioManager.PlaySFX(sfx);
        }
        //else send error feedback
        else 
        {
            feedback.StartCoroutine(feedback.FeedbackTrigger("You don't have enough money!"));
            cameraShake.ShakeCamera();
        }
    }
}
