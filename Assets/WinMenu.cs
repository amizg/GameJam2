using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public GameObject winMenuUI;
    public GameObject healthBar;

    public void Win()
    {
        healthBar.SetActive(false);
        winMenuUI.SetActive(true);
        Time.timeScale = 0.5f;
    }

    public void Restart()
    {
        healthBar.SetActive(true);
        winMenuUI.SetActive(false);
        SceneManager.LoadScene("Level");
        Time.timeScale = 1f;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
        Time.timeScale = 1f;
    }
}
