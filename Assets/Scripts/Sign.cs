using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    [SerializeField] private Text signsText;
    [TextArea(1, 3)]
    [SerializeField] private string textToAdd;
    [SerializeField] private GameObject whiteBorder;
    [SerializeField] private float secondsEachLetter;
    bool canTriggerSign = false;
    bool isShowingText = false;

    void Start()
    {
        whiteBorder.SetActive(false);
        signsText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(canTriggerSign)
        {
            if(Input.GetKeyDown(KeyCode.F) && !isShowingText)
            {
                signsText.text = "";
                signsText.enabled = true;
                isShowingText = true;
                StartCoroutine(ShowLetter());
            }
            else if(Input.GetKeyDown(KeyCode.F) && isShowingText)
            {
                signsText.enabled = false;
                isShowingText = false;
                StopAllCoroutines();
                signsText.text = "";
            }
        }
    }

    IEnumerator ShowLetter()
    {
        foreach(char character in textToAdd.ToCharArray())
        {
            signsText.text += character;
            yield return new WaitForSeconds(secondsEachLetter);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            canTriggerSign = true;
            whiteBorder.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            canTriggerSign = false;
            whiteBorder.SetActive(false);
            isShowingText = false;
            signsText.enabled = false;
            StopAllCoroutines();
        }
    }
}
