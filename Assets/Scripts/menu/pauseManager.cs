using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseManager : MonoBehaviour
{
    public StateManager stateMan;

    public bool paused = false;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void PauseGame()
    {
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        stateMan.PauseGame();
    }

    public void ResumeGame()
    {
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        stateMan.ResumeGame();
    }

    public void MainMenu()
    {
        stateMan.ResumeGame();
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
