using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellShopSlot : ShopSlot_ParentClass
{
    protected override void Awake()
    {
        base.Awake();
        coinText.text = itemData.Value.ToString();
    }
    
    public void SellItem()
    {
        //Sell one item
        if(InventoryManager.Instance.DecreaseItem(itemData) && itemData.IsUnlocked && !Input.GetKey(KeyCode.LeftShift))
        {
            PlayerCoin.Instance.PlayerCoinText.text = (int.Parse(PlayerCoin.Instance.PlayerCoinText.text) + itemData.Value).ToString();
            AudioManager.Instance.PlaySFX(sfx);
        }
        //Sell all method
        else if(itemData.IsUnlocked && Input.GetKey(KeyCode.LeftShift))     
        {
            for(int i = 0; i < InventoryManager.Instance.inventorySlots.Length; i++)
            {
                if(InventoryManager.Instance.inventorySlots[i].itemInSlot != null &&
                 itemData == InventoryManager.Instance.inventorySlots[i].itemInSlot.item)
                {
                    PlayerCoin.Instance.PlayerCoinText.text = (int.Parse(PlayerCoin.Instance.PlayerCoinText.text) + itemData.Value * (InventoryManager.Instance.inventorySlots[i].itemInSlot.count + 1)).ToString();
                    InventoryManager.Instance.inventorySlots[i].itemInSlot.count = 0;
                    InventoryManager.Instance.inventorySlots[i].itemInSlot.RefreshCount();
                    AudioManager.Instance.PlaySFX(sfx);
                }
            }
        }
        //Send error feedback
        else
        {
            Feedback.Instance.StartCoroutine(Feedback.Instance.FeedbackTrigger("You have no item to sell!"));
            CameraShake.Instance.ShakeCamera();
        }
    }
}
