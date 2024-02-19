    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    [HideInInspector] public bool canShopNow = false;
    [HideInInspector] public bool isOpeningTheShop = false;

    public GameObject shopGroup;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private Color enableButtonColor;
    [SerializeField] private Color disableButtonColor;

    [Space(10)]
    public GameObject buyShop;
    public GameObject sellShop;
    [SerializeField] private Scrollbar shopScrollbar;

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

    void Update()
    {
        if(canShopNow && Input.GetKeyDown(KeyCode.F) && !isOpeningTheShop && !InventoryManager.Instance.isOpeningTheInventory)
        {
            InventoryManager.Instance.ToggleOnTheInventory();
            ToggleOnShopUI();
            EnableShopPage(buyShop, buyButton);
        }
        else if(canShopNow && Input.GetKeyDown(KeyCode.F) && isOpeningTheShop)
        {
            InventoryManager.Instance.ToggleOffTheInventory();
            ToggleOffShopUI();
        }
    }

    public void BuyButton()
    {
        EnableShopPage(buyShop, buyButton);
        AudioManager.Instance.PlaySFX(clickSFX);
    }

    public void SellButton()
    {
        EnableShopPage(sellShop, sellButton);
        AudioManager.Instance.PlaySFX(clickSFX);
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
            ToggleOffShopUI();
            canShopNow = false;
            shopWhiteBorder.GetComponent<SpriteRenderer>().enabled = false;
        }        
    }

    void ToggleOnShopUI()
    {
        shopGroup.SetActive(true);
        isOpeningTheShop = true;
        shopScrollbar.value = 1;
        AudioManager.Instance.PlaySFX(openSFX);
    }

    void ToggleOffShopUI()
    {
        shopGroup.SetActive(false);
        isOpeningTheShop = false;
        AudioManager.Instance.PlaySFX(closeSFX);
    }
}
