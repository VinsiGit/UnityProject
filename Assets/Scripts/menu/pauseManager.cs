using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseManager : MonoBehaviour
{
    public StateManager stateMan;

    public bool paused = false;

    private int initialScore = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                AudioListener.volume = 0f;
                PauseGame();
            }
            else
            {
                AudioListener.volume = 1f;
                ResumeGame();
            }
        }
    }

    private void Start()
    {
        initialScore = PlayerManager.Score;
    }

    void PauseGame()
    {
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        stateMan.PauseGame();
    }

    public void RestartGame()
    {
        AudioListener.volume = 1f;
        PlayerManager.Score = initialScore;
        stateMan.ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Cursor.lockState = CursorLockMode.Locked;
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
