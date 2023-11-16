using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public bool canShopNow = false;
    public bool isOpeningTheShop = false;

    public GameObject shopGroup;
    public GameObject inventoryGroup;
    public GameObject inventory;
    public GameObject upgradeGroup;
    public InventoryManager inventoryManagerScript;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private Color enableButtonColor;
    [SerializeField] private Color disableButtonColor;

    [Space(10)]
    public GameObject buyShop;
    public GameObject sellShop;

    [Space(10)]
    public GameObject shopWhiteBorder;

    [Space(20)]
    public AudioClip openSFX;
    public AudioClip closeSFX;
    public AudioClip clickSFX;

    void Start()
    {
        shopGroup.SetActive(false);
        shopWhiteBorder.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(canShopNow && Input.GetKeyDown(KeyCode.F) && !isOpeningTheShop && !inventoryManagerScript.isOpeningTheInventory)
        {
            shopGroup.SetActive(true);
            inventoryGroup.SetActive(true);
            inventory.SetActive(true);
            inventoryManagerScript.EnableInventoryPage(inventoryManagerScript.inventory, inventoryManagerScript.inventoryButton);
            upgradeGroup.SetActive(false);
            EnableShopPage(buyShop, buyButton);
            isOpeningTheShop = true;
            inventoryManagerScript.isOpeningTheInventory = true;
            AudioSource.PlayClipAtPoint(openSFX, transform.position);

            //deselect all the slots, except the selected one
            for(int i = 0; i < inventoryManagerScript.inventorySlots.Length; i++)
            {
                if(inventoryManagerScript.inventorySlots[i] != inventoryManagerScript.inventorySlots[inventoryManagerScript.selectedSlot])
                {
                    inventoryManagerScript.inventorySlots[i].Deselect();
                }
            }
        }
        else if(canShopNow && Input.GetKeyDown(KeyCode.F) && isOpeningTheShop)
        {
            shopGroup.SetActive(false);
            inventoryGroup.SetActive(false);
            inventory.SetActive(false);
            isOpeningTheShop = false;
            inventoryManagerScript.isOpeningTheInventory = false;
            AudioSource.PlayClipAtPoint(closeSFX, transform.position);
        }
    }

    public void BuyButton()
    {
        EnableShopPage(buyShop, buyButton);
        AudioSource.PlayClipAtPoint(clickSFX, GameObject.Find("Main Camera").transform.position, 2);
    }

    public void SellButton()
    {
        EnableShopPage(sellShop, sellButton);
        AudioSource.PlayClipAtPoint(clickSFX, GameObject.Find("Main Camera").transform.position, 2);
    }

    void EnableShopPage(GameObject shop, Button button)
    {
        buyShop.SetActive(false);
        sellShop.SetActive(false);
        shop.SetActive(true);

        buyButton.image.color = disableButtonColor;
        sellButton.image.color = disableButtonColor;
        button.image.color = enableButtonColor;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            canShopNow = true;
            shopWhiteBorder.GetComponent<SpriteRenderer>().enabled = true;
        }        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            canShopNow = false;
            isOpeningTheShop = false;
            shopGroup.SetActive(false);
            inventoryGroup.SetActive(false);
            inventoryManagerScript.isOpeningTheInventory = false;
            shopWhiteBorder.GetComponent<SpriteRenderer>().enabled = false;
            shopGroup.SetActive(false);
        }        
    }
}
