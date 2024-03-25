using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCategory_CropSeed : Singleton<UpgradeCategory_CropSeed>
{
    [SerializeField] protected List<Image> lvImage;
    private int currentLevel = -1;
    public int CurrentLevel => currentLevel;
    [SerializeField] private float buffPercentage;
    public float BuffPercentage => buffPercentage;

    [SerializeField] protected AudioClip sfx;

    public virtual void UpgradeButton()
    {
        if(currentLevel < 9 && int.Parse(UpgradePoint.Instance.UpgradePointText.text) > 0)
        {
            currentLevel++;
            lvImage[currentLevel].color = Color.yellow;
            UpgradePoint.Instance.UpgradePointText.text = (int.Parse(UpgradePoint.Instance.UpgradePointText.text) - 1).ToString();
            AudioManager.Instance.PlaySFX(sfx);
        }
        else 
        {
            if(currentLevel >= 9)
                Feedback.Instance.StartCoroutine(Feedback.Instance.FeedbackTrigger("Max level reached!"));
            else if(int.Parse(UpgradePoint.Instance.UpgradePointText.text) <= 0)
                Feedback.Instance.StartCoroutine(Feedback.Instance.FeedbackTrigger("You have no point!"));

            CameraShake.Instance.ShakeCamera();
        }
    }
}
