using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : Singleton<PauseManager>
{
    public GameObject pauseMenu;
    public GameObject instructionUI;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
            instructionUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Instructions()
    {
        instructionUI.SetActive(true);
    }

    public void Settings()
    {

    }

    public void Exit()
    {
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }
}
