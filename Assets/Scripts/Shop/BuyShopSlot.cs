using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;

public class BuyShopSlot : ShopSlot_ParentClass
{
    protected override void Awake()
    {
        base.Awake();
        coinText.text = itemData.Cost.ToString();
    }

    public void BuyItem()
    {
        //if has enough money
        if(int.Parse(PlayerCoin.Instance.PlayerCoinText.text) >= itemData.Cost && itemData.IsUnlocked)
        {
            InventoryManager.Instance.AddItem(itemData);
            PlayerCoin.Instance.PlayerCoinText.text = (int.Parse(PlayerCoin.Instance.PlayerCoinText.text) - itemData.Cost).ToString();
            AudioManager.Instance.PlaySFX(sfx);
        }
        //else send error feedback
        else 
        {
            Feedback.Instance.StartCoroutine(Feedback.Instance.FeedbackTrigger("You don't have enough money!"));
            CameraShake.Instance.ShakeCamera();
        }
    }
}
