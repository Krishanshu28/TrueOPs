using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [Header("All Menu's")]
    public GameObject pauseMenuUI;
    public GameObject endGameMenuUI;
    public GameObject objectiveMenuUI;

    public static bool gameIsStopped = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsStopped )
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
            }
            
            else
            {
                Pause();
                Cursor.lockState = CursorLockMode.None;
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (gameIsStopped)
            {
                HideObjective();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                ShowObjective();
                Cursor.lockState= CursorLockMode.None;
            }
        }
    }

    public void ShowObjective()
    {
        objectiveMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsStopped = true;
    }

    public void HideObjective()
    {
        objectiveMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        gameIsStopped = false;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        gameIsStopped = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Mission");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        print("quit");
        Application.Quit();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsStopped = true;

    }
    
}
