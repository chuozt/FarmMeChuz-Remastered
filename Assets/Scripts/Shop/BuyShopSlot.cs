using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class BuyShopSlot : MonoBehaviour
{
    [SerializeField] private Item itemData;
    [SerializeField] private Text playerCoinTxt;
    [SerializeField] private Image itemPic;
    [SerializeField] private Text itemName;
    [SerializeField] private Text coinText;
    [SerializeField] private Image coinSprite;
    [SerializeField] private Text buy_sellText;
    [SerializeField] private Button buy_sellButton;
    [SerializeField] private Image blurImage;

    [Space(10)]
    [SerializeField] private InventoryManager inventoryManagerScript;

    [Space(10)]
    [SerializeField] private AudioClip sfx;

    [Space(10)]
    [SerializeField] private Feedback feedback;
    [SerializeField] private CameraShake cameraShake;

    void Awake()
    {
        playerCoinTxt = GameObject.Find("Player Coin Txt").GetComponent<Text>();
        itemPic.sprite = itemData.ItemSprite;
        itemName.text = itemData.Name;
        coinText.text = itemData.Cost.ToString();

        if(!itemData.IsUnlocked)
        {
            DisableWhenIsNotUnlocked();
        }
        else
        {
            EnableWhenUnlocked();
        }
    }

    void Update()
    {
        if(!itemData.IsUnlocked)
        {
            DisableWhenIsNotUnlocked();
        }
        else
        {
            EnableWhenUnlocked();
        }
    }

    public void BuyItem()
    {
        //if has enough money
        if(int.Parse(playerCoinTxt.text) >= itemData.Cost && itemData.IsUnlocked)
        {
            inventoryManagerScript.AddItem(itemData);
            playerCoinTxt.text = (int.Parse(playerCoinTxt.text) - itemData.Cost).ToString();
            AudioSource.PlayClipAtPoint(sfx, GameObject.Find("Main Camera").transform.position);
        }
        //else send error feedback
        else 
        {
            feedback.StartCoroutine(feedback.FeedbackTrigger("You don't have enough money!"));
            cameraShake.ShakeCamera();
        }
    }

    // public void BuyPoint()
    // {
    //     //if has enough money
    //     if(int.Parse(playerCoinTxt.text) >= itemData.Cost && itemData.IsUnlocked)
    //     {
    //         pointText.text = (int.Parse(pointText.text) + 1).ToString();
    //         playerCoinTxt.text = (int.Parse(playerCoinTxt.text) - itemData.Cost).ToString();
    //         AudioSource.PlayClipAtPoint(sfx, GameObject.Find("Main Camera").transform.position);
            
    //         //increase the point's cost each time the player buy
    //         itemData.Cost += 225;
    //         coinText.text = itemData.Cost.ToString();
    //     }
    //     //else send error feedback
    //     else 
    //     {
    //         feedback.StartCoroutine(feedback.FeedbackTrigger("You don't have enough money!"));
    //         cameraShake.ShakeCamera();
    //     }
    // }

    //Show the unlocked items' infor in the shop
    void EnableWhenUnlocked()
    {
        coinText.enabled = true;
        coinSprite.enabled = true;
        buy_sellButton.image.enabled = true;
        buy_sellText.enabled = true;
        itemName.enabled = true;
        blurImage.enabled = false;
    }

    //Hide the locked items' infor in the shop
    void DisableWhenIsNotUnlocked()
    {
        coinText.enabled = false;
        coinSprite.enabled = false;
        buy_sellButton.image.enabled = false;
        buy_sellText.enabled = false;
        itemName.enabled = false;
        blurImage.enabled = true;
    }
}
