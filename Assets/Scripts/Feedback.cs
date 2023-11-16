using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Feedback : MonoBehaviour
{
    [SerializeField] private Color feedbackColor;
    [SerializeField] private float duration;
    private Text feedbackText;
    private bool isTriggered = false;

    void Start()
    {
        feedbackText = GetComponent<Text>();
        feedbackText.color = new Color(0,0,0,0);
    }

    public IEnumerator FeedbackTrigger(string feedbackString)
    {
        //if the text is not visible yet
        if(!isTriggered)
        {
            feedbackText.text = feedbackString;
            feedbackText.color = feedbackColor;
            isTriggered = true;
            yield return new WaitForSeconds(duration);
            isTriggered = false;
            feedbackText.color = new Color(0,0,0,0);
        }
    }
}
