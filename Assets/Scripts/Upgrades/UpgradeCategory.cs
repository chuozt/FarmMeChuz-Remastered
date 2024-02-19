using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCategory : MonoBehaviour
{
    public Text pointNumberText;
    public int currentLv = -1;
    public List<Image> lvImage;
    public UpgradeCategoryData upgradeCategoryData;

    [SerializeField] private AudioClip sfx;

    public void UpgradeButton()
    {
        if(currentLv < 9 && int.Parse(pointNumberText.text) > 0)
        {
            currentLv++;
            lvImage[currentLv].color = Color.yellow;
            pointNumberText.text = (int.Parse(pointNumberText.text) - 1).ToString();
            upgradeCategoryData.ApplyUpgrade();
            AudioSource.PlayClipAtPoint(sfx, GameObject.Find("Main Camera").transform.position, 0.5f);
        }
        else 
        {
            if(currentLv >= 9)
            {
                Feedback.Instance.StartCoroutine(Feedback.Instance.FeedbackTrigger("Max level reached!"));
            }
            else if(int.Parse(pointNumberText.text) <= 0)
            {
                Feedback.Instance.StartCoroutine(Feedback.Instance.FeedbackTrigger("You have no point!"));
            }
            CameraShake.Instance.ShakeCamera();
        }
    }
}
