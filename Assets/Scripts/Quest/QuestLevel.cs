using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestLevel : MonoBehaviour
{
    [Header("- Datas -")]
    public Quest quest;
    public Reward reward;

    [Space(20)]
    public Text levelText;
    public Text coinRewardText;
    public Text pointRewardText;

    [Space(10)]
    public GameObject questPrefab;
    public GameObject rewardPrefab;
    public Transform questArea;
    public Transform rewardArea;

    [Space(10)]
    public bool levelIsDone = false;
    public Image blurImage;

    Text playerCoinText;
    Text pointNumberText;
    AudioManager audioManager;

    [SerializeField] private AudioClip sfxQuestLevelComplete;

    void Start()
    {
        playerCoinText = GameObject.Find("PlayerCoinText").GetComponent<Text>();
        pointNumberText = GameObject.Find("PointNumberText").GetComponent<Text>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }
    
    void Awake()
    {
        blurImage.enabled = false;
        levelText.text = "Level " + quest.Level.ToString();
        
        for(int i = 0; i < quest.ItemsNeed.Count; i++)
        {
            quest.IsDone[i] = false;

            GameObject obj = Instantiate(questPrefab, this.transform.position, Quaternion.identity);
            obj.transform.parent = questArea;
            obj.GetComponent<QuestButton>().item = quest.ItemsNeed[i].ItemNeed;
            obj.GetComponent<QuestButton>().num = quest.ItemsNeed[i].NumbersOfItemNeed;
            obj.GetComponent<QuestButton>().questIndex = i;

            Transform child0 = obj.transform.GetChild(0);
            Transform child1 = obj.transform.GetChild(1);

            child0.GetComponent<Image>().sprite = quest.ItemsNeed[i].ItemNeed.ItemSprite;
            child1.GetComponent<Text>().text = "0/" + quest.ItemsNeed[i].NumbersOfItemNeed.ToString();
        }

        for(int i = 0; i < reward.ItemsUnlock.Count; i++)
        {
            reward.ItemsUnlock[i].IsUnlocked = false;
            GameObject obj = Instantiate(rewardPrefab, this.transform.position, Quaternion.identity);
            obj.transform.parent = rewardArea;
            Transform child = obj.transform.GetChild(0);
            child.GetComponentInChildren<Image>().sprite = reward.ItemsUnlock[i].ItemSprite;
        }

        coinRewardText.text = reward.Coin.ToString();
        pointRewardText.text = "+ " + reward.Point.ToString() + " point(s)";
    }

    public void CheckIfQuestLevelIsDone()
    {
        for(int i = 0; i < quest.ItemsNeed.Count; i++)
        {
            if(quest.IsDone[i])
            {
                levelIsDone = true;
                continue;
            }

            if(!quest.IsDone[i])
            {
                levelIsDone = false;
                break;
            }
        }

        if(levelIsDone)
        {
            for(int i = 0; i < reward.ItemsUnlock.Count; i++)
                reward.ItemsUnlock[i].IsUnlocked = true;

            blurImage.enabled = true;
            playerCoinText.text = (int.Parse(playerCoinText.text) + reward.Coin).ToString();
            pointNumberText.text = (int.Parse(pointNumberText.text) + reward.Point).ToString();
            audioManager.PlaySFX(sfxQuestLevelComplete);
        }
    }
}
