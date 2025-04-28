using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject LoseScreen;
    [SerializeField] private GameObject WinScreen;

    public void Pause()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        LevelManager.GamePaused = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        LevelManager.GamePaused = false;

    }

    public void Restart()
    {
        LevelGenerator.Instance.Restart();
        LevelManager.Instance.RestartTimer();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadNextLevel(){
        LevelGenerator.Instance.GenerateNewLevel();
    }
    public void EnableLoseMenu() { LoseScreen.SetActive(true); Time.timeScale = 0;}
    public void DisableLoseMenu() { LoseScreen.SetActive(false);  Time.timeScale = 1;}
    public void EnableWinMenu() { WinScreen.SetActive(true); Time.timeScale = 0; }
    public void DisableWinMenu() { WinScreen.SetActive(false); Time.timeScale = 1; }

}
