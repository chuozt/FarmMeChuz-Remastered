using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Feedback : Singleton<Feedback>
{
    [SerializeField] private Color feedbackColor;
    [SerializeField] private float duration;
    [SerializeField] private float secondsEachLetter;
    private Text feedbackText;
    private bool isTriggered = false;
    private bool isShowingCharByChar = false;

    void Start()
    {
        feedbackText = GetComponent<Text>();
        feedbackText.color = new Color(0,0,0,0);
    }

    public IEnumerator FeedbackTrigger(string feedbackString)
    {
        if(isShowingCharByChar)
            yield return 0;
        //if the text is not visible yet
        yield return new WaitForSeconds(0.05f);

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

    public IEnumerator ShowLetter(string specialText)
    {
        //if the text is not visible yet
        feedbackText.text = "";
        isShowingCharByChar = true;

        if(!isTriggered)
        {
            feedbackText.color = feedbackColor;
            isTriggered = true;
        }

        foreach(char character in specialText.ToCharArray())
        {
            feedbackText.text += character;
            yield return new WaitForSeconds(secondsEachLetter);
        }

        yield return new WaitForSeconds(1.5f);
        isShowingCharByChar = false;
        isTriggered = false;
        feedbackText.color = new Color(0,0,0,0);
    }
}
