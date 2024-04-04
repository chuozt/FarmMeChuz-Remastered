using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject instructionPage;
    [SerializeField] private GameObject settingsPage;
    [SerializeField] private AudioClip sfxClick;

    private bool isOpeningThePauseMenu = false;
    public bool IsOpeningThePauseMenu => isOpeningThePauseMenu;

    void OnEnable() => Player.onPlayerDie += ToggleOffThePauseMenu;

    void OnDisable() => Player.onPlayerDie -= ToggleOffThePauseMenu;

    void Start()
    {
        pauseMenu.SetActive(false);
        instructionPage.SetActive(false);
        settingsPage.SetActive(false);
    }

    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !pauseMenu.activeInHierarchy)
            ToggleOnThePauseMenu();
        else if(Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeInHierarchy)
            ToggleOffThePauseMenu();
    }

    void ToggleOnThePauseMenu()
    {
        Player.Instance.SetInteractingState(false);
        isOpeningThePauseMenu = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ToggleOffThePauseMenu()
    {
        Player.Instance.SetInteractingState(true);
        isOpeningThePauseMenu = false;
        pauseMenu.SetActive(false);
        instructionPage.SetActive(false);
        settingsPage.SetActive(false);
        Time.timeScale = 1;
    }

    public void Resume()
    {
        Player.Instance.SetInteractingState(true);
        isOpeningThePauseMenu = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioManager.Instance.PlaySFX(sfxClick);
    }

    public void Instructions()
    {
        instructionPage.SetActive(true);
        AudioManager.Instance.PlaySFX(sfxClick);
    } 

    public void Settings()
    {
        settingsPage.SetActive(true);
        AudioManager.Instance.PlaySFX(sfxClick);
    } 

    public void Exit()
    {
        Time.timeScale = 1;
        AudioManager.Instance.PlaySFX(sfxClick);
        SceneManager.LoadScene("MenuScene");
    }
}
