using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellShopSlot : MonoBehaviour
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
    public InventoryManager inventoryManagerScript;

    [Space(20)]
    public AudioClip sfx;

    [Space(10)]
    [SerializeField] private Feedback feedback;
    [SerializeField] private CameraShake cameraShake;

    void Awake()
    {
        playerCoinTxt = GameObject.Find("Player Coin Txt").GetComponent<Text>();
        itemPic.sprite = itemData.ItemSprite;
        itemName.text = itemData.Name;
        coinText.text = itemData.Value.ToString();

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

    public void SellItem()
    {
        if(inventoryManagerScript.DecreaseItem(itemData) && itemData.IsUnlocked && !Input.GetKey(KeyCode.LeftShift))
        {
            playerCoinTxt.text = (int.Parse(playerCoinTxt.text) + itemData.Value).ToString();
            AudioSource.PlayClipAtPoint(sfx, GameObject.Find("Main Camera").transform.position);
        }
        //Sell all method
        else if(itemData.IsUnlocked && Input.GetKey(KeyCode.LeftShift))     
        {
            for(int i = 0; i < inventoryManagerScript.inventorySlots.Length; i++)
            {
                if(inventoryManagerScript.inventorySlots[i].itemInSlot != null &&
                 itemData == inventoryManagerScript.inventorySlots[i].itemInSlot.item)
                {
                    playerCoinTxt.text = (int.Parse(playerCoinTxt.text) + itemData.Value * (inventoryManagerScript.inventorySlots[i].itemInSlot.count + 1)).ToString();
                    inventoryManagerScript.inventorySlots[i].itemInSlot.count = 0;
                    inventoryManagerScript.inventorySlots[i].itemInSlot.RefreshCount();
                    AudioSource.PlayClipAtPoint(sfx, GameObject.Find("Main Camera").transform.position);
                }
            }
        }
        else
        {
            feedback.StartCoroutine(feedback.FeedbackTrigger("You have no item to sell!"));
            cameraShake.ShakeCamera();
        }
    }

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
