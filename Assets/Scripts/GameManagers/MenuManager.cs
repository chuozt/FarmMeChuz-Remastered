using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("InGameScene", LoadSceneMode.Single);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
