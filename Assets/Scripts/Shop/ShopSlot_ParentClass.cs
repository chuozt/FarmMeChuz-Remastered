using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;

public class ShopSlot_ParentClass : MonoBehaviour
{
    [Header("- Slot Components -")]
    [SerializeField] protected Item itemData;
    [SerializeField] protected Image itemPic;
    [SerializeField] protected Text itemName;
    [SerializeField] protected Text coinText;
    [SerializeField] protected Image coinSprite;
    [SerializeField] protected Text buy_sellText;
    [SerializeField] protected Button buy_sellButton;
    [SerializeField] protected Image blurImage;
    
    [Space(10)]
    [SerializeField] protected AudioClip sfx;

    protected virtual void Awake()
    {        
        itemPic.sprite = itemData.ItemSprite;
        itemName.text = itemData.Name;

        if(!itemData.IsUnlocked)
            DisableWhenIsNotUnlocked();
        else
            EnableWhenUnlocked();
    }

    protected void Update()
    {
        if(!itemData.IsUnlocked)
            DisableWhenIsNotUnlocked();
        else
            EnableWhenUnlocked();
    }

    //Show the unlocked items' infor in the shop
    protected void EnableWhenUnlocked()
    {
        coinText.enabled = true;
        coinSprite.enabled = true;
        buy_sellButton.image.enabled = true;
        buy_sellText.enabled = true;
        itemName.enabled = true;
        blurImage.enabled = false;
    }

    //Hide the locked items' infor in the shop
    protected void DisableWhenIsNotUnlocked()
    {
        coinText.enabled = false;
        coinSprite.enabled = false;
        buy_sellButton.image.enabled = false;
        buy_sellText.enabled = false;
        itemName.enabled = false;
        blurImage.enabled = true;
    }
}
