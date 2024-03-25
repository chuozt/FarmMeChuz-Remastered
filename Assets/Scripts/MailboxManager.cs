using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailboxManager : Singleton<MailboxManager>
{
    [SerializeField] private GameObject mailBoxEmpty;
    [SerializeField] private GameObject mailBoxHasMailGroup;
    [SerializeField] private GameObject mailBoxBorder;
    [SerializeField] private GameObject mailBoxThinkingBubble;
    [SerializeField] private GameObject mailUI;
    [SerializeField] private Text requestText;
    [SerializeField] private Text signatureText;
    [SerializeField] private Image requestItemImage;
    [SerializeField] private Text requestNumberText;
    [SerializeField] private Text playersNumberOfItemText;
    [SerializeField] private AudioClip sfxOpenMail;
    [SerializeField] private AudioClip sfxCloseMail;
    [SerializeField] private AudioClip sfxSendMail;
    [SerializeField] private List<SO_MailRequest> mailRequestList;

    bool canOpenMailBox = false;
    bool isHavingMail = false;
    bool hasCheckedTheMail = false;
    bool isOpeningTheMail = false;
    public bool IsOpeningTheMail => isOpeningTheMail;
    SO_MailRequest mailRequest;
    int requestNumber = 0;
    int totalGainedGold = 0;
    Color playersNumberOfItemTextColor;

    void OnEnable() => Player.onPlayerDie += ToggleOffTheMailUI;
    void OnDisable() => Player.onPlayerDie -= ToggleOffTheMailUI;

    void Start()
    {
        playersNumberOfItemTextColor = playersNumberOfItemText.color;
        ToggleOffTheMailUI();
    }

    void Awake()
    {
        mailBoxHasMailGroup.SetActive(false);
        mailBoxBorder.SetActive(false);
        GenerateMail();
    }
    
    void Update()
    {
        if(canOpenMailBox && Input.GetKeyDown(KeyCode.F))
        {
            if(!hasCheckedTheMail)
                mailBoxThinkingBubble.SetActive(false);

            if(!isOpeningTheMail)
            {
                InventoryManager.Instance.ToggleOnTheInventory();
                ToggleOnTheMailUI();
                AudioManager.Instance.PlaySFX(sfxOpenMail);
            }
            else
            {
                InventoryManager.Instance.ToggleOffTheInventory();
                ToggleOffTheMailUI();
                AudioManager.Instance.PlaySFX(sfxCloseMail);
            }
        }
    }

    void FixedUpdate()
    {
        if(!isOpeningTheMail)
            return;

        playersNumberOfItemText.text = "You have: " + InventoryManager.Instance.CountTotalNumberOfAnItem(mailRequest.RequestItem);
        if(InventoryManager.Instance.CountTotalNumberOfAnItem(mailRequest.RequestItem) < requestNumber)
            playersNumberOfItemText.color = Color.red;
        else
            playersNumberOfItemText.color = playersNumberOfItemTextColor;
    }

    void ToggleOnTheMailUI()
    {
        isOpeningTheMail = true;
        mailUI.SetActive(true);
    }

    void ToggleOffTheMailUI()
    {
        isOpeningTheMail = false;
        mailUI.SetActive(false);
    }

    [ContextMenu("GenerateMail")]
    void GenerateMail()
    {
        // if(!(DayNightManager.Instance.CurrentDay % 3 == 0) || !(DayNightManager.Instance.CurrentDay % 4 == 0))
        //     return;
        
        int randomNumber = Random.Range(0, 3);

        if(randomNumber == 0)
        {
            //Update mailbox's state
            isHavingMail = true;
            hasCheckedTheMail = false;
            mailBoxEmpty.SetActive(false);
            mailBoxHasMailGroup.SetActive(true);
            mailBoxThinkingBubble.SetActive(true);

            //Update request info
            mailRequest = mailRequestList[Random.Range(0, mailRequestList.Count)];

            while(!mailRequest.RequestItem.IsUnlocked)
                mailRequest = mailRequestList[Random.Range(0, mailRequestList.Count)];

            requestNumber = Random.Range(mailRequest.MinRequestNumber, mailRequest.MaxRequestNumber + 1);
            float randomGoldBuff = Random.Range(1.2f, 1.5f);
            totalGainedGold = (int)(requestNumber * mailRequest.RequestItem.Value * randomGoldBuff);

            requestText.text = "Hi, I would like to have some " + mailRequest.RequestItem.Name + " to " + mailRequest.RequestReason + 
                               ". Could I buy " + requestNumber +  " from you for " + totalGainedGold + " GOLDS?";
            signatureText.text = "- " + mailRequest.Signature + " -";
            requestItemImage.sprite = mailRequest.RequestItem.ItemSprite;
            requestNumberText.text = "x" + requestNumber;
        }
    }

    public void SendItem()
    {
        if(InventoryManager.Instance.CountTotalNumberOfAnItem(mailRequest.RequestItem) >= requestNumber)
        {
            for(int i = 0; i < requestNumber; i++)
                InventoryManager.Instance.DecreaseItem(mailRequest.RequestItem);
            
            PlayerCoin.Instance.PlayerCoinText.text = (int.Parse(PlayerCoin.Instance.PlayerCoinText.text) + totalGainedGold).ToString();
            InventoryManager.Instance.ToggleOffTheInventory();
            ToggleOffTheMailUI();
            ResetToInitialState();
            //AudioManager.Instance.PlaySFX(sfxSendMail);
        }
        else
        {
            CameraShake.Instance.ShakeCamera();
            StartCoroutine(Feedback.Instance.FeedbackTrigger("You don't have enough materials to send!"));
        }
    }

    void ResetToInitialState()
    {
        canOpenMailBox = false;
        isHavingMail = false;
        isOpeningTheMail = false;
        mailRequest = null;
        requestNumber = 0;
        totalGainedGold = 0;
        mailBoxEmpty.SetActive(true);
        mailBoxHasMailGroup.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player") && isHavingMail)
        {
            canOpenMailBox = true;
            mailBoxBorder.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            canOpenMailBox = false;
            mailBoxBorder.SetActive(false);
        }
    }
}
