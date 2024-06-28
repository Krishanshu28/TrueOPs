using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void OnPlayButton()
    {
        SceneManager.LoadScene("Mission");
    }
    public void OnQuitButton()
    {
        print("quit");
        Application.Quit();
    }
}
